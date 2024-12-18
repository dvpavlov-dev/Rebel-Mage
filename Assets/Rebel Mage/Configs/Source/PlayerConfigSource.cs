using UnityEngine;

namespace Rebel_Mage.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config")]
    public class PlayerConfigSource : ScriptableObject
    {
        [Header("Move")]
        public float Hp;
        public float MoveSpeed;
        
        [Header("Dash")]
        public float Dash_CooldownTime;
    }
}
