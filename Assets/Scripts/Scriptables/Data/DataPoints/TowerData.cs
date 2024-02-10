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

        public TowerSingle GetTower(int level)
        {
            if (level == 0)
                return StartTower;

            if (level < 0 || level > UpgradeTowers.Count)
            {
                Debug.LogError("Tower level is out of range");
                return null;
            }
            
            return UpgradeTowers[level - 1];
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
        public Dictionary<Vector3Int, (TowerData, int)> TowerBehaviour = new Dictionary<Vector3Int, (TowerData, int)>();
    }
}