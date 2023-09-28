using UnityEngine;

namespace Models
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 10.0f;
        
        private int damage;
        private int shieldDamage;
        private Transform target;

        public void SetProjectile(Transform newTarget, int damage, int shieldDamage)
        {
            target = newTarget;
            this.damage = damage;
            this.shieldDamage = shieldDamage;
        }

        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 moveDirection = (target.position - transform.position).normalized;
            transform.Translate(moveDirection * speed * Time.deltaTime);

            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < 0.1f)
            {
                ApplyDamage(target.gameObject);
                Destroy(gameObject);
            }
        }

        private void ApplyDamage(GameObject enemy)
        {
            // Aquí puedes implementar la lógica para aplicar daño al enemigo
            // Por ejemplo, puedes tener una clase Enemy con un método TakeDamage
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage, shieldDamage);
            }
        }
    }
}