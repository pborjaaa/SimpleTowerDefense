using System.Collections;
using Controllers;
using Models;
using TMPro;
using UnityEngine;
using Utilities;

namespace Views
{
    public class LevelView : NavigableView<LevelController>
    {
        [SerializeField] private GameObject levelContainer;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject startPoint;
        [SerializeField] private GameObject[] endPoints;
        [SerializeField] private GameObject[] paths;
        [SerializeField] private TextMeshProUGUI escapedEnemiesText;

        private float timeRemaining;
        
        private void Start()
        {
            SetController(new LevelController(new ResourcesLevelLoader()));
            SetupLevel();
            Controller.EnemyEscapedEvent.Subscribe(OnEnemyEscapedEvent);
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0f)
                {
                    timeRemaining = 0f;
                }
        
                timerText.text = timeRemaining.ToString("F0");
            }
            else
            {
                timeRemaining = Controller.Level.timeBetweenWaves;
                Controller.AdvanceWave();
                
                if(!Controller.LastWave)
                    StartCoroutine(SpawnWave(Controller.Level.waves[Controller.CurrentWave]));
            }
        }

        private IEnumerator SpawnWave(Wave wave)
        {
            foreach (var enemyAmount in wave.enemyAmount)
            {
                for (var i = 0; i < enemyAmount.amount; i++)
                {
                    SpawnEnemy(enemyAmount.enemy);
                    yield return new WaitForSeconds(wave.delayBetweenEnemies);
                }
            }
        }
        
        private void SpawnEnemy(EnemyType enemyType)
        {
            var enemy = Controller.GetEnemy(enemyType);
            var enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.Setup(Controller.ChooseRandomPath(paths), Controller.EnemyEscapedEvent, startPoint.transform);
        }

        private void SetupLevel()
        {
            if (Controller.Level != null)
            {
                SetupTimer(Controller.Level.startingDelay);
                SetupText();
            }
            else
            {
                //TODO - pantalla de fin de levels
            }
        }

        private void SetupTimer(float startingDelay)
        {
            timeRemaining = startingDelay;
        }

        private void SetupText()
        {
            escapedEnemiesText.text = "Enemies escaped: " + Controller.EscapedEnemies + "/" + Controller.Level.maxEnemiesEscaped;
        }
        
        private void OnEnemyEscapedEvent(GameObject enemy)
        {
            Controller.EscapedEnemies++;
            SetupText();
            Controller.ValidateGameConditions();
            Controller.ReturnEnemy(enemy);
        }
    }
}