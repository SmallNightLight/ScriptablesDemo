using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class EnemyDataReference : Reference<EnemyData, EnemyDataVariable>
    {
        public EnemyDataReference Copy()
        {
            EnemyDataReference clone = new EnemyDataReference();
            clone._isVariable = _isVariable;

            if (_variable != null)
            {
                clone._variable = ScriptableObject.CreateInstance<EnemyDataVariable>();
                clone._variable.Value = _variable.Value;
            }
            
            clone._constant = _constant;

            return clone;
        }
    }
}