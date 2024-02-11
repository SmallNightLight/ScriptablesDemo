using ScriptableArchitecture.Data;
using UnityEngine;

/// <summary>
/// Debugging class to set some debugging features like instant killign enemies, invincible enemies and infinite money
/// </summary>
public class Debugging : MonoBehaviour
{
    [SerializeField] private BoolReference _infiniteCoins;
    [SerializeField] private IntReference _coins;

    [SerializeField] private BoolReference _instantKill;
    [SerializeField] private BoolReference _invincibleEnemies; //Enemies still takje damage but they cannot die

    /// <summary>
    /// Updates the debugging parameters
    /// </summary>
    private void Update()
    {
        if (_infiniteCoins.Value)
        {
            _coins.Value = 99999;
        }

        Effect.INSTANTKILL = _instantKill.Value;
    }
}