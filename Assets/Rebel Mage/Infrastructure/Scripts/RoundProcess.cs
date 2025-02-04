using System;
using Rebel_Mage.Enemy;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Infrastructure
{
    public class RoundProcess : MonoBehaviour, IRoundProcess
    {
        public Action OnEndRound { get; set; }
        public Action OnEndGame { get; set; }
        public Action OnPlayerLost { get; set; }

        public bool IsRoundInProgress { get; set; }
        public int PointsForAllRounds { get; set; }
        public int RoundsCompleted { get; set; }

        private int _difficultyModifier = 1;
        private int _roundCount;
        private Configs _configs;
        private IEnemySpawner _enemySpawner;
        private IFactoryActors _factoryActors;
        private GameObject _player;

        [Inject]
        private void Constructor(IEnemySpawner enemySpawner, Configs configs, IFactoryActors factoryActors)
        {
            _enemySpawner = enemySpawner;
            _configs = configs;
            _factoryActors = factoryActors;

            _enemySpawner.OnAllEnemyDestroyed += EndRound;
        }

        public void StartRound()
        {
            if (_player == null)
            {
                _player = _factoryActors.CreatePlayer(new Vector3(0, 0.1f, 0));
                _player.GetComponent<Player.Player>().OnDead = () => {
                    EndRound(TypeEndRound.PLAYER_LOST);
                };
            }

            if (!_player.activeSelf)
            {
                _player.SetActive(true);
                _player.GetComponent<Player.Player>().InitPlayer();
            }

            _enemySpawner.SpawnEnemy(_factoryActors, _configs.RoundsConfig.RoundParametersList[_roundCount], _difficultyModifier, _player);

            IsRoundInProgress = true;
        }

        private void EndRound(int pointsForRound)
        {
            PointsForAllRounds += pointsForRound;

            EndRound(TypeEndRound.END_ROUND);
        }

        private void EndRound(TypeEndRound typeEndRound)
        {
            if (IsRoundInProgress)
            {
                IsRoundInProgress = false;

                _player.transform.position = new Vector3(0,1,0);
                _player.SetActive(false);

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

                        RoundsCompleted++;
                        break;

                    case TypeEndRound.PLAYER_LOST:
                        OnPlayerLost?.Invoke();
                        break;
                    
                    default:
                        Debug.Log("Unknown type round");
                        break;
                }
            }
        }
    }
    
    public interface IRoundProcess
    {
        public Action OnEndRound { get; set; }
        public Action OnEndGame { get; set; }
        public Action OnPlayerLost { get; set; }

        public bool IsRoundInProgress { get; set; }
        public int PointsForAllRounds { get; set; }
        public int RoundsCompleted { get; set; }

        void StartRound();
    }

    internal enum TypeEndRound
    {
        END_ROUND,
        PLAYER_LOST,
    }
}
