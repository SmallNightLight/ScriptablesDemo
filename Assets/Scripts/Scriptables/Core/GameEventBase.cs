using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    public class GameEventBase : ScriptableObject, IGameEvent
    {
        [SerializeField] private List<GameEventListenerBase> _listeners = new List<GameEventListenerBase>();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised();
                Log($"Raised {_listeners[i].name}");
            }
        }

        public void RegisterListener(GameEventListenerBase listener)
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

        public void UnregisterListener(GameEventListenerBase listener)
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


        //Stack trace

        private void OnEnable()
        {
#if UNITY_EDITOR
            _stacktrace.Clear();
#endif
        }

        private void Log(string message)
        {
#if UNITY_EDITOR
            _stacktrace.Add(message);
#endif
        }

        public Stacktrace GetStackTrace() => _stacktrace;

#if UNITY_EDITOR
        private Stacktrace _stacktrace = new Stacktrace();
#endif
    }

    public class GameEventBase<T> : ScriptableObject, IGameEvent
    {
        private List<GameEventListenerBase<T>> _listeners = new List<GameEventListenerBase<T>>();

        public T DebugValue;

        public void Raise(T value)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(value);
                Log($"Raised value: {value} on {_listeners[i].name}");
            }
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


        //Stack trace

        private void OnEnable()
        {
#if UNITY_EDITOR
            _stacktrace.Clear();
#endif
        }

        private void Log(string message)
        {
#if UNITY_EDITOR
            _stacktrace.Add(message);
#endif
        }

        public Stacktrace GetStackTrace() => _stacktrace;

#if UNITY_EDITOR
        private Stacktrace _stacktrace = new Stacktrace();
#endif
    }

    public interface IGameEvent
    {
        public List<IListener> GetListeners();

        public Stacktrace GetStackTrace();
    }
}