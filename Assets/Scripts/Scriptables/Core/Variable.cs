using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    public abstract class Variable<T> : Variable, IGameEvent
    {
        [SerializeField]
        public T Value;
        public InitializeType Type;
        public T StartValue;

        [SerializeField] private VariableType _variableType;
        private List<GameEventListenerBase<T>> _listeners = new List<GameEventListenerBase<T>>();

        //OnEnable is called when the first scene is loaded, regardles if the scene has a reference to it (build game)
        private void OnEnable()
        {
            //This is only for the editor as the build game does not save Scriptable objects across sessions
            if (Type == InitializeType.ResetOnGameStart)
                Value = StartValue;

#if UNITY_EDITOR
            _stacktrace.Clear();
#endif
        }

        public void Set(T value)
        {
            Value = value;
            Log($"Set Value to {Value}");
        }

        public void Raise(T value)
        {
            Value = value;
            Log($"Raised and set Value to {Value}");

            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(value);
                Log($"Raised value: {value} on {_listeners[i].name}");
            }
        }

        public void Raise()
        {
            Raise(Value);
        }

        public void RegisterListener(GameEventListenerBase<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
                Log($"Registered listener: {listener.name}");
            }
            else
            {
                Log($"Tried registering an existing listener: {listener.name}");
            }
        }

        public void UnregisterListener(GameEventListenerBase<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
                Log($"Unregistered listener: {listener.name}");
            }
            else
            {
                Log($"Tried removing a non existing listener: {listener.name}");
            }
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
            Log("Cleared all listeners");
        }

        public List<IListener> GetListeners() => _listeners.Cast<IListener>().ToList();


        //Debugging

        private void Log(string message)
        {
#if UNITY_EDITOR
            _stacktrace.Add(message);
#endif
        }

        public Stacktrace GetStackTrace() => _stacktrace;

        private Stacktrace _stacktrace = new Stacktrace();
        public T DebugValue;
    }

    public abstract class Variable : ScriptableObject { }

    public enum InitializeType
    {
        Normal, ResetOnGameStart, ReadOnly
    }

    public enum VariableType
    {
        Variable, VariableEvent, Event
    }
}