using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "Effects/DamageEffect")]
    public class DamageEffect : Effect
    {
        public float Damage;

        protected override void OnEnemyHit()
        {
            EnemyData.Health -= Damage;
        }
    }
}