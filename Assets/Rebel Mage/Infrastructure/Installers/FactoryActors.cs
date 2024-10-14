using System;
using UnityEngine;
using Rebel_Mage.Configs;
using Rebel_Mage.Enemy;

namespace Rebel_Mage.Infrastructure
{
    public class FactoryActors : IFactoryActors
    {
        private readonly Prefabs m_Prefabs;
        private readonly Configs.Configs m_Configs;

        public FactoryActors(Prefabs prefabs, Configs.Configs configs)
        {
            m_Prefabs = prefabs;
            m_Configs = configs;
        }
        
        public GameObject CreatePlayer(Vector3 position)
        {
            GameObject player = GameObject.Instantiate(m_Prefabs.PlayerPref, position, Quaternion.identity, null);
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
        private GameObject CreateMeleeEnemy(Vector3 position, GameObject target, Action onDead, out int pointsForEnemy)
        {
            GameObject meleeEnemy = GameObject.Instantiate(m_Prefabs.MeleeEnemyPref, position, Quaternion.identity, null);
            
            MeleeEnemy enemyController = meleeEnemy.GetComponent<MeleeEnemy>();
            enemyController.InitEnemy(m_Configs, target, onDead);
            pointsForEnemy = enemyController.PointsForEnemy;
            
            return meleeEnemy;
        }

        private GameObject CreateBaseEnemy(Vector3 position, GameObject target, Action onDead, out int pointsForEnemy)
        {
            GameObject baseEnemy = GameObject.Instantiate(m_Prefabs.BaseEnemyPref, position, Quaternion.identity, null);
            var enemyController = baseEnemy.GetComponent<BaseEnemy>();
            enemyController.InitEnemy(m_Configs, target, onDead);
            pointsForEnemy = enemyController.PointsForEnemy;

            return baseEnemy;
        }
    }

    public interface IFactoryActors
    {
        GameObject CreatePlayer(Vector3 position);

        GameObject CreateEnemy(EnemyType enemyType, Vector3 position, GameObject target, Action onDead, out int pointsForEnemy);
    }
}
