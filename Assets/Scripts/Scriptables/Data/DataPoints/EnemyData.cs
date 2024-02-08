using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class EnemyData : IDataPoint
    {
        public float Health;
        public float Damage;
        public float Speed;
        public int Coins;
        public Sprite Sprite;

        public EnemyData Copy()
        {
            EnemyData copy = new EnemyData();
            copy.Health = Health;
            copy.Damage = Damage;
            copy.Speed = Speed;
            copy.Coins = Coins;
            copy.Sprite = Sprite;
            return copy;
        }
    }
}