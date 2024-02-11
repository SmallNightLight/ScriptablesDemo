using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// The data for a a complete tower unit. Has start tower and a list of towers it can be upgraded to
    /// </summary>
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

    /// <summary>
    /// The data for the actual tower
    /// </summary>
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

    /// <summary>
    /// The data for a tower collection that manages multiple towers and their location.
    /// Has function to check for upgrades and whether they can be bought
    /// </summary>
    [System.Serializable]
    public class TowerCollection
    {
        //Dictionary for all tower in the scene and their current level
        public Dictionary<Vector3Int, (TowerData, int)> Towers = new Dictionary<Vector3Int, (TowerData, int)>();

        /// <summary>
        /// Check wheter the tower at the given cell position can be further upgraded
        /// </summary>
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

        /// <summary>
        /// Tries to get the tower at given cell position. The offset can be used to get towers at different levels 
        /// (1 for the next upgrade). Returns false if no tower could be found
        /// </summary>
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