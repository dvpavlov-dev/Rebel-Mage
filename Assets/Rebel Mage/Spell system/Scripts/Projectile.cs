using UnityEngine;

namespace PushItOut.Spell_system
{
    public abstract class Projectile : MonoBehaviour
    {
        protected GameObject _owner;

        public void SetOwner(GameObject owner)
        {
            _owner = owner;
        }
    }
}
