using System;
using Controllers;
using Pool;
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

            var targetVelocity = new Vector3(target.GetComponent<Enemy>().speed, 0, 0);
            var targetPosition = CalculatePosition(target.position, targetVelocity);
            moveDirection = (targetPosition - transform.position).normalized;
        }
        
        private Vector3 CalculatePosition(Vector3 targetPosition, Vector3 targetVelocity)
        {
            var toTarget = targetPosition - transform.position;
            var relativeVelocity = targetVelocity - speed * toTarget.normalized;
            var timeToImpact = toTarget.magnitude / relativeVelocity.magnitude;
            var futurePosition = targetPosition + targetVelocity * timeToImpact;

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