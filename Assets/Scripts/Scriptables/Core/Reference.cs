using System;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    [System.Serializable]
    public abstract class Reference<T, TVariable> where TVariable : Variable<T>
    {
        [SerializeField] protected bool _isVariable;
        [SerializeField] protected TVariable _variable;
        [SerializeField] protected T _constant;

        public T Value
        {
            get
            {
                if (_isVariable && _variable != null)
                    return _variable.Value;
                else
                    return _constant;
            }
            set
            {
                if (_variable.Type == InitializeType.ReadOnly)
                {
                    Debug.LogWarning($"Cannot set: {_variable.name} (Readonly)");
                    return;
                }

                if (_isVariable && _variable != null)
                    _variable.Value = value;
                else
                {
                    _constant = value;
                    _isVariable = false;
                }
            }
        }
    }
}