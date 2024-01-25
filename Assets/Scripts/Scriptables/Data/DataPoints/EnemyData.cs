using ScriptableArchitecture.Core;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class EnemyData : IDataPoint
    {
        public int Heath;
        public int Damage;
    }
}