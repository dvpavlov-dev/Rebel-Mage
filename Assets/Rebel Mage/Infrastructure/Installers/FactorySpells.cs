using Rebel_Mage.Configs.Source;
using Rebel_Mage.Spell_system;
using UnityEngine;

namespace Rebel_Mage.Infrastructure
{
    public class FactorySpells : IFactorySpells
    {
        private IFactorySpells _factorySpellsImplementation;
        
        public void CastSpell(GameObject owner, Animator animator, Transform spellPoint, SpellConfig spellConfig)
        {
            switch (spellConfig)
            {
                case FireBallConfigSource config:
                    CastFireBall(owner, animator, spellPoint, config);
                    break;
                
                case IceBallConfigSource config:
                    CastIceBall(owner, animator, spellPoint, config);
                    break;
                
                case MagicSurgeConfigSource config:
                    CastMagicSurge(owner, animator, spellPoint, config);
                    break;
                
                default:
                    Debug.LogError("The config was not found");
                    break;
            }
        }

        private static void CastFireBall(GameObject owner, Animator animator, Transform spellPoint, FireBallConfigSource config)
        {
            FireBallController controller = owner.GetComponent<FireBallController>();
            if (controller == null)
            {
                controller = (FireBallController)owner.AddComponent(typeof(FireBallController));
                controller.Constructor(owner, animator, spellPoint, config);
            }
            
            controller.CastSpell();
        }
        
        private static void CastIceBall(GameObject owner, Animator animator, Transform spellPoint, IceBallConfigSource config)
        {
            IceBallController controller = owner.GetComponent<IceBallController>();
            if (controller == null)
            {
                controller = (IceBallController)owner.AddComponent(typeof(IceBallController));
                controller.Constructor(owner, animator, spellPoint, config);
            }
            
            controller.CastSpell();
        }

        private static void CastMagicSurge(GameObject owner, Animator animator, Transform spellPoint, MagicSurgeConfigSource config)
        {
            MagicSurgeController controller = owner.GetComponent<MagicSurgeController>();
            if (controller == null)
            {
                controller = (MagicSurgeController)owner.AddComponent(typeof(MagicSurgeController));
                controller.Constructor(owner, animator, spellPoint, config);
            }
            
            controller.CastSpell();
        }
    }

    public interface IFactorySpells
    {
        public void CastSpell(GameObject owner, Animator animator, Transform spellPoint, SpellConfig spellConfig);
    }
}