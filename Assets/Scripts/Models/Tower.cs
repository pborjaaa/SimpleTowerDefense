using System;
using Controllers;
using UnityEditor;
using UnityEngine;

namespace Models
{
    public class Tower : MonoBehaviour
    {
        public int damage;
        public int range;
        public int shieldDamage;
        public float attackCooldown;
        public ProjectilePool projectilePool;
        
        private float lastAttackTime;

        private void Start()
        {
            projectilePool = FindObjectOfType<ProjectilePool>();
        }

        private void Update()
        {
            CheckEnemies();
        }

        private void CheckEnemies()
        {
            var colliders = Physics.OverlapSphere(transform.position, range);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    if (collider && Time.time - lastAttackTime >= attackCooldown)
                    {
                        ShootAtEnemy(collider.gameObject);
                        lastAttackTime = Time.time;
                    }
                }
            }
        }

        private void ShootAtEnemy(GameObject enemy)
        {
            var projectile = projectilePool.GetProjectile();
            projectile.transform.position = transform.position;
            
            var projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetProjectile(enemy.transform, damage, shieldDamage);
            }
        }
        
        private void OnDrawGizmos()
        {
            Handles.color = new Color(1f, 0f, 0f, 0.2f);
            Handles.DrawSolidDisc(transform.position, Vector3.forward, range);
        }
    }

    public enum TowerType
    {
        NORMAL,
        LONG_RANGE,
        HIGH_DAMAGE,
        ANTI_SHIELD
    }
}