using ScriptableArchitecture.Data;
using UnityEngine;

public class UIManager : MonoBehaviour, ISetupManager
{
    [SerializeField] private WorldTextMessageReference _worldTextMessage;
    [SerializeField] private Vector2 _displayOffset;


    void ISetupManager.Setup()
    {
        
    }

    public void EnemyDeath(EnemyDeathData enemyDeathData)
    {
        WorldTextMessage message = new WorldTextMessage();
        message.Message = $"+{enemyDeathData.EnemyData.Coins}";
        message.Position = enemyDeathData.Position + _displayOffset;

        _worldTextMessage.Raise(message);
    }
}