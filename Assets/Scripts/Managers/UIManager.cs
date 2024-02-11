using ScriptableArchitecture.Data;
using UnityEngine;

/// <summary>
/// Manages UI elements, like displaying messages when enemies die
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private WorldTextMessageReference _worldTextMessage;
    [SerializeField] private Vector2 _displayOffset;

    /// <summary>
    /// Raises an enemy death message with the correct text and position
    /// </summary>
    public void EnemyDeath(EnemyDeathData enemyDeathData)
    {
        WorldTextMessage message = new WorldTextMessage();
        message.Message = $"+{enemyDeathData.EnemyData.Coins}";
        message.Position = enemyDeathData.Position + _displayOffset;

        _worldTextMessage.Raise(message);
    }
}