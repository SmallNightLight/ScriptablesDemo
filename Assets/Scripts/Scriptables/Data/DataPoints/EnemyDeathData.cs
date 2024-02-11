using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// Small serialized class for Enemy Death data and the last position
    /// </summary>
    [System.Serializable]
    public class EnemyDeathData : IDataPoint
    {
        public EnemyData EnemyData;
        public Vector2 Position;
    }
}