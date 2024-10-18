using System;
using Rebel_Mage.Configs;
using Rebel_Mage.Spell_system;
using UnityEngine;
using Zenject;

namespace Rebel_Mage.Player
{
    [RequireComponent(typeof(DamageController), typeof(PlayerMoveController))]
    [RequireComponent(typeof(ZenAutoInjecter), typeof(PlayerSpellController))]
    [RequireComponent(typeof(PlayerUIController))]
    public class Player : MonoBehaviour
    {
        public Action OnDead { get; set; }

        private IDamage m_DmgController;
        private PlayerConfigSource m_PlayerConfig;

        [Inject]
        private void Constructor(Configs.Configs configs)
        {
            m_PlayerConfig = configs.PlayerConfig;
        }

        public void InitPlayer()
        {
            m_DmgController = GetComponent<IDamage>();
            m_DmgController.InitHealthPoints(m_PlayerConfig.Hp);
            m_DmgController.OnDead += OnDeadAction;
            GetComponent<PlayerMoveController>().Init(m_PlayerConfig);
        }
        
        private void OnDeadAction()
        {
            OnDead?.Invoke();
        }

        private void OnDestroy()
        {
            m_DmgController.OnDead -= OnDead;
        }
    }
}
