using ScriptableArchitecture.Data;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISetupManager, IUpdateManager
{
    [SerializeField] private IntReference _coins;

    [SerializeField, Range(0.0f, 5.0f)] private float _gameSpeed = 1.0f;

    public void Setup()
    {
        _gameSpeed = 1.0f;
    }

    public void EnemyDeath(EnemyDeathData enemyDeathData)
    {
        _coins.Value += enemyDeathData.EnemyData.Coins;
    }

    public void Update()
    {
        Time.timeScale = _gameSpeed;
    }
}