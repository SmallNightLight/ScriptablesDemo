using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    [CreateAssetMenu(fileName = "Receiver", menuName = "Scriptables/Receiver")]
    public class Receiver : ScriptableObject
    {
        private List<Emitter> _emitters = new List<Emitter>();
        private Emitter _currentEmitter;

        public T Get<T>() where T : Object => _currentEmitter.GetValue<T>();

        public void RegisterEmmiter(Emitter emitter)
        {
            if (_emitters.Contains(emitter)) return;

            _emitters.Add(emitter);
            UpdatePriorityList(emitter);
        }

        public void UnregisterEmmiter(Emitter emitter)
        {
            if (!_emitters.Contains(emitter)) return;

            _emitters.Remove(emitter);

            if (_currentEmitter == emitter)
                UpdatePriorityList();
        }

        public void RemoveAllEmmiters()
        {
            _emitters.Clear();
        }

        public void UpdatePriorityList(Emitter changedEmitter)
        {
            if (_currentEmitter == null || changedEmitter.Priority > _currentEmitter.Priority)
                UpdatePriorityList();
        }

        public void UpdatePriorityList()
        {
            if (_emitters == null || _emitters.Count == 0) 
            {
                _currentEmitter = null;
                return;
            }
            
            Emitter priorityEmitter = _emitters[0];
            int priority = priorityEmitter.Priority;

            for(int i = 1; i < _emitters.Count; i++)
            {
                if (_emitters[i].Priority > priority)
                {
                    priorityEmitter = _emitters[i];
                    priority = priorityEmitter.Priority;
                }
            }

            _currentEmitter = priorityEmitter;
        }
    }
}