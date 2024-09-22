using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Infrastructure
{
    public class FactorySpells : IFactorySpells
    {
        private IFactorySpells m_FactorySpellsImplementation;
        public void CastSpell<T>(GameObject owner, Animator animator, Transform spellPoint, T spellConfig) where T : SpellConfig
        {
            switch (spellConfig)
            {
                case FireBallConfigSource config:
                    CastFireBall(owner, animator, spellPoint, config);
                    break;
                
                case IceBallConfigSource config:
                    CastIceBall(owner, animator, spellPoint, config);
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
            
            controller.CastFireBall();
        }
        
        private static void CastIceBall(GameObject owner, Animator animator, Transform spellPoint, IceBallConfigSource config)
        {
            IceBallController controller = owner.GetComponent<IceBallController>();
            if (controller == null)
            {
                controller = (IceBallController)owner.AddComponent(typeof(IceBallController));
                controller.Constructor(owner, animator, spellPoint, config);
            }
            
            controller.CastIceBall();
        }
    }

    public interface IFactorySpells
    {
        public void CastSpell<T>(GameObject owner, Animator animator, Transform spellPoint, T spellConfig) where T : SpellConfig;
    }
}