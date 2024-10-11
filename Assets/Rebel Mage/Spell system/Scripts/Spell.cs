using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public abstract class Spell<T> : MonoBehaviour
    {
        protected T Config { get; private set; }
        
        protected GameObject Owner;

        public void Constructor(GameObject owner, T config)
        {
            Owner = owner;
            Config = config;
        }
    }

}
