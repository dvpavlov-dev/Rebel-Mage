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
        private int _difficultyModifier;

        public bool IsRoundInProgress { get; private set; }

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
                _player.GetComponent<Player.Player>().OnDead = () => {
                    EndRound(TypeEndRound.PLAYER_LOST);
                };
            }

            if (!_player.activeSelf)
            {
                _player.SetActive(true);
                _player.GetComponent<Player.Player>().InitPlayer();
            }

            Debug.Log($"_roundCount: {_roundCount}");
            _enemySpawner.InitEnemySpawner(_factory, _player);
            _enemySpawner.SpawnEnemy(_configs.RoundsConfig.RoundParametersList[_roundCount]);

            IsRoundInProgress = true;
        }

        private void EndRound()
        {
            EndRound(TypeEndRound.END_ROUND);
        }

        private void EndRound(TypeEndRound typeEndRound)
        {
            if (IsRoundInProgress)
            {
                IsRoundInProgress = false;

                _enemySpawner.ClearEnemyList();
                _player.transform.position = new Vector3(0, 1, 0);

                switch (typeEndRound)
                {
                    case TypeEndRound.END_ROUND:
                        if (_roundCount >= _configs.RoundsConfig.RoundParametersList.Count - 1)
                        {
                            _roundCount = 0;
                            _difficultyModifier++;
                            OnEndGame?.Invoke();
                        }
                        else
                        {
                            _roundCount++;
                            OnEndRound?.Invoke();
                        }
                        break;

                    case TypeEndRound.PLAYER_LOST:
                        OnPlayerLost?.Invoke();
                        break;
                }
            }
        }
    }

    internal enum TypeEndRound
    {
        END_ROUND,
        PLAYER_LOST,
    }
}
