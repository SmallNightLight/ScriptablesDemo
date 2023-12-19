using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    [CreateAssetMenu(fileName = "Receiver", menuName = "Scriptables/Receiver")]
    public class Receiver : ScriptableObject
    {
        private List<Emitter> _emitters = new List<Emitter>();
        private Emitter _activeEmitter;

        public T Value<T>() where T : Object
        {
            if (_activeEmitter == null)
            {
                Debug.LogError($"No active emitter in {name}, make sure to first add an emitter");
                Debug.Break();
                return null;
            }

            return _activeEmitter.Value<T>();
        }
        public void RegisterEmitter(Emitter emitter)
        {
            if (_emitters.Contains(emitter)) return;

            _emitters.Add(emitter);
            UpdatePriorityList(emitter);
        }

        public void UnregisterEmitter(Emitter emitter)
        {
            if (!_emitters.Contains(emitter)) return;

            _emitters.Remove(emitter);

            if (_activeEmitter == emitter)
                UpdatePriorityList();
        }

        public void RemoveAllEmitters()
        {
            _emitters.Clear();
        }

        public void UpdatePriorityList(Emitter changedEmitter)
        {
            if (_activeEmitter == null || changedEmitter == _activeEmitter || changedEmitter.Priority > _activeEmitter.Priority)
                UpdatePriorityList();
        }

        public void UpdatePriorityList()
        {
            if (_emitters == null || _emitters.Count == 0) 
            {
                _activeEmitter = null;
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

            _activeEmitter = priorityEmitter;
        }
    }
}