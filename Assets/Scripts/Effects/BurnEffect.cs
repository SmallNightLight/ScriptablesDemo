using UnityEngine;

[CreateAssetMenu(fileName = "BurnEffect", menuName = "Effects/BurnEffect")]
public class BurnEffect : Effect
{
    [Tooltip("The total amount of damage applied over the duration")] public float TotalDamage;

    protected override void OnUpdate(float deltaTime)
    {
        EnemyData.Heath -= TotalDamage * deltaTime;
    }
}