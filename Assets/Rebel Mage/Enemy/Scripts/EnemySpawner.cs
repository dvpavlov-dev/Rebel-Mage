using System;
using System.Collections;
using System.Collections.Generic;
using Rebel_Mage.Configs;
using UnityEngine;
using Random = System.Random;
using Rebel_Mage.Infrastructure;

namespace Rebel_Mage.Enemy
{
    public class EnemySpawner : MonoBehaviour, IEnemySpawner
    {
        public Action<int> OnAllEnemyDestroyed { get; set; }
        
        private const float DISTANCE_SPAWN = 30;

        private readonly List<GameObject> _enemyOnScene = new();
        
        private int _enemyDestroyed;
        private IActorsFactory _actorsFactory;
        private int _pointsForRound;
        private Random _rnd;
        private SpawnOneSide _spawnOneSide;

        private void Awake()
        {
            _rnd = new Random();
            _spawnOneSide = new SpawnOneSide();
        }

        public void SpawnEnemy(IActorsFactory actorsFactory, RoundsConfigSource.RoundParameters roundParameters, int difficultyModifier, GameObject target)
        {
            _actorsFactory ??= actorsFactory;
            ClearEnemyList();

            foreach (RoundsConfigSource.EnemyParameters enemyParameters in roundParameters.EnemyParameters)
            {
                CreateEnemy(enemyParameters.EnemyCount * difficultyModifier, enemyParameters.EnemyType, enemyParameters.SpawnType, target);
            }

            foreach (RoundsConfigSource.EnemyParameters _ in roundParameters.EnemyParameters)
            {
                StartCoroutine(SpawnWave());
            }
        }

        private void ClearEnemyList()
        {
            for (int i = _enemyOnScene.Count - 1; i >= 0; i--)
            {
                _enemyOnScene[i].SetActive(false);
            }

            _enemyOnScene.Clear();
            _enemyDestroyed = 0;

            StopAllCoroutines();
        }

        private void CreateEnemy(int enemyCount, EnemyType enemyType, SpawnType spawnType, GameObject target)
        {
            _pointsForRound = 0;

            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 position = SetPositionEnemy(spawnType);

                GameObject enemy = _actorsFactory.CreateEnemy(enemyType, position, target, SyncEnemyCount, out int pointsForEnemy);
                _enemyOnScene.Add(enemy);

                enemy.SetActive(false);
                _pointsForRound += pointsForEnemy;
            }
        }

        private void SyncEnemyCount()
        {
            _enemyDestroyed++;

            if (_enemyDestroyed > _enemyOnScene.Count - 1)
            {
                OnAllEnemyDestroyed?.Invoke(_pointsForRound);
            }
        }

        private Vector3 SetPositionEnemy(SpawnType spawnType)
        {
            switch (spawnType)
            {
                case SpawnType.CIRCLE:
                    int rand = _rnd.Next(0, 360);
                    float x = DISTANCE_SPAWN * Mathf.Cos(rand);
                    float z = DISTANCE_SPAWN * Mathf.Sin(rand);
                    return new Vector3(x, 1, z);

                case SpawnType.ONE_SIDE:
                    return _spawnOneSide.GetPositionSpawnOneSide(DISTANCE_SPAWN);

                default:
                    return new Vector3(DISTANCE_SPAWN, 1, 0);
            }
        }

        private IEnumerator SpawnWave()
        {
            WaitForSeconds waitForSeconds = new (0.2f);
            
            foreach (GameObject enemy in _enemyOnScene)
            {
                enemy.SetActive(true);

                yield return waitForSeconds;
            }

            yield return null;
        }
    }

    internal class SpawnOneSide
    {
        private const int COUNT_PER_SIDE = 3;
        private readonly Random _rnd;
        private int _currentCountPerSide;
        private int _currentRandAngle;

        public SpawnOneSide()
        {
            _rnd = new Random();
            _currentRandAngle = _rnd.Next(0, 360);
        }

        public Vector3 GetPositionSpawnOneSide(float distanceSpawn)
        {
            if (_currentCountPerSide < COUNT_PER_SIDE)
            {
                _currentCountPerSide++;
                float angelWithOffset = _currentRandAngle + _rnd.Next(-15, 15);
                angelWithOffset *= Mathf.Deg2Rad;
                float x = distanceSpawn * Mathf.Cos(angelWithOffset);
                float z = distanceSpawn * Mathf.Sin(angelWithOffset);
                return new Vector3(x, 1, z);
            }
            else
            {
                _currentRandAngle = _rnd.Next(0, 360);
                _currentCountPerSide = 0;
                return GetPositionSpawnOneSide(distanceSpawn);
            }
        }
    }

    public interface IEnemySpawner
    {
        public Action<int> OnAllEnemyDestroyed { get; set; }

        void SpawnEnemy(IActorsFactory actorsFactory, RoundsConfigSource.RoundParameters roundParameters, int difficultyModifier, GameObject target);
    }
}
