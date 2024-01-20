using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    public abstract class Variable<T> : Variable, IGameEvent<T>
    {
        [SerializeField] private T _value;
        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [SerializeField] private T _startValue;
        public T StartValue
        {
            get { return _startValue; }
        }

        private List<IListener<T>> _listeners = new List<IListener<T>>();
        private HashSet<T> _items = new HashSet<T>();

        //OnEnable is called when the first scene is loaded, regardles if the scene has a reference to it (build game)
        private void OnEnable()
        {
            //This is only for the editor as the build game does not save Scriptable objects across sessions
            if (InitializeType == InitializeType.ResetOnGameStart)
            {
                Value = StartValue;
                Log(_stacktraceVariable, $"Set value to startvalue: {Value}");
            }
                

#if UNITY_EDITOR
            _stacktraceEvent.Clear();
#endif
        }


        //Variable

        public void Set(T value)
        {
            Value = value;
            Log(_stacktraceVariable, $"Set value to {Value}");
        }


        //Event

        public void Raise(T value)
        {
            Value = value;
            Log(_stacktraceEvent, $"Raised and set value to {Value}");

            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(value);
                LogListener(_stacktraceEvent, $"Raised value: {value}", _listeners[i]);
            }
        }

        public void Raise()
        {
            Raise(Value);
        }

        public void RegisterListener(IListener<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
                LogListener(_stacktraceEvent, $"Registered listener", listener);
            }
            else
            {
                LogListener(_stacktraceEvent, $"Tried registering an existing listener", listener);
            }
        }

        public void UnregisterListener(IListener<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
                LogListener(_stacktraceEvent, $"Unregistered listener", listener);
            }
            else
            {
                LogListener(_stacktraceEvent, $"Tried removing a non existing listener", listener);
            }
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
            Log(_stacktraceEvent, "Cleared all listeners");
        }

        public List<IListener> GetListeners() => _listeners.Cast<IListener>().ToList();


        //RuntimeSet

        public void Add(T value)
        {
            _items.Add(value);
            Log(_stacktraceRuntimeSet, $"Added value to runtimeSet: {value}");
        }

        public void Remove(T value)
        {
            _items.Remove(value);
        }


        //Debugging

        private void Log(Stacktrace stacktrace, string message)
        {
#if UNITY_EDITOR
            stacktrace.Add(message);
#endif
        }

        private void LogListener(Stacktrace stacktrace, string message, IListener listener)
        {
#if UNITY_EDITOR
            stacktrace.Add($"{message}" + (listener != null ? $" ({listener.GetName()})" : ""));
#endif
        }

        public Stacktrace GetStackTrace() => _stacktraceEvent;

        private Stacktrace _stacktraceEvent = new Stacktrace();
        private Stacktrace _stacktraceVariable = new Stacktrace();
        private Stacktrace _stacktraceRuntimeSet = new Stacktrace();
        public T DebugValue;
    }

    public abstract class Variable : ScriptableObject 
    {
        public VariableType VariableType;
        public InitializeType InitializeType;
    }
}