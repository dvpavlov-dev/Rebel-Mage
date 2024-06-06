using System.Collections.Generic;
using UnityEngine;
using Vanguard_Drone.Configs;
using Vanguard_Drone.Infrastructure;

namespace Vanguard_Drone.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private List<GameObject> _enemyOnScene = new();
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
                    _factory.CreateEnemy(enemyParameters.EnemyType, position, _target);
                }
            }
        }

        private Vector3 SetPositionEnemy(SpawnType spawnType)
        {
            switch (spawnType)
            {
                case SpawnType.CIRCLE:
                    return new Vector3(0, 0, 0);
                    break;
            
                case SpawnType.ONE_SIDE:
                    return new Vector3(0, 0, 0);
                    break;
            
                default:
                    return new Vector3(0, 0, 0);
            }
        }
    }
}
