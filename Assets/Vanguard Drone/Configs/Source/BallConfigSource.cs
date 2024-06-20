using PushItOut.Spell_system.Configs;
using UnityEngine;

namespace PushItOut.Configs.Source
{
    public class BallConfigSource : SpellConfig
    {
        [Space]
        public float Damage;
        public float Speed;
        public float ExplosionRadius;
    }
}