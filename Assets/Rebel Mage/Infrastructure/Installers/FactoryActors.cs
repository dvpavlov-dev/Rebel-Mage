using System;
using System.Collections.Generic;
using UnityEngine;
using Rebel_Mage.Configs;
using Rebel_Mage.Enemy;
using Object = UnityEngine.Object;
using R3;

namespace Rebel_Mage.Infrastructure
{
    public class FactoryActors : IFactoryActors
    {
        private readonly Prefabs _prefabs;
        private readonly Configs.Configs _configs;
        private readonly Dictionary<EnemyType, Queue<GameObject>> _enemyPools = new();

        private Transform _containerForEnemy;

        public FactoryActors(Prefabs prefabs, Configs.Configs configs)
        {
            _prefabs = prefabs;
            _configs = configs;
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
                CreatePool(onEndInitialize);
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
                    return Object.Instantiate(_prefabs.MeleeEnemyPref, _containerForEnemy);

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
            GameObject enemy = null;
            
            if (_enemyPools.TryGetValue(enemyType, out Queue<GameObject> enemyPool))
            {
                enemy = enemyPool.Dequeue();
            }

            if (enemy == null)
            {
                enemy = CreateEnemy(enemyType);
            }

            return enemy;
        }

        private void CreatePool(Action onEndCreatedPool)
        {
            _containerForEnemy = new GameObject().transform;
            _containerForEnemy.name = "[CONTAINER FOR ENEMY]";
            
            // foreach (RoundsConfigSource.RoundParameters roundParameters in _configs.RoundsConfig.RoundParametersList)
            // {
            //     foreach (RoundsConfigSource.EnemyParameters parameters in roundParameters.EnemyParameters)
            //     {
            //         _enemyPools.TryAdd(parameters.EnemyType, new Queue<GameObject>());
            //         
            //         if (_enemyPools.TryGetValue(parameters.EnemyType, out Queue<GameObject> enemyPool))
            //         {
            //             for (int i = enemyPool.Count; i < parameters.EnemyCount; i++)
            //             {
            //                 GameObject enemy = CreateEnemy(parameters.EnemyType);
            //                 enemy.SetActive(false);
            //                 
            //                 _enemyPools[parameters.EnemyType].Enqueue(enemy); 
            //                 
            //                 Debug.Log($"Create {parameters.EnemyType}, count: {enemyPool.Count}");
            //             }
            //         }
            //     }
            // }
            
            // Debug.Log("Thread sleep start");
            // Thread.Sleep(10000);
            // Debug.Log("Thread sleep end");

            
            var arr = _configs.RoundsConfig.RoundParametersList.ToObservable();
            
            arr.Subscribe(
                roundParameters =>
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
                                
                                Debug.Log($"Create {parameters.EnemyType}, count: {enemyPool.Count}");
                            }
                        }
                    }
                });
            
            arr.(() => onEndCreatedPool?.Invoke());

        }

        private GameObject CreateMeleeEnemy(Vector3 position, GameObject target, Action onDead, out int pointsForEnemy)
        {
            GameObject meleeEnemy = GetEnemy(EnemyType.MELEE_ENEMY);
            meleeEnemy.transform.position = position;
            
            MeleeEnemy enemyController = meleeEnemy.GetComponent<MeleeEnemy>();
            enemyController.InitEnemy(_configs, target, onDead);
            pointsForEnemy = enemyController.PointsForEnemy;
            
            return meleeEnemy;
        }

        private GameObject CreateBaseEnemy(Vector3 position, GameObject target, Action onDead, out int pointsForEnemy)
        {
            GameObject baseEnemy = GetEnemy(EnemyType.BASE_ENEMY);
            baseEnemy.transform.position = position;
            
            var enemyController = baseEnemy.GetComponent<BaseEnemy>();
            enemyController.InitEnemy(_configs, target, onDead);
            pointsForEnemy = enemyController.PointsForEnemy;

            return baseEnemy;
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
