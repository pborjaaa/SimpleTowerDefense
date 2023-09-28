using System;
using Events;
using Models;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class LevelController : ViewController
    {
        public Level Level;
        public int CurrentWave;
        public bool LastWave;
        public EnemyEscapedEvent EnemyEscapedEvent;
        public EnemyDeathEvent EnemyDeathEvent;
        public CurrencyChangedEvent CurrencyChangedEvent;
        public int EscapedEnemies;
        public PlayerState PlayerState;
        
        private EnemyPool enemyPool;
        
        public LevelController(ILevelLoader levelLoader)
        {
            EnemyEscapedEvent = new EnemyEscapedEvent();
            EnemyDeathEvent = new EnemyDeathEvent();
            CurrencyChangedEvent = new CurrencyChangedEvent();
            CurrencyChangedEvent.Subscribe(OnCurrencyChangedEvent);
            PlayerState = new PlayerState(GameConfig.InitialCoins, CurrencyChangedEvent);
            enemyPool = new EnemyPool();
            
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
            Level = levelLoader.Load("Level " + currentLevel);
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
        
        public void OnEnemyDeathEvent(GameObject enemy)
        {
            PlayerState.Coins += GetEnemyCoins(enemy.GetComponent<Enemy>().enemyType);
            ReturnEnemy(enemy);
        }
        
        private void OnCurrencyChangedEvent(int amount)
        {
            PlayerState.Coins += amount;
        }

        private int GetEnemyCoins(EnemyType type)
        {
            return type switch
            {
                EnemyType.NORMAL => GameConfig.NormalEnemyCoins,
                EnemyType.FAST => GameConfig.FastEnemyCoins,
                EnemyType.HIGH_HP => GameConfig.HighHpEnemyCoins,
                EnemyType.SHIELDED => GameConfig.ShieldedEnemyCoins,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}