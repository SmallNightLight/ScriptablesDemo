using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    public abstract class Variable<T> : Variable, IGameEvent<T>
    {
        public T Value;
        public T StartValue;

        private List<IListener<T>> _listeners = new List<IListener<T>>();

        //OnEnable is called when the first scene is loaded, regardles if the scene has a reference to it (build game)
        private void OnEnable()
        {
            //This is only for the editor as the build game does not save Scriptable objects across sessions
            if (InitializeType == InitializeType.ResetOnGameStart)
                Value = StartValue;

#if UNITY_EDITOR
            _stacktrace.Clear();
#endif
        }

        public void Set(T value)
        {
            Value = value;
            Log($"Set Value to {Value}", null);
        }

        public void Raise(T value)
        {
            Value = value;
            Log($"Raised and set Value to {Value}", null);

            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(value);
                Log($"Raised value: {value}", _listeners[i]);
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
                Log($"Registered listener", listener);
            }
            else
            {
                Log($"Tried registering an existing listener", listener);
            }
        }

        public void UnregisterListener(IListener<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
                Log($"Unregistered listener", listener);
            }
            else
            {
                Log($"Tried removing a non existing listener", listener);
            }
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
            Log("Cleared all listeners", null);
        }

        public List<IListener> GetListeners() => _listeners.Cast<IListener>().ToList();


        //Debugging

        private void Log(string message, IListener listener)
        {
#if UNITY_EDITOR
            _stacktrace.Add($"{message}" + (listener != null ? $" ({listener.GetName()})" : ""));
#endif
        }

        public Stacktrace GetStackTrace() => _stacktrace;

        private Stacktrace _stacktrace = new Stacktrace();
        public T DebugValue;
    }

    public abstract class Variable : ScriptableObject 
    {
        public VariableType VariableType;
        public InitializeType InitializeType;
    }
}