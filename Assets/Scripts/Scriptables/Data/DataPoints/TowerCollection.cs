using ScriptableArchitecture.Core;
using System.Collections.Generic;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class TowerCollection : IDataPoint
    {
        public string BaseName;
        public List<TowerDataReference> Towers;
    }
}