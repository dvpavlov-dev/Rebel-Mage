using UnityEngine;
using Rebel_Mage.Configs;

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

        public GameObject CreateEnemy(EnemyType enemyType, Vector3 position, GameObject target)
        {
            switch (enemyType)
            {
                case EnemyType.BASE_ENEMY:
                    return CreateBaseEnemy(position, target);
                
                case EnemyType.MELEE_ENEMY:
                    return CreateMeleeEnemy(position, target);
                
                default:
                    Debug.LogError("Enemy type not found");
                    return null;
            }
        }
        private GameObject CreateMeleeEnemy(Vector3 position, GameObject target)
        {
            GameObject meleeEnemy = GameObject.Instantiate(m_Prefabs.MeleeEnemyPref, position, Quaternion.identity, null);
            meleeEnemy.GetComponent<Rebel_Mage.Enemy.Enemy>().InitEnemy(m_Configs, target);

            return meleeEnemy;
        }

        private GameObject CreateBaseEnemy(Vector3 position, GameObject target)
        {
            GameObject baseEnemy = GameObject.Instantiate(m_Prefabs.BaseEnemyPref, position, Quaternion.identity, null);
            baseEnemy.GetComponent<Rebel_Mage.Enemy.Enemy>().InitEnemy(m_Configs, target);

            return baseEnemy;
        }
    }

    public interface IFactoryActors
    {
        GameObject CreatePlayer(Vector3 position);

        GameObject CreateEnemy(EnemyType enemyType, Vector3 position, GameObject target);
    }
}
