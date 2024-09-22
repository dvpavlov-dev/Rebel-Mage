using Rebel_Mage.Configs.Source;
using UnityEngine;

public abstract class SpellController : MonoBehaviour
{
    protected GameObject m_Owner;
    protected Animator m_Animator;
    protected SpellConfig m_Config;
    protected Transform m_SpellPoint;
    
    public void Constructor(GameObject owner, Animator animator, Transform spellPoint, SpellConfig config)
    {
        m_Owner = owner;
        m_Animator = animator;
        m_Config = config;
        m_SpellPoint = spellPoint;
    }
}
