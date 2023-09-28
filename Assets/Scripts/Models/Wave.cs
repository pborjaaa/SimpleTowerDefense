using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    [CreateAssetMenu(fileName = "Wave", menuName = "Game/Wave", order = 2)]
    public class Wave : ScriptableObject
    {
        public EnemyAmount[] enemyAmount;
        public float delayBetweenEnemies; 
    }
}