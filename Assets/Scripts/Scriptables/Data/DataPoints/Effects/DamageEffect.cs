using UnityEngine;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// This is a dameg effect, that only deals damege to the target on impact
    /// </summary>
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "Effects/DamageEffect")]
    public class DamageEffect : Effect
    {
        public float Damage;

        /// <summary>
        /// Deals damage to the target on impact
        /// </summary>
        protected override void OnEnemyHit()
        {
            EnemyData.Health -= Damage;
        }
    }
}