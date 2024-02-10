using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class TowerData : IDataPoint
    {
        public string Name;
        public string Description;
        public TowerSingle StartTower;
        public List<TowerSingle> UpgradeTowers;

        public bool TryGetTower(int level, out TowerSingle tower)
        {
            if (level < 0 || level > UpgradeTowers.Count)
            {
                tower = null;
                return false;
            }

            if (level == 0)
            {
                tower = StartTower;
                return true;
            }

            tower = UpgradeTowers[level - 1];
            return true;
        }
    }

    [System.Serializable]
    public class TowerSingle
    {
        public string Name;
        public Sprite Sprite;
        public int Cost;
        public float Range;
        public float Interval;
        public float ProjectileSpeed;
        public List<Effect> Effects;
    }

    [System.Serializable]
    public class TowerCollection
    {
        //Dictionary for all tower in the scene and their current level
        public Dictionary<Vector3Int, (TowerData, int)> Towers = new Dictionary<Vector3Int, (TowerData, int)>();

        /// <summary>
        /// Check wheter the tower at the given cell position can be further upgraded
        /// </summary>
        /// <param name="cellPosition"></param>
        /// <returns></returns>
        public bool CanBeUpgraded(Vector3Int cellPosition)
        {
            return TryGetTower(cellPosition, out TowerSingle tower, 1);
        }


        /// <summary>
        /// Check wheter the tower at the given cell position can be upgraded with the current amount of coins
        /// </summary>
        public bool CanBeBoughtNext(Vector3Int cellPosition, int currentCoins)
        {
            if (TryGetTower(cellPosition, out TowerSingle tower, 1))
            {
                return currentCoins >= tower.Cost;
            }

            return false;
        }

        public bool TryGetTower(Vector3Int cellPosition, out TowerSingle tower, int levelOffset = 0)
        {
            if (Towers.TryGetValue(cellPosition, out (TowerData, int) towerBehaviour))
            {
                TowerData towerData = towerBehaviour.Item1;
                int level = towerBehaviour.Item2;
                return towerData.TryGetTower(level + levelOffset, out tower);
            }

            tower = null;
            return false;
        }
    }
}