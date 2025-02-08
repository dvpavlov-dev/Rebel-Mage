using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    public abstract class SpellController<T> : MonoBehaviour
    {
        protected Animator Animator;
        protected T Config;
        protected GameObject Owner;
        protected Transform SpellPoint;

        public void Constructor(GameObject owner, Animator animator, Transform spellPoint, T config)
        {
            Owner = owner;
            Animator = animator;
            Config = config;
            SpellPoint = spellPoint;
        }

        public abstract void CastSpell();
    }
}
