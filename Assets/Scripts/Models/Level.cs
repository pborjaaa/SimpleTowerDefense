using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    [CreateAssetMenu(fileName = "Level", menuName = "Game/Level", order = 1)]
    public class Level : ScriptableObject
    {
        public int levelNumber;
        public float startingDelay;
        public float timeBetweenWaves;
        public int maxEnemiesEscaped;
        public List<Wave> waves;
        public GameObject levelPrefabStructure;
    }
}