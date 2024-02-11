using UnityEngine;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// This is a burn effect that deals damage over time
    /// </summary>
    [CreateAssetMenu(fileName = "BurnEffect", menuName = "Effects/BurnEffect")]
    public class BurnEffect : Effect
    {
        [Tooltip("The total amount of damage applied over the duration")] public float TotalDamage;

        /// <summary>
        /// Deals damage based on the deltaTime
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void OnUpdate(float deltaTime)
        {
            EnemyData.Health -= TotalDamage * deltaTime;
        }
    }
}