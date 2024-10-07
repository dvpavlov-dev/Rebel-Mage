using Rebel_Mage.Configs.Source;
using UnityEngine;

namespace Rebel_Mage.Spell_system
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BallSpell : AttackSpell
    {
        private BallConfigSource BallConfig => Config as BallConfigSource;

        void Start()
        {
            Invoke(nameof(DestroyThisObject), 5);
        }

        private void Update()
        {
            MoveProjectile(BallConfig);
        }

        private void MoveProjectile(BallConfigSource config)
        {
            transform.Translate(new Vector3(0, 0, 1) * config.Speed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded) return;

            OnDestroyProjectile();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _owner)
                return;

            HitHandling(other.gameObject);

            DestroyThisObject();
        }

        protected override void HitHandling(GameObject other)
        {
            if (other.GetComponent<IDamage>() is {} damageController)
            {
                damageController.TakeDamage(BallConfig.Damage / 2);
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
