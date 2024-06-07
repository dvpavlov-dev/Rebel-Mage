using System;
using System.Collections.Generic;
using UnityEngine;
using Vanguard_Drone.Configs;
using Vanguard_Drone.Infrastructure;
using Random = UnityEngine.Random;

namespace Vanguard_Drone.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private readonly List<GameObject> _enemyOnScene = new();
        
        private Factory _factory;
        private GameObject _target;

        public void InitEnemySpawner(Factory factory, GameObject target)
        {
            _factory = factory;
            _target = target;
        }
    
        public void SpawnEnemy(RoundsConfigSource.RoundParameters roundParameters)
        {
            foreach (RoundsConfigSource.EnemyParameters enemyParameters in roundParameters.EnemyParameters)
            {
                for (int i = 0; i < enemyParameters.EnemyCount; i++)
                {
                    Vector3 position = SetPositionEnemy(enemyParameters.SpawnType); 
                    _enemyOnScene.Add(_factory.CreateEnemy(enemyParameters.EnemyType, position, _target));
                }
            }
        }

        private Vector3 SetPositionEnemy(SpawnType spawnType)
        {
            switch (spawnType)
            {
                case SpawnType.CIRCLE:
                    transform.rotation = new Quaternion(0, Random.Range(0,360), 0, 1);
                    return new Vector3(20, 1, 0);
                    break;
            
                case SpawnType.ONE_SIDE:
                    return new Vector3(20, 1, 0);
                    break;
            
                default:
                    return new Vector3(20, 1, 0);
            }
        }
    }
}
