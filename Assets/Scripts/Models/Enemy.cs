using System;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Models
{
    public abstract class Enemy : MonoBehaviour
    {
        public int health;
        public float speed;
        public int shield;
        public EnemyType enemyType;
        
        private int currentWaypointIndex = 0;
        private List<Transform> waypoints = new();
        private bool movingToWaypoints;
        private EnemyEscapedEvent enemyEscapedEvent;

        protected Enemy(float speed, int health, int shield, EnemyType enemyType)
        {
            this.speed = speed;
            this.health = health;
            this.shield = shield;
            this.enemyType = enemyType;
        }

        private void Update()
        {
            if (movingToWaypoints)
                MoveToWaypoint();
        }

        public void Setup(GameObject[] path, EnemyEscapedEvent enemyEscapedEvent, Transform startingPos)
        {
            this.enemyEscapedEvent = enemyEscapedEvent;
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
                        enemyEscapedEvent.Publish(gameObject);
                    }
                }
            }
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