using System;
using PushItOut.Configs;
using PushItOut.Spell_system;
using UnityEngine;
using Zenject;

namespace Vanguard_Drone.Player
{
    [RequireComponent(typeof(DamageController), typeof(PlayerMoveController))]
    [RequireComponent(typeof(ZenAutoInjecter), typeof(PlayerSpellController))]
    [RequireComponent(typeof(PlayerUIController))]
    public class Player : MonoBehaviour
    {
        public Action OnDead;
        
        private PlayerConfigSource m_PlayerConfig;

        [Inject]
        private void Constructor(Infrastructure.Configs configs)
        {
            m_PlayerConfig = configs.PlayerConfig;
        }

        public void InitPlayer()
        {
            GetComponent<DamageController>().InitHealthPoints(m_PlayerConfig.Hp);
            GetComponent<PlayerMoveController>().Init(m_PlayerConfig);
            GetComponent<PlayerSpellController>().Init();
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            
            OnDead?.Invoke();
        }
    }
}
