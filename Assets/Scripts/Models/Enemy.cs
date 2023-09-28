using System;
using System.Collections.Generic;
using Controllers;
using Events;
using UnityEngine;

namespace Models
{
    public class Enemy : MonoBehaviour
    {
        public int health;
        public float speed;
        public int shield;
        public EnemyType enemyType;
        
        private int currentWaypointIndex = 0;
        private List<Transform> waypoints = new();
        private bool movingToWaypoints;
        private EnemyEscapedEvent EnemyEscapedEvent;
        private EnemyDeathEvent enemyDeathEvent;

        private void Update()
        {
            if (movingToWaypoints)
                MoveToWaypoint();
        }

        public void Setup(GameObject[] path, EnemyEscapedEvent enemyEscapedEvent, Transform startingPos, EnemyDeathEvent enemyDeathEvent)
        {
            EnemyEscapedEvent = enemyEscapedEvent;
            this.enemyDeathEvent = enemyDeathEvent;
            transform.position = startingPos.position;
            waypoints = new List<Transform>();
            
            foreach (var child in path)
            {
                waypoints.Add(child.transform);
            }

            movingToWaypoints = true;
        }
        
        private void MoveToWaypoint()
        {
            if (currentWaypointIndex < waypoints.Count)
            {
                var direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
                
                transform.Translate(direction * speed * Time.deltaTime);
                
                if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
                {
                    currentWaypointIndex++;
                    
                    if (currentWaypointIndex == waypoints.Count)
                    {
                        EnemyEscapedEvent.Publish(gameObject);
                    }
                }
            }
        }

        public void TakeDamage(int damageAmount, int shieldDamageAmount, GameObject projectile)
        {
            shield -= shieldDamageAmount;
            
            if (shield <= 0)
                health -= damageAmount;

            if (health <= 0)
                enemyDeathEvent.Publish(gameObject);
        }
    }

    [Serializable]
    public class EnemyAmount
    {
        public EnemyType enemy;
        public int amount;
    }

    public enum EnemyType
    {
        NORMAL,
        FAST,
        HIGH_HP,
        SHIELDED
    }
}