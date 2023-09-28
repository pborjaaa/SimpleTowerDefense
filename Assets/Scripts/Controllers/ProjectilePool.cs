using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class ProjectilePool : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public int poolSize;
        
        private List<GameObject> projectiles;

        public void Start()
        {
            InitializePool();
        }
        
        private void InitializePool()
        {
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

            return null;
        }

        public void ReturnProjectile(GameObject projectile)
        {
            projectile.SetActive(false);
        }
    }
}