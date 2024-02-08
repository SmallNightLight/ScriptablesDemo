using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class EnemyData : IDataPoint
    {
        public float Heath;
        public float Damage;
        public float Speed;
        public int Coins;
        public Sprite Sprite;
    }
}