using System;
using PushItOut.Spell_system.Configs;
using UnityEngine;
using Zenject;

namespace PushItOut.Spell_system
{
    public class SpellsAction : MonoBehaviour
    {
        public Transform SpellPoint;
        public Action<SpellConfig> OnSpellActivate;

        private Spells _spells;

        [Inject]
        public void Constructor(Spells spells)
        {
            _spells = spells;
        }

        public void UseSpell(TypeSpell typeSpell)
        {
            if (_spells.TryGetSpell(typeSpell, out SpellConfig useSpell))
            {
                if (_spells.CheckCooldown(typeSpell, useSpell))
                {
                    if (useSpell.SpellPrefab != null)
                    {
                        GameObject projectile = Instantiate(useSpell.SpellPrefab, SpellPoint.position, SpellPoint.rotation);
                        projectile.GetComponent<Projectile>().SetOwner(gameObject);
                    }
                    
                    OnSpellActivate?.Invoke(useSpell);
                }
            }
        }
    }
}