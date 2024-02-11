using ScriptableArchitecture.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// The data for an enemy unit. Can be used as a base but copy it for each individual enemy as its values are modified from the effects.
    /// Includes health, speed, sprite and coins. Also provides functions for managing the effects
    /// </summary>
    [System.Serializable]
    public class EnemyData : IDataPoint
    {
        public float Health;
        public float Speed;
        public int Coins;
        public Sprite Sprite;
        public Vector3 Position;

        private List<Effect> _activeEffects = new List<Effect>();
        [HideInInspector] public bool IsDead;

        /// <summary>
        /// Gets a copy of the current enemy data
        /// </summary>
        public EnemyData Copy()
        {
            EnemyData copy = new EnemyData();
            copy.Health = Health;
            copy.Speed = Speed;
            copy.Coins = Coins;
            copy.Sprite = Sprite;
            copy.Position = Position;
            return copy;
        }

        /// <summary>
        /// Adds an effect that will be updated here
        /// </summary>
        public void AddEffect(Effect newEffect)
        {
            if (newEffect == null)
            {
                Debug.LogWarning("Cannot add effect - newEffect is null");
                return;
            }

            if (!CanAddEffect(newEffect)) return;

            newEffect.EnemyData = this;
            _activeEffects.Add(newEffect);
        }

        /// <summary>
        /// Checks wheter the effect type already is currently added - prevent effects of same type from stacking
        /// </summary>
        private bool CanAddEffect(Effect effect)
        {
            return !_activeEffects.Exists(item => item.GetType() == effect.GetType());
        }

        /// <summary>
        /// Updates all active effects and removes them when they have ended
        /// </summary>
        public void UpdateEffects()
        {
            if (_activeEffects == null) return;

            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                if (_activeEffects[i] == null || _activeEffects[i].IsEnded())
                {
                    _activeEffects.RemoveAt(i);
                    continue;
                }

                _activeEffects[i].UpdateEffect(Time.deltaTime);
            }
        }
    }
}