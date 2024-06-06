using UnityEngine;

namespace PushItOut.Spell_system
{
    public interface IImpact
    {
        void ExplosionImpact(Vector3 positionImpact, float maxDistance, float explosionForce);

        void ChangeSpeedImpact(float slowdownPercentage, float timeSlowdown);
    }
}
