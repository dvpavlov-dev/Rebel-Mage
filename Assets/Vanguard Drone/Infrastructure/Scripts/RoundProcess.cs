using System;
using UnityEngine;
using Vanguard_Drone.Enemy;
using Zenject;

namespace Vanguard_Drone.Infrastructure
{
    public class RoundProcess : MonoBehaviour
    {
        public Action OnEndRound;
        public Action OnEndGame;
        public Action OnPlayerLost;

        private EnemySpawner _enemySpawner;
        private Configs _configs;
        private Factory _factory;

        private GameObject _player;
        private int _roundCount;

        public bool IsRoundInProgress { get; set; }

        [Inject]
        private void Constructor(EnemySpawner enemySpawner, Configs configs, Factory factory)
        {
            _enemySpawner = enemySpawner;
            _configs = configs;
            _factory = factory;

            _enemySpawner.OnAllEnemyDestroyed += EndRound;
        }

        public void StartRound()
        {
            if (_player == null)
            {
                _player = _factory.CreatePlayer(new Vector3(0, 1, 0));
                _player.GetComponent<Player.Player>().OnDead = () => 
                {
                    _enemySpawner.ClearEnemyList();
                    OnPlayerLost?.Invoke();
                };
            }

            _player.SetActive(true);
            _enemySpawner.InitEnemySpawner(_factory, _player);
            _enemySpawner.SpawnEnemy(_configs.RoundsConfig.RoundParametersList[_roundCount]);

            IsRoundInProgress = true;
        }

        private void EndRound()
        {
            _enemySpawner.ClearEnemyList();
            _player.SetActive(false);
            _player.transform.position = new Vector3(0, 1, 0);
            IsRoundInProgress = false;

            if (_roundCount >= _configs.RoundsConfig.RoundParametersList.Count)
            {
                OnEndGame?.Invoke();
            }
            else
            {
                _roundCount++;
                OnEndRound?.Invoke();
            }
        }
    }
}
