using System;
using Controllers;
using UnityEngine;

namespace Models
{
    public class Projectile : MonoBehaviour
    {
        public float speed;
        public int projectileLife;
        
        private int damage;
        private int shieldDamage;
        private Transform target;
        private ProjectilePool projectilePool;
        private Vector3 moveDirection;
        private float projectileTimer;
        
        public void SetProjectile(Transform newTarget, int damage, int shieldDamage)
        {
            target = newTarget;
            this.damage = damage;
            this.shieldDamage = shieldDamage;
            projectileTimer = 0;
            CalculateDirection();
        }
        
        private void Start()
        {
            projectilePool = FindObjectOfType<ProjectilePool>();
        }

        private void Update()
        {
            Movement();
            ValidateProjectileLife();
        }

        private void Movement()
        {
            transform.Translate(moveDirection * speed * Time.deltaTime);
        }

        private void ValidateProjectileLife()
        {
            projectileTimer += Time.deltaTime;
            
            if (projectileTimer >= projectileLife)
                projectilePool.ReturnProjectile(gameObject);
        }

        private void CalculateDirection()
        {
            if (target == null)
            {
                projectilePool.ReturnProjectile(gameObject);
                return;
            }

            var targetPosition = CalculatePosition(target.position, target.GetComponent<Enemy>().speed);
            moveDirection = (targetPosition - transform.position).normalized;
        }
        
        private Vector3 CalculatePosition(Vector3 targetPosition, float targetSpeed)
        {
            var timeToImpact = Vector3.Distance(transform.position, targetPosition) / speed;
            var futurePosition = targetPosition + targetSpeed * Vector3.right * timeToImpact;

            return futurePosition;
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

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<Enemy>())
                ApplyDamage(other.gameObject);
        }
    }
}