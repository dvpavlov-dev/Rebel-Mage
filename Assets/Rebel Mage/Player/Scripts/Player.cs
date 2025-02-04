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

        private IDamage _dmgController;
        private PlayerConfigSource _playerConfig;

        [Inject]
        private void Constructor(Infrastructure.Configs configs)
        {
            _playerConfig = configs.PlayerConfig;
        }

        public void InitPlayer()
        {
            _dmgController = GetComponent<IDamage>();
            _dmgController.InitHealthPoints(_playerConfig.Hp);
            _dmgController.OnDead += OnDeadAction;
            GetComponent<PlayerMoveController>().Init(_playerConfig);
        }
        
        private void OnDeadAction()
        {
            OnDead?.Invoke();
        }

        private void OnDestroy()
        {
            _dmgController.OnDead -= OnDead;
        }
    }
}
