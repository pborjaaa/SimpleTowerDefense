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
        public GameObject projectilePrefab;
        
        private float lastAttackTime;
        
        private void Update()
        {
            // Busca objetos dentro del rango de ataque en 3D
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // Comprueba si ha pasado suficiente tiempo desde el último ataque
                    if (Time.time - lastAttackTime >= attackCooldown)
                    {
                        // Dispara al enemigo
                        ShootAtEnemy(collider.gameObject);
                        lastAttackTime = Time.time;
                    }
                }
            }
        }

        private void ShootAtEnemy(GameObject enemy)
        {
            // Crea y dispara un proyectil hacia el enemigo
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetProjectile(enemy.transform, damage, shieldDamage);
            }
        }
        
        private void OnDrawGizmos()
        {
            // Dibuja un gizmo de esfera para representar el rango de ataque de la torre
            Handles.color = new Color(1f, 0f, 0f, 0.2f); // Color rojo transparente
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