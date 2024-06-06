using PushItOut.Configs.Source;
using UnityEngine;

namespace PushItOut.Spell_system
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BallProjectile : Projectile
    {
        public BallConfigSource BallConfig;

        void Start()
        {
            Invoke(nameof(DestroyThisObject), 5);
        }

        private void Update()
        {
            transform.Translate(new Vector3(0, 0, 1) * BallConfig.Speed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            if(!gameObject.scene.isLoaded) return;
            
            OnDestroyProjectile();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _owner)
                return;

            HitHandling(other.gameObject);

            DestroyThisObject();
        }

        private void HitHandling(GameObject other)
        {
            if (other.GetComponent<IDamage>() is {} damageController)
            {
                damageController.TakeDamage(BallConfig.Damage / 2);
            }

            foreach (Collider hit in Physics.OverlapSphere(transform.position, BallConfig.ExplosionRadius))
            {
                GameObject hitObject = hit.gameObject;

                ImpactOnObject(hitObject);
            }
        }
        protected virtual void ImpactOnObject(GameObject hitObject)
        {
            if (hitObject.GetComponent<IDamage>() is {} damageSystemHit && hitObject.gameObject != _owner)
            {
                damageSystemHit.TakeDamage(BallConfig.Damage / 2);
            }
        }

        protected virtual void OnDestroyProjectile()
        {
            _debugHitPos = transform.position;
        }

        private void DestroyThisObject()
        {
            Destroy(gameObject);
        }

        #region ForDebug

        private Vector3 _debugHitPos;
        void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_debugHitPos, BallConfig.ExplosionRadius);
        }

        #endregion
    }
}
