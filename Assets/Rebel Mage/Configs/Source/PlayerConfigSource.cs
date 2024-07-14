using UnityEngine;

namespace PushItOut.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config")]
    public class PlayerConfigSource : ScriptableObject
    {
        public float Hp;
        public float MoveSpeed;
    }
}
