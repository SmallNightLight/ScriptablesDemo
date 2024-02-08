using ScriptableArchitecture.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Effects/DamageEffect")]
public class DamageEffect : Effect 
{
    public float Damage;

    protected override void OnEnemyHit()
    {
        EnemyData.Heath -= Damage;
    }
}