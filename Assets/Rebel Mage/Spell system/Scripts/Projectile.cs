using Rebel_Mage.Spell_system.Configs;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public abstract class Projectile : MonoBehaviour
    {
        protected abstract SpellConfig Config { get; }
        
        protected GameObject _owner;

        public void SetOwner(GameObject owner)
        {
            _owner = owner;
        }
    }
}
