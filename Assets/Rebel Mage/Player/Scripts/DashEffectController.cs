using UnityEngine;

namespace Rebel_Mage.Player
{
    public class DashEffectController : MonoBehaviour
    {
        void Start()
        {
            Invoke(nameof(DestroyEffect), 1f);
        }

        private void DestroyEffect()
        {
            Destroy(gameObject);
        }
    }
}
