using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/TowerSingle Event Listener")]
    public class TowerSingleGameEventListener : GameEventListenerBase<TowerSingle>
    {
        [SerializeField] private TowerSingleVariable _event;

        public override IGameEvent<TowerSingle> GetGameEventT() => _event;
    }
}