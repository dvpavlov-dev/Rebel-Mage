using System;
using PushItOut.Configs;
using PushItOut.Spell_system;
using UnityEngine;
using Zenject;

namespace Vanguard_Drone.Player
{
    [RequireComponent(typeof(DamageController), typeof(PlayerController))]
    public class Player : MonoBehaviour
    {
        public Action OnDead;
        
        private PlayerConfigSource _playerConfig;
    
        [Inject]
        private void Constructor(Infrastructure.Configs configs)
        {
            _playerConfig = configs.PlayerConfig;
        }

        public void InitPlayer()
        {
            GetComponent<DamageController>().InitHealthPoints(_playerConfig.Hp);
            GetComponent<PlayerController>().SetupPlayerController(_playerConfig.MoveSpeed);
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            
            OnDead?.Invoke();
        }
    }
}
