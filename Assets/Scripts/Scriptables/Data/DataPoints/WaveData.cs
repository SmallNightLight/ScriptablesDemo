using ScriptableArchitecture.Core;
using System.Collections.Generic;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// The data for a wave of enemies
    /// </summary>
    [System.Serializable]
    public class WaveData : IDataPoint
    {
        /// <summary>
        /// The data for a sub wave of enemies, their spawn rate and enemy type, the time indicates the time until the next subwave spawns
        /// </summary>
        [System.Serializable]
        public class EnemyWave
        {
            public EnemyDataReference Enemy;
            public int Count;
            public float Time;
        }

        public float SpawnSpeed;
        public List<EnemyWave> EnemyData;
    }
}