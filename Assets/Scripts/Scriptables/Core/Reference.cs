using System;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    [Serializable]
    public class Reference<T> : BaseReference
    {
        [SerializeField] protected bool _isVariable;
        [SerializeField] protected Variable<T> _variable;
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
                if (_isVariable && _variable != null)
                    _variable.Value = value;
                else
                {
                    _constant = value;
                    _isVariable = false;
                }
            }
        }

#if UNITY_EDITOR
        public Reference()
        {
            _valueType = typeof(T).AssemblyQualifiedName;
        }
#endif
    }

    [Serializable]
    public abstract class BaseReference 
    {
#if UNITY_EDITOR
        [SerializeField] protected string _valueType;
#endif
    }
}