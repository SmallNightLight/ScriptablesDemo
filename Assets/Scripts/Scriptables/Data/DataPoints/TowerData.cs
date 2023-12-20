using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public struct TowerData : IDataPoint
    {
        public string Name;
        public string Description;
        public Sprite Sprite;
    }
}