using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Pool
{
    public class EnemyPool
    {
        private List<GameObject> enemyPool = new();
        private Dictionary<EnemyType, GameObject> enemyPrefabs = new();

        public EnemyPool()
        {
            enemyPrefabs.Add(EnemyType.NORMAL, Resources.Load<GameObject>("Prefabs/NormalEnemy"));
            enemyPrefabs.Add(EnemyType.FAST, Resources.Load<GameObject>("Prefabs/FastEnemy"));
            enemyPrefabs.Add(EnemyType.HIGH_HP, Resources.Load<GameObject>("Prefabs/HighHpEnemy"));
            enemyPrefabs.Add(EnemyType.SHIELDED, Resources.Load<GameObject>("Prefabs/ShieldedEnemy"));
        }

        public GameObject GetEnemy(EnemyType type)
        {
            foreach (var enemyInstance in enemyPool)
            {
                var enemy = enemyInstance.GetComponent<Enemy>();
                if (enemy != null && enemy.enemyType == type && !enemyInstance.activeSelf)
                {
                    enemyInstance.SetActive(true);
                    return enemyInstance;
                }
            }
            
            if (enemyPrefabs.TryGetValue(type, out var prefab))
            {
                var enemyInstance = Object.Instantiate(prefab);
                enemyPool.Add(enemyInstance);
                return enemyInstance;
            }

            return null;
        }

        public void ReturnEnemy(GameObject enemy)
        {
            enemy.transform.position = default;
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }

        public bool EnemiesDead()
        {
            foreach (var enemy in enemyPool)
            {
                if (enemy.activeSelf)
                {
                    return false;
                }
            }
            return true;
        }
    }
}