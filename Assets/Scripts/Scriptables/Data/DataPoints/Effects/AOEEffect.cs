using UnityEngine;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// This is an AOE effect that deals secondary damage in to targets in the radius
    /// </summary>
    [CreateAssetMenu(fileName = "AOEEffect", menuName = "Effects/AOEEffect")]
    public class AOEEffect : Effect
    {
        public EnemyDataReference _enemyList;
        public float Damage;
        public float Radius;
        public float SecondaryDamage;

        /// <summary>
        /// Apply damage to the enemies in range instantly
        /// </summary>
        protected override void OnEnemyHit()
        {
            EnemyData.Health -= Damage;

            foreach(EnemyData enemy in _enemyList.RuntimeSet)
            {
                if (enemy != EnemyData && Vector2.Distance(enemy.Position, EnemyData.Position) < Radius)
                    enemy.Health -= SecondaryDamage;
            }
        }
    }
}