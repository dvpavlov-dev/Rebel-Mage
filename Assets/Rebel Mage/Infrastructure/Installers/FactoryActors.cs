using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Rebel_Mage.Configs;
using Rebel_Mage.Enemy;
using Object = UnityEngine.Object;
using R3;
using Rebel_Mage.UI;

namespace Rebel_Mage.Infrastructure
{
    public class FactoryActors : IFactoryActors
    {
        private readonly Prefabs _prefabs;
        private readonly Configs _configs;
        private readonly IUIFactory _uIFactory;
        private readonly Dictionary<EnemyType, Queue<GameObject>> _enemyPools = new();

        private CompositeDisposable _disposable = new();
        private Transform _containerForEnemy;

        public FactoryActors(Prefabs prefabs, Configs configs, IUIFactory uIFactory)
        {
            _prefabs = prefabs;
            _configs = configs;
            _uIFactory = uIFactory;

            Application.quitting += OnGameQuit;
        }

        public GameObject CreatePlayer(Vector3 position)
        {
            GameObject player = Object.Instantiate(_prefabs.PlayerPref, position, Quaternion.identity, null);
            player.GetComponent<Player.Player>().InitPlayer();

            return player;
        }

        public GameObject CreateEnemy(EnemyType enemyType, Vector3 position, GameObject target, Action onDead, out int pointsForEnemy)
        {
            switch (enemyType)
            {
                case EnemyType.BASE_ENEMY:
                    return CreateBaseEnemy(position, target, onDead, out pointsForEnemy);
                
                case EnemyType.MELEE_ENEMY:
                    return CreateMeleeEnemy(position, target, onDead, out pointsForEnemy);
                
                default:
                    Debug.LogError("Enemy type not found");
                    pointsForEnemy = 0;
                    return null;
            }
        }

        public void InitFactoryActors(Action onEndInitialize)
        {
            if (_enemyPools.Count == 0)
            {
                Observable.FromAsync(CreatePool)
                    .Subscribe(
                        onNext: _ =>
                        {
                            onEndInitialize?.Invoke();
                        }
                    ).AddTo(_disposable);
            }
            else
            {
                onEndInitialize?.Invoke();
            }
        }

        private GameObject CreateEnemy(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.BASE_ENEMY:
                    return Object.Instantiate(_configs.EnemyConfig.BaseEnemy.EnemyPref, _containerForEnemy);

                case EnemyType.MELEE_ENEMY:
                    return Object.Instantiate(_configs.EnemyConfig.MeleeEnemy.EnemyPref, _containerForEnemy);

                default:
                    Debug.LogError("Enemy type not found");
                    return null;
            }
        }

        public void DisposeEnemy(EnemyType enemyType, GameObject enemy)
        {
            if (_enemyPools.TryGetValue(enemyType, out Queue<GameObject> enemyPool))
            {
                enemyPool.Enqueue(enemy);
                enemy.SetActive(false);
            }
        }

        private GameObject GetEnemy(EnemyType enemyType)
        {
            GameObject enemy;
            
            if (_enemyPools.TryGetValue(enemyType, out Queue<GameObject> enemyPool) && enemyPool.Count != 0)
            {
                enemy = enemyPool.Dequeue();
            }
            else
            {
                enemy = CreateEnemy(enemyType);
                
                if (!_enemyPools.ContainsKey(enemyType))
                {
                    _enemyPools.Add(enemyType, new Queue<GameObject>());
                }
            }
            
            enemy.SetActive(false);

            return enemy;
        }

        private async ValueTask CreatePool(CancellationToken cancellationToken)
        {
            _containerForEnemy = new GameObject().transform;
            _containerForEnemy.name = "[CONTAINER FOR ENEMY]";

            try
            {
                Debug.Log($"Loading started");

                LoadingCurtains loadingCurtains = _uIFactory.CreateLoadingCurtains();
                
                float objectsCount = CalculateObjectsCount();
                float objectsSpawned = 0;
                
                loadingCurtains.Show();
                loadingCurtains.UpdateDescription($"Идет загрузка врагов, подождите...");

                foreach (RoundsConfigSource.RoundParameters roundParameters in _configs.RoundsConfig.RoundParametersList)
                {
                    foreach (RoundsConfigSource.EnemyParameters parameters in roundParameters.EnemyParameters)
                    {
                        _enemyPools.TryAdd(parameters.EnemyType, new Queue<GameObject>());

                        if (_enemyPools.TryGetValue(parameters.EnemyType, out Queue<GameObject> enemyPool))
                        {
                            for (int i = enemyPool.Count; i < parameters.EnemyCount; i++)
                            {
                                GameObject enemy = CreateEnemy(parameters.EnemyType);
                                enemy.SetActive(false);

                                _enemyPools[parameters.EnemyType].Enqueue(enemy);

                                objectsSpawned++;
                                loadingCurtains.UpdateProgress((float)Math.Truncate(objectsSpawned/objectsCount * 100));

                                Debug.Log($"Create {parameters.EnemyType}, count: {enemyPool.Count}");
                                
                                cancellationToken.ThrowIfCancellationRequested();
                                await Task.Yield();
                            }
                        }
                    }
                }
                
                loadingCurtains.Hide();
                Debug.Log($"Loading completed");
            }
            catch (Exception ex)
            {
                foreach (EnemyType enemyPool in _enemyPools.Keys)
                {
                    foreach (GameObject enemy in _enemyPools[enemyPool])
                    {
                        Object.DestroyImmediate(enemy);
                    }
                }
                
                Debug.LogError($"Task canceled, reason: {ex}");
            }
        }
        
        private float CalculateObjectsCount()
        {
            float objectsCount = 0;
            
            foreach (RoundsConfigSource.RoundParameters roundParameters in _configs.RoundsConfig.RoundParametersList)
            {
                foreach (RoundsConfigSource.EnemyParameters parameters in roundParameters.EnemyParameters)
                {
                    objectsCount += parameters.EnemyCount;
                }
            }
            
            return objectsCount;
        }

        private GameObject CreateMeleeEnemy(Vector3 position, GameObject target, Action onDead, out int pointsForEnemy)
        {
            GameObject meleeEnemy = GetEnemy(EnemyType.MELEE_ENEMY);
            meleeEnemy.transform.position = position;
            
            MeleeEnemy enemyController = meleeEnemy.GetComponent<MeleeEnemy>();
            enemyController.InitEnemy(_configs, target, onDead, this);
            pointsForEnemy = enemyController.PointsForEnemy;
            
            return meleeEnemy;
        }

        private GameObject CreateBaseEnemy(Vector3 position, GameObject target, Action onDead, out int pointsForEnemy)
        {
            GameObject baseEnemy = GetEnemy(EnemyType.BASE_ENEMY);
            baseEnemy.transform.position = position;
            
            var enemyController = baseEnemy.GetComponent<BaseEnemy>();
            enemyController.InitEnemy(_configs, target, onDead, this);
            pointsForEnemy = enemyController.PointsForEnemy;

            return baseEnemy;
        }
        
        private void OnGameQuit()
        {
            _disposable.Dispose();
        }
    }

    public interface IFactoryActors
    {
        void InitFactoryActors(Action onEndInitialize);

        void DisposeEnemy(EnemyType enemyType, GameObject enemy);
        
        GameObject CreatePlayer(Vector3 position);

        GameObject CreateEnemy(EnemyType enemyType, Vector3 position, GameObject target, Action onDead, out int pointsForEnemy);
    }
}
