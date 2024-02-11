using UnityEngine;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// This effect slows the target on imact
    /// </summary>
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "Effects/SlowEffect")]
    public class SlowEffect : Effect
    {
        [Tooltip("1 is default speed, 0 is no movement")] public float SlowDownFactor;

        private float _startSpeed;

        /// <summary>
        /// Applies the effect on impact
        /// </summary>
        protected override void OnEnemyHit()
        {
            _startSpeed = EnemyData.Speed;
            EnemyData.Speed *= SlowDownFactor;
        }

        /// <summary>
        /// Resets the enemy speed when the effect has ended
        /// </summary>
        protected override void OnEffectEnd()
        {
            EnemyData.Speed = _startSpeed;
        }
    }
}