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
        
        private Configs.Configs m_Configs;
        private int m_DifficultyModifier = 1;

        private IEnemySpawner m_EnemySpawner;
        private IFactoryActors m_FactoryActors;

        private GameObject m_Player;
        private int m_RoundCount;

        [Inject]
        private void Constructor(IEnemySpawner enemySpawner, Configs.Configs configs, IFactoryActors factoryActors)
        {
            m_EnemySpawner = enemySpawner;
            m_Configs = configs;
            m_FactoryActors = factoryActors;

            m_EnemySpawner.OnAllEnemyDestroyed += EndRound;
        }

        public void StartRound()
        {
            if (m_Player == null)
            {
                m_Player = m_FactoryActors.CreatePlayer(new Vector3(0, 0.1f, 0));
                m_Player.GetComponent<Player.Player>().OnDead = () => {
                    EndRound(TypeEndRound.PLAYER_LOST);
                };
            }

            if (!m_Player.activeSelf)
            {
                m_Player.SetActive(true);
                m_Player.GetComponent<Player.Player>().InitPlayer();
            }

            m_EnemySpawner.SpawnEnemy(m_FactoryActors, m_Configs.RoundsConfig.RoundParametersList[m_RoundCount], m_DifficultyModifier, m_Player);

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

                m_Player.transform.position = new Vector3(0, 1, 0);
                m_Player.SetActive(false);

                switch (typeEndRound)
                {
                    case TypeEndRound.END_ROUND:
                        if (m_RoundCount >= m_Configs.RoundsConfig.RoundParametersList.Count - 1)
                        {
                            m_RoundCount = 0;
                            m_DifficultyModifier++;
                            OnEndGame?.Invoke();
                        }
                        else
                        {
                            m_RoundCount++;
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
