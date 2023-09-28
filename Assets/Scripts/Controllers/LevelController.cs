using Events;
using Models;
using UnityEngine;
using Utilities;

namespace Controllers
{
    public class LevelController : ViewController
    {
        public Level Level;
        public int CurrentWave;
        public bool LastWave;
        public EnemyEscapedEvent EnemyEscapedEvent;
        public int EscapedEnemies;

        private EnemyPool enemyPool;
        
        public LevelController(ILevelLoader levelLoader)
        {
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            Level = levelLoader.Load("Level " + currentLevel);
            EnemyEscapedEvent = new EnemyEscapedEvent();
            enemyPool = new EnemyPool();
            CurrentWave = 0;
        }

        public void AdvanceWave()
        {
            if (CurrentWave < Level.waves.Count - 1)
                CurrentWave++;
            else
                LastWave = true;
        }

        public GameObject GetEnemy(EnemyType enemyType)
        {
            return enemyPool.GetEnemy(enemyType);
        }
        
        public void ReturnEnemy(GameObject enemy)
        {
            enemyPool.ReturnEnemy(enemy);
        }

        public GameObject[] ChooseRandomPath(GameObject[] paths)
        {
            var randomPath = paths[Random.Range(0, paths.Length)];
            
            var childTransforms = randomPath.GetComponentsInChildren<Transform>();
            var childObjects = new GameObject[childTransforms.Length - 1];
        
            for (var i = 0; i < childTransforms.Length; i++)
            {
                if (childTransforms[i].gameObject != randomPath)
                {
                    childObjects[i - 1] = childTransforms[i].gameObject;
                }
            }
            
            return childObjects;
        }

        public void ValidateGameConditions()
        {
            if (EscapedEnemies >= Level.maxEnemiesEscaped)
            {
                //TODO - LOSE CONDITION
            }
        }
    }
}