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
        public Dictionary<Vector3Int, TowerSingle> Towers = new Dictionary<Vector3Int, TowerSingle>();
    }
}