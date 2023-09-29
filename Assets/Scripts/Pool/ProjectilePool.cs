using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private int poolSize;
        
        private List<GameObject> projectiles;
        private GameObject projectilePrefab;
        
        public void Start()
        {
            InitializePool();
        }
        
        private void InitializePool()
        {
            projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
            projectiles = new List<GameObject>();

            for (var i = 0; i < poolSize; i++)
            {
                var projectile = Instantiate(projectilePrefab, transform);
                projectile.SetActive(false);
                projectiles.Add(projectile);
            }
        }

        public GameObject GetProjectile()
        {
            foreach (var projectile in projectiles)
            {
                if (!projectile.activeInHierarchy)
                {
                    projectile.SetActive(true);
                    return projectile;
                }
            }
            
            var newProjectile = Instantiate(projectilePrefab, transform);
            projectiles.Add(newProjectile);
            return newProjectile;
        }

        public void ReturnProjectile(GameObject projectile)
        {
            projectile.SetActive(false);
        }
    }
}