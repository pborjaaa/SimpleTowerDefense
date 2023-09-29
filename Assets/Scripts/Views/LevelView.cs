using System;
using System.Collections;
using Controllers;
using Models;
using TMPro;
using UI.View;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

namespace Views
{
    public class LevelView : NavigableView<LevelController>
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI escapedEnemiesText;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private int towerMenuOffset;
        [SerializeField] private GameObject towerMenu;
        [SerializeField] private GameObject enemysContainer;
        [SerializeField] private GameObject levelStructureContainer;
        [SerializeField] private GameObject towerButtonContainer;
        [SerializeField] private GameObject towerButtonPrefab;
        
        private float timeRemaining;
        private LevelStructurePrefab levelStructurePrefab;
        
        private void Start()
        {
            SetController(new LevelController(new ResourcesLevelLoader()));
            SetupLevel();
            Controller.EnemyEscapedEvent.Subscribe(OnEnemyEscapedEvent);
            Controller.CurrencyChangedEvent.Subscribe(SetupCoinsText);
            Controller.EnemyDeathEvent.Subscribe(OnEnemyDeathEvent);
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (timeRemaining > 0f && !Controller.LastWave)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0f)
                {
                    timeRemaining = 0f;
                }
        
                timerText.text = "Next wave in: " + timeRemaining.ToString("F0");
            }
            else
            {
                timeRemaining = Controller.Level.timeBetweenWaves;
                Controller.AdvanceWave();

                if (!Controller.LastWave)
                    StartCoroutine(SpawnWave(Controller.Level.waves[Controller.CurrentWave]));
                else
                    timerText.text = "LAST WAVE";
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
            enemy.transform.parent = enemysContainer.transform;
            var enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.Setup(Controller.ChooseRandomPath(levelStructurePrefab.paths), Controller.EnemyEscapedEvent, levelStructurePrefab.startingPos, Controller.EnemyDeathEvent);
        }

        private void SetupLevel()
        {
            if (Controller.Level != null)
            {
                SetupLevelPrefab();
                SetupTimer(Controller.Level.startingDelay);
                SetupEnemiesText();
                SetupCoinsText();
                SetupTowerMenu();
            }
            else
            {
                //TODO - pantalla de fin de levels
            }
        }

        private void SetupLevelPrefab()
        {
            levelStructurePrefab = Controller.Level.levelPrefabStructure.GetComponent<LevelStructurePrefab>();
            foreach (var towerButtonPos in levelStructurePrefab.towerPositions)
            {
                SetupTowerButton(towerButtonPos, OnTowerButtonClicked);
            }

            foreach (var towerButtonPos in levelStructurePrefab.topTowerPositions)
            {
                SetupTowerButton(towerButtonPos, OnTopTowerButtonClicked);
            }
            
            Instantiate(levelStructurePrefab.levelStructure, levelStructureContainer.transform);
        }

        private void SetupTowerButton(GameObject towerButtonPos, Action<Button> buttonAction)
        {
            var towerButton = Instantiate(towerButtonPrefab, towerButtonContainer.transform);
            var towerButtonComponent = towerButton.GetComponent<Button>();
            towerButton.transform.position = towerButtonPos.transform.position;
            towerButtonComponent.onClick.AddListener(() => buttonAction(towerButtonComponent));
        }

        private void SetupTowerMenu()
        {
            towerMenu.gameObject.SetActive(false);
        }

        private void SetupTimer(float startingDelay)
        {
            timeRemaining = startingDelay;
        }

        private void SetupEnemiesText()
        {
            escapedEnemiesText.text = "Enemies escaped: " + Controller.EscapedEnemies + "/" + Controller.Level.maxEnemiesEscaped;
        }
        
        private void SetupCoinsText(int amount = 0)
        {
            coinsText.text = "Coins: " + Controller.PlayerState.Coins;
        }
        
        private void OnEnemyEscapedEvent(GameObject enemy)
        {
            Controller.EscapedEnemies++;
            SetupEnemiesText();
            Controller.ValidateGameConditions();
            Controller.ReturnEnemy(enemy);
        }

        public void OnTopTowerButtonClicked(Button button)
        {
            TowerMenu(button.transform.position, true, button.transform);
        }
        
        public void OnTowerButtonClicked(Button button)
        {
            TowerMenu(button.transform.position, false, button.transform);
        }
        
        private void TowerMenu(Vector3 position, bool topButton, Transform button)
        {
            towerMenu.gameObject.SetActive(true);
            towerMenu.GetComponent<TowerMenuView>().Setup(Controller.PlayerState, button, Controller.CurrencyChangedEvent);
            towerMenu.transform.position = topButton ? new Vector3(position.x, position.y - towerMenuOffset, position.z) : new Vector3(position.x, position.y + towerMenuOffset, position.z);
        }
        
        private void OnEnemyDeathEvent(GameObject enemy)
        {
            Controller.OnEnemyDeathEvent(enemy);
            SetupCoinsText();
        }
    }
}