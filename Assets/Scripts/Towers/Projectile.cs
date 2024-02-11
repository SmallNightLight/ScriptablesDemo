using ScriptableArchitecture.Data;
using UnityEngine;

/// <summary>
/// Represents a projectile object that flies towards an enemy and applies effects on impact
/// </summary>
public class Projectile : MonoBehaviour
{
    [Header("Data")]
    public EnemyData EnemyData;
    public TowerSingle TowerData;

    [Header("Settings")]
    [SerializeField] private float _targetMargin;

    /// <summary>
    /// Updates the projectile position
    /// </summary>
    private void Update()
    {
        MoveTowardsTarget();
    }

    /// <summary>
    /// Moves the projectile towards the enemy position until it reaches it and applies the effects
    /// </summary>
    private void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, EnemyData.Position, TowerData.ProjectileSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, EnemyData.Position) < _targetMargin)
        {
            ApplyEffects();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Applies effects to the target enemy on impact
    /// </summary>
    private void ApplyEffects()
    {
        foreach (Effect baseEffect in TowerData.Effects)
        {
            if (baseEffect == null)
            {
                Debug.LogWarning("Cannot apply null effect");
                continue;
            }

            //Create copy of the baseEffect and add it to the enemy
            EnemyData.AddEffect(Instantiate(baseEffect));
        }
    }
}