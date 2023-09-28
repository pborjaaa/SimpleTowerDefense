using Controllers;
using UnityEngine;

namespace Models
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 10.0f;
        
        private int damage;
        private int shieldDamage;
        private Transform target;
        private ProjectilePool projectilePool;

        public void SetProjectile(Transform newTarget, int damage, int shieldDamage)
        {
            target = newTarget;
            this.damage = damage;
            this.shieldDamage = shieldDamage;
        }
        
        private void Start()
        {
            projectilePool = FindObjectOfType<ProjectilePool>();
        }

        private void Update()
        {
            ProjectileMovement();
        }

        private void ProjectileMovement()
        {
            if (target == null)
            {
                projectilePool.ReturnProjectile(gameObject);
                return;
            }

            var moveDirection = (target.position - transform.position).normalized;
            transform.Translate(moveDirection * speed * Time.deltaTime);

            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < 0.1f)
            {
                ApplyDamage(target.gameObject);
            }
        }

        private void ApplyDamage(GameObject enemy)
        {
            var enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage, shieldDamage, gameObject);
                projectilePool.ReturnProjectile(gameObject);
            }
        }
    }
}