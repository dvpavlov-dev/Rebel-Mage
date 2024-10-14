using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BallSpell<T> : AttackSpell<T> where T : BallConfigSource
    {
        void Start()
        {
            Invoke(nameof(DestroyThisObject), 5);
        }

        private void Update()
        {
            transform.Translate(new Vector3(0, 0, 1) * Config.Speed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded) return;

            OnDestroyProjectile();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Owner)
                return;

            HitHandling(other.gameObject);

            DestroyThisObject();
        }

        protected override void HitHandling(GameObject other)
        {
            if (other.GetComponent<IDamage>() is {} damageController)
            {
                damageController.TakeDamage(Config.Damage / 2);
            }

            base.HitHandling(other);
        }

        protected virtual void OnDestroyProjectile()
        {
            // _debugHitPos = transform.position;
        }

        private void DestroyThisObject()
        {
            Destroy(gameObject);
        }

        #region ForDebug

        // private Vector3 _debugHitPos;
        // void OnDrawGizmos()
        // {
        //     Gizmos.DrawSphere(_debugHitPos, BallConfig.ExplosionRadius);
        // }

        #endregion
    }
}
