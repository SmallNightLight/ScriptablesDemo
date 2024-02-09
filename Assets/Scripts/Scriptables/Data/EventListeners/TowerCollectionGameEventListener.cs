using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/TowerCollection Event Listener")]
    public class TowerCollectionGameEventListener : GameEventListenerBase<TowerCollection>
    {
        [SerializeField] private TowerCollectionVariable _event;

        public override IGameEvent<TowerCollection> GetGameEventT() => _event;
    }
}