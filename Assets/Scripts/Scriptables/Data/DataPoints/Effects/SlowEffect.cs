using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "Effects/SlowEffect")]
    public class SlowEffect : Effect
    {
        [Tooltip("1 is default speed, 0 is no movement")] public float SlowDownFactor;

        private float _startSpeed;

        protected override void OnEnemyHit()
        {
            _startSpeed = EnemyData.Speed;
            EnemyData.Speed *= SlowDownFactor;
        }

        protected override void OnEffectEnd()
        {
            EnemyData.Speed = _startSpeed;
        }
    }
}