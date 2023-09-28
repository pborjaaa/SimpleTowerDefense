using System;
using Controllers;
using Models;
using Utilities;

namespace UI.Controller
{
    public class TowerMenuController : ViewController
    {
        public PlayerState PlayerState;

        public bool CanBuyTower(TowerType towerType)
        {
            return PlayerState.Coins >= GetTowerCost(towerType);
        }

        public int GetTowerCost(TowerType towerType)
        {
            return towerType switch
            {
                TowerType.NORMAL => GameConfig.NormalTowerCost,
                TowerType.LONG_RANGE => GameConfig.LongRangeTowerCost,
                TowerType.HIGH_DAMAGE => GameConfig.HighDamageTowerCost,
                TowerType.ANTI_SHIELD => GameConfig.AntiShieldTowerCost,
                _ => throw new ArgumentOutOfRangeException(nameof(towerType), towerType, null)
            };
        }
    }
}