using ScriptableArchitecture.Data;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Data")]
    public EnemyData EnemyData;
    public TowerSingle TowerData;

    [Header("Settings")]
    [SerializeField] private float _targetMargin;

    private void Start()
    {
        
    }

    private void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        //Moves towards the enemy position until it reaches the next target
        transform.position = Vector2.MoveTowards(transform.position, EnemyData.Position, TowerData.ProjectileSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, EnemyData.Position) < _targetMargin)
        {
            ApplyEffects();
            Destroy(gameObject);
        }
    }

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