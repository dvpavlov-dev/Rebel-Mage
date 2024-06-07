using UnityEngine;
using Vanguard_Drone.Enemy;
using Zenject;

namespace Vanguard_Drone.Infrastructure
{
    public class RoundProcess : MonoBehaviour
    {
        private EnemySpawner _enemySpawner;
        private Configs _configs;

        private int roundCount = 0;

        public bool IsRoundInProgress { get; set; }

        [Inject]
        private void Constructor(EnemySpawner enemySpawner, Configs configs)
        {
            _enemySpawner = enemySpawner;
            _configs = configs;
        }
        
        public void StartRound()
        {
            _enemySpawner.SpawnEnemy(_configs.RoundsConfig.RoundParametersList[roundCount]);
            IsRoundInProgress = true;
        }
    }
}
