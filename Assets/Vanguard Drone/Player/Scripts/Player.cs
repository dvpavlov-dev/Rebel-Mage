using PushItOut.Configs;
using PushItOut.Spell_system;
using UnityEngine;
using Vanguard_Drone.Infrastructure;
using Zenject;

namespace Vanguard_Drone.Player
{
    [RequireComponent(typeof(DamageController), typeof(PlayerController))]
    public class Player : MonoBehaviour
    {
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
    }
}
