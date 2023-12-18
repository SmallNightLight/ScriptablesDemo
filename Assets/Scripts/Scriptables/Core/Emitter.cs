using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ScriptableArchitecture.Core
{
    public class Emitter : MonoBehaviour
    {
        [SerializeField] private Receiver _receiver;
        [SerializeField] private ReceiveType _receiveType = ReceiveType.AlwaysActive;

        [Tooltip("Higher number for more important emmiters")]
        [SerializeField] private int _priority = 0;

        [SerializeField] private Object _value;

        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                _receiver?.UpdatePriorityList(this);
            }
        }

        private bool _isAdded;

        public T GetValue<T>() where T : Object
        {
            if (_value is T)
                return _value as T;

            Debug.LogError($"Could not convert from \"{_value.GetType()} to {typeof(T)} ");
            return null;
        }

        public void SetValue<T>(T value)
        {
            _value = value as Object;
        }

        public void SetValue(Object value)
        {
            _value = value;
        }

        private void OnEnable()
        {
            if (_receiveType.HasFlag(ReceiveType.AlwaysActive))
                Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        public void Register()
        {
            _isAdded = true;
            _receiver.RegisterEmmiter(this);
        }

        public void Unregister()
        {
            if (_isAdded)
                _receiver?.UnregisterEmmiter(this);

            _isAdded = false;
        }

        [Flags]
        private enum ReceiveType
        {
            None = 0,
            AlwaysActive = 1 << 0,
        }
    }
}