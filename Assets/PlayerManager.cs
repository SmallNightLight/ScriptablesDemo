using ScriptableArchitecture.Data;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISetupManager
{
    [SerializeField] private IntReference _coins;

    public void Setup()
    {
        
    }

    public void EnemyDeath(EnemyDeathData enemyDeathData)
    {
        _coins.Value += enemyDeathData.EnemyData.Coins;
    }
}