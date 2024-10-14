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

        private readonly List<GameObject> m_EnemyOnScene = new();
        private int m_EnemyDestroyed;

        private IFactoryActors m_FactoryActors;
        private int m_PointsForRound;
        private Random m_Rnd;
        private SpawnOneSide m_SpawnOneSide;

        private void Awake()
        {
            m_Rnd = new Random();
            m_SpawnOneSide = new SpawnOneSide();
        }

        public void SpawnEnemy(IFactoryActors factoryActors, RoundsConfigSource.RoundParameters roundParameters, int difficultyModifier, GameObject target)
        {
            m_FactoryActors ??= factoryActors;
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
            for (int i = m_EnemyOnScene.Count - 1; i >= 0; i--)
            {
                Destroy(m_EnemyOnScene[i]);
            }

            m_EnemyOnScene.Clear();
            m_EnemyDestroyed = 0;

            StopAllCoroutines();
        }

        private void CreateEnemy(int enemyCount, EnemyType enemyType, SpawnType spawnType, GameObject target)
        {
            m_PointsForRound = 0;

            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 position = SetPositionEnemy(spawnType);

                GameObject enemy = m_FactoryActors.CreateEnemy(enemyType, position, target, SyncEnemyCount, out int pointsForEnemy);
                m_EnemyOnScene.Add(enemy);

                enemy.SetActive(false);
                m_PointsForRound += pointsForEnemy;
            }
        }

        private void SyncEnemyCount()
        {
            m_EnemyDestroyed++;

            if (m_EnemyDestroyed > m_EnemyOnScene.Count - 1)
            {
                OnAllEnemyDestroyed?.Invoke(m_PointsForRound);
            }
        }

        private Vector3 SetPositionEnemy(SpawnType spawnType)
        {
            switch (spawnType)
            {
                case SpawnType.CIRCLE:
                    int rand = m_Rnd.Next(0, 360);
                    float x = DISTANCE_SPAWN * Mathf.Cos(rand);
                    float z = DISTANCE_SPAWN * Mathf.Sin(rand);
                    return new Vector3(x, 1, z);

                case SpawnType.ONE_SIDE:
                    return m_SpawnOneSide.GetPositionSpawnOneSide(DISTANCE_SPAWN);

                default:
                    return new Vector3(DISTANCE_SPAWN, 1, 0);
            }
        }

        private IEnumerator SpawnWave()
        {
            foreach (GameObject enemy in m_EnemyOnScene)
            {
                enemy.SetActive(true);

                yield return new WaitForSeconds(0.2f);
            }

            yield return null;
        }
    }

    internal class SpawnOneSide
    {
        private const int COUNT_PER_SIDE = 3;
        private readonly Random m_Rnd;
        private int m_CurrentCountPerSide;
        private int m_CurrentRandAngle;

        public SpawnOneSide()
        {
            m_Rnd = new();
            m_CurrentRandAngle = m_Rnd.Next(0, 360);
        }

        public Vector3 GetPositionSpawnOneSide(float distanceSpawn)
        {
            if (m_CurrentCountPerSide < COUNT_PER_SIDE)
            {
                m_CurrentCountPerSide++;
                float angelWithOffset = m_CurrentRandAngle + m_Rnd.Next(-15, 15);
                angelWithOffset *= Mathf.Deg2Rad;
                float x = distanceSpawn * Mathf.Cos(angelWithOffset);
                float z = distanceSpawn * Mathf.Sin(angelWithOffset);
                return new Vector3(x, 1, z);
            }
            else
            {
                m_CurrentRandAngle = m_Rnd.Next(0, 360);
                m_CurrentCountPerSide = 0;
                return GetPositionSpawnOneSide(distanceSpawn);
            }
        }
    }

    internal interface IEnemySpawner
    {
        public Action<int> OnAllEnemyDestroyed { get; set; }

        void SpawnEnemy(IFactoryActors factoryActors, RoundsConfigSource.RoundParameters roundParameters, int difficultyModifier, GameObject target);
    }
}
