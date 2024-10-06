using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public abstract class Spell : MonoBehaviour
    {
        protected abstract SpellConfig Config { get; }
        
        protected GameObject _owner;

        public void SetOwner(GameObject owner)
        {
            _owner = owner;
        }
    }

}
