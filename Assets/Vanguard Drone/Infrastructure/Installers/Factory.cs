using System;
using UnityEngine;
using Vanguard_Drone.Configs;
using Vanguard_Drone.Enemy.Scripts;
using Zenject;

namespace Vanguard_Drone.Infrastructure
{
    public class Factory
    {
        private readonly DiContainer _diContainer;
        private readonly Prefabs _prefabs;
        private readonly Configs _configs;

        public Factory(DiContainer diContainer, Prefabs prefabs, Configs configs)
        {
            _diContainer = diContainer;
            _prefabs = prefabs;
            _configs = configs;
        }
        
        public GameObject CreatePlayer(Vector3 position)
        {
            GameObject player = GameObject.Instantiate(_prefabs.PlayerPref, position, Quaternion.identity, null);
            player.GetComponent<Player.Player>().InitPlayer();

            return player;
        }

        public GameObject CreateEnemy(EnemyType enemyType, Vector3 position, GameObject target)
        {
            switch (enemyType)
            {
                case EnemyType.BASE_ENEMY:
                    return CreateBaseEnemy(position, target);
                
                default:
                    Debug.LogError("Enemy type not found");
                    return null;
            }
        }
        
        private GameObject CreateBaseEnemy(Vector3 position, GameObject target)
        {
            GameObject baseEnemy = GameObject.Instantiate(_prefabs.BaseEnemyPref, position, Quaternion.identity, null);
            baseEnemy.GetComponent<Enemy.Enemy>().InitEnemy(_configs, target);

            return baseEnemy;
        }
    }
}
