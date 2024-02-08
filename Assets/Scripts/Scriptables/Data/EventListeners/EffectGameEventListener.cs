using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/Effect Event Listener")]
    public class EffectGameEventListener : GameEventListenerBase<Effect>
    {
        [SerializeField] private EffectVariable _event;

        public override IGameEvent<Effect> GetGameEventT() => _event;
    }
}