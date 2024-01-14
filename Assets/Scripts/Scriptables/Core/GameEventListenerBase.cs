using UnityEngine;
using UnityEngine.Events;

namespace ScriptableArchitecture.Core
{
    public abstract class GameEventListenerBase : MonoBehaviour, IListener
    {
        public GameEventBase Event;
        [SerializeField] private UnityEvent _response;
        
        public void OnEventRaised()
        {
            _response.Invoke();
        }

        private void OnEnable()
        {
            if (Event == null)
            {
                Debug.LogWarning($"No Event selected on {name}");
                return;
            }
                
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (Event == null)
                return;

            Event.UnregisterListener(this);
        }

        public Object GetListenerObject() => this;

        public IGameEvent GetGameEvent() => Event;
    }

    public abstract class GameEventListenerBase<T> : MonoBehaviour, IListener
    {
        public GameEventBase<T> Event;
        [SerializeField] private UnityEvent<T> _response;

        public void OnEventRaised(T value)
        {
            _response.Invoke(value);
        }

        private void OnEnable()
        {
            if (Event == null)
            {
                Debug.LogWarning($"No Event selected on {name}");
                return;
            }

            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (Event == null)
                return;

            Event.UnregisterListener(this);
        }

        public Object GetListenerObject() => this;

        public IGameEvent GetGameEvent() => Event;
    }

    public interface IListener
    {
        public Object GetListenerObject();

        public IGameEvent GetGameEvent();
    }
}