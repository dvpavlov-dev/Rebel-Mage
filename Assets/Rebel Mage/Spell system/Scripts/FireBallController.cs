using Rebel_Mage.Spell_system;
using UnityEngine;

public class FireBallController : SpellController
{
    public void CastFireBall()
    {
        StartCast();
    }
    
    private void StartCast()
    {
        m_Animator.SetTrigger(m_Config.AnimationName);
        Invoke(nameof(SpawnProjectile), 0.6f);
    }

    private void SpawnProjectile()
    {
        GameObject projectile = Instantiate(m_Config.SpellPrefab, m_SpellPoint.position, m_SpellPoint.rotation);
        projectile.GetComponent<Projectile>().SetOwner(m_Owner);
    }
}
