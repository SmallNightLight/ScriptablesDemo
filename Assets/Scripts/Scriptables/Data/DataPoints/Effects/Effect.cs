using UnityEngine;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// Abstract class for an effect that can be applied to enemies, override this class for nay effect and modify the provided enemyData
    /// </summary>
    public abstract class Effect : ScriptableObject
    {
        [HideInInspector] public EnemyData EnemyData;

        [Tooltip("Duration in seconds to apply the effect - use 0 for instant effect")] public float Duration;

        private float _timer;
        private bool _effectStarted;
        private bool _effectEnded;

        public static bool INSTANTKILL;

        /// <summary>
        /// Start the effect
        /// </summary>
        private void ApplyEffect()
        {
            if (EnemyData == null)
            {
                Debug.LogWarning("No EnemyData - no effect applied!");
                return;
            }

            if (INSTANTKILL)
            {
                EnemyData.Health = 0;
                return;
            }

            OnEnemyHit();

            _timer = 0f;
        }

        /// <summary>
        /// Updates the effect and calls the start and end functions at the correct time
        /// </summary>
        public void UpdateEffect(float deltaTime)
        {
            if (_effectEnded) return;

            if (EnemyData == null)
            {
                Debug.LogWarning("No EnemyData - no effect applied!");
                return;
            }

            if (!_effectStarted)
            {
                ApplyEffect();
                _effectStarted = true;
                return;
            }

            _timer += deltaTime;

            if (_timer >= Duration)
            {
                OnEffectEnd();
                _effectEnded = true;
                return;
            }

            OnUpdate(deltaTime);
        }

        /// <summary>
        /// Clones the effec
        /// </summary>
        public Effect Clone()
        {
            return MemberwiseClone() as Effect;
        }

        /// <summary>
        /// Checks whether the effect has aldready ended
        /// </summary>
        /// <returns></returns>
        public bool IsEnded() => _effectEnded;

        /// <summary>
        /// Override this function for the effect start
        /// </summary>
        protected virtual void OnEnemyHit() { }

        /// <summary>
        /// Override this function for the effect update
        /// </summary>
        protected virtual void OnUpdate(float deltaTime) { }

        /// <summary>
        /// Override this function for the effect end
        /// </summary>
        protected virtual void OnEffectEnd() { }
    }
}