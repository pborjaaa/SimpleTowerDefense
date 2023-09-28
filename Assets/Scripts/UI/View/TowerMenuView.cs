using System.Collections.Generic;
using Events;
using Models;
using UI.Controller;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace UI.View
{
    public class TowerMenuView : NavigableView<TowerMenuController>
    {
        [SerializeField] private GameObject normalTowerPrefab;
        [SerializeField] private GameObject highDamageTowerPrefab;
        [SerializeField] private GameObject longRangeTowerPrefab;
        [SerializeField] private GameObject antiShieldTowerPrefab;
        [SerializeField] private GameObject towersContainer;
        [SerializeField] private Button closeButton;
            
        private Dictionary<TowerType, GameObject> towerPrefabs = new();
        private Transform currentButton;
        private CurrencyChangedEvent currencyChangedEvent;
        
        private void Start()
        {
            SetController(new TowerMenuController());
            
            towerPrefabs.Add(TowerType.NORMAL, normalTowerPrefab);
            towerPrefabs.Add(TowerType.HIGH_DAMAGE, highDamageTowerPrefab);
            towerPrefabs.Add(TowerType.LONG_RANGE, longRangeTowerPrefab);
            towerPrefabs.Add(TowerType.ANTI_SHIELD, antiShieldTowerPrefab);
            
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public void OnNormalBuyButtonClicked()
        {
            TryBuyTower(TowerType.NORMAL);
        }
        
        public void OnHighDamageBuyButtonClicked()
        {
            TryBuyTower(TowerType.HIGH_DAMAGE);
        }
        
        public void OnLongRangeBuyButtonClicked()
        {
            TryBuyTower(TowerType.LONG_RANGE);
        }
        
        public void OnAntiShieldBuyButtonClicked()
        {
            TryBuyTower(TowerType.ANTI_SHIELD);
        }

        private void TryBuyTower(TowerType towerType)
        {
            if (Controller.CanBuyTower(towerType))
            {
                if (towerPrefabs.TryGetValue(towerType, out var prefab))
                {
                    var tower = Instantiate(prefab, towersContainer.transform);
                    tower.transform.position = currentButton.position;
                    currencyChangedEvent.Publish(-Controller.GetTowerCost(towerType));
                    currentButton.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
            }
        }

        public void Setup(PlayerState playerState, Transform button, CurrencyChangedEvent currencyChangedEvent)
        {
            this.currencyChangedEvent = currencyChangedEvent;
            Controller.PlayerState = playerState;
            currentButton = button;
        }
    }
}