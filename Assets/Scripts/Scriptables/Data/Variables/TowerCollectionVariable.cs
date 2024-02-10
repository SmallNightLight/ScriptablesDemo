using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "TowerCollectionVariable", menuName = "Scriptables/Variables/TowerCollection")]
    public class TowerCollectionVariable : Variable<TowerCollection>
    {
        public void ClearTowers()
        {
            Value.Towers.Clear();
        }
    }
}