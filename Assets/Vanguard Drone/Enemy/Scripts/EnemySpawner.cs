using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanguard_Drone.Configs;
using Vanguard_Drone.Infrastructure;
using Random = System.Random;

namespace Vanguard_Drone.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public Action OnAllEnemyDestroyed;
        
        private const float DISTANCE_SPAWN = 30;

        private readonly List<GameObject> _enemyOnScene = new();

        private Factory _factory;
        private Random _rnd;
        private SpawnOneSide _spawnOneSide;
        private GameObject _target;
        private int _enemyDestroyed;
        private int _difficultyModifier;
        
        public void InitEnemySpawner(Factory factory, GameObject target, int difficultyModifier)
        {
            _factory = factory;
            _target = target;

            _rnd = new Random();
            _spawnOneSide = new SpawnOneSide();

            _difficultyModifier = difficultyModifier;
        }

        public void SpawnEnemy(RoundsConfigSource.RoundParameters roundParameters)
        {
            foreach (RoundsConfigSource.EnemyParameters enemyParameters in roundParameters.EnemyParameters)
            {
                CreateEnemy(enemyParameters.EnemyCount * _difficultyModifier, enemyParameters.EnemyType, enemyParameters.SpawnType);
            }
            
            foreach (RoundsConfigSource.EnemyParameters _ in roundParameters.EnemyParameters)
            {
                StartCoroutine(SpawnWave());
            }
        }

        private void CreateEnemy(int enemyCount, EnemyType enemyType, SpawnType spawnType)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 position = SetPositionEnemy(spawnType);

                GameObject enemy = _factory.CreateEnemy(enemyType, position, _target);
                _enemyOnScene.Add(enemy);
                
                enemy.SetActive(false);
                enemy.GetComponent<Enemy>().OnDead += SyncEnemyCount;
            }
        }

        public void ClearEnemyList()
        {
            Debug.Log($"_enemyOnScene.Count: {_enemyOnScene.Count}");
            for (int i = _enemyOnScene.Count - 1; i >= 0; i--)
            {
                Destroy(_enemyOnScene[i]);
            }
            
            _enemyOnScene.Clear();
            _enemyDestroyed = 0;
            
            StopAllCoroutines();
        }

        private void SyncEnemyCount()
        {
            _enemyDestroyed++;

            if (_enemyDestroyed > _enemyOnScene.Count - 1)
            {
                OnAllEnemyDestroyed?.Invoke();
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
            foreach (GameObject enemy in _enemyOnScene)
            {
                enemy.SetActive(true);

                yield return new WaitForSeconds(0.2f);
            }

            yield return null;
        }
    }

    class SpawnOneSide
    {
        private readonly Random _rnd;

        private int _countPerSide = 3;
        private int _currentCountPerSide;
        private int _currentRandAngle;

        public SpawnOneSide()
        {
            _rnd = new();
            _currentRandAngle = _rnd.Next(0, 360);
        }

        public Vector3 GetPositionSpawnOneSide(float distanceSpawn)
        {
            if (_currentCountPerSide < _countPerSide)
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
}
