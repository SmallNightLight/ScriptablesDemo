using ScriptableArchitecture.Core;
using System.Collections.Generic;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public struct TowerCollection : IDataPoint
    {
        public string BaseName;
        public List<TowerDataReference> Towers;
    }
}