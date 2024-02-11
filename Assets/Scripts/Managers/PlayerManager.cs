using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages player-related functionalities such as coin collection, game speed control, and game over conditions.
/// Implements the ISetupManager and IUpdateManager interfaces, so this Monobehaviours needs a GameManager as a parent
/// </summary>
public class PlayerManager : MonoBehaviour, ISetupManager, IUpdateManager
{
    [Header("Data")]
    [SerializeField] private IntReference _coins;
    [SerializeField] private IntReference _enemyEndCount; //The amount of enemies that need to reach the end for game over
    [SerializeField] private IntReference _endCounter; //The amount of enemies that reached the end
    [SerializeField] private GameEvent _gameOverEvent;
    [SerializeField] private BoolReference _hasReachedEnd;
    [SerializeField] private BoolReference _restartEvent;
    [SerializeField] private BoolReference _selectingTower;

    [Header("Debugging")]
    [SerializeField, Range(0.0f, 10.0f)] private float _gameSpeed = 1.0f;

    /// <summary>
    /// Initializes some default values on game start
    /// </summary>
    public void Setup()
    {
        _restartEvent.Value = false;
        _gameSpeed = 1.0f;
    }

    /// <summary>
    /// Updates the coins based on the EnemyDeathData
    /// </summary>
    public void EnemyDeath(EnemyDeathData enemyDeathData)
    {
        _coins.Value += enemyDeathData.EnemyData.Coins;
    }

    /// <summary>
    /// Handles the game time and checks for player input
    /// </summary>
    public void Update()
    {
        ManageGameSpeed();
        CheckForEnd();
        CheckForRestart();
    }

    /// <summary>
    /// Handles the game speed and allows the arrows to change it
    /// </summary>
    private void ManageGameSpeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _gameSpeed *= 0.666f;
            ClampGameSpeed();
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _gameSpeed *= 1.5f;
            ClampGameSpeed();
        }

        Time.timeScale = _gameSpeed;
    }

    /// <summary>
    /// Clamps the game speed from 0.1 to 10
    /// </summary>
    private void ClampGameSpeed() => _gameSpeed = Mathf.Clamp(_gameSpeed, 0.1f, 5f);

    /// <summary>
    /// Checkf if the game has ended and calls the GameOver event
    /// </summary>
    private void CheckForEnd()
    {
        if (!_hasReachedEnd.Value && _endCounter.Value >= _enemyEndCount.Value)
        {
            _hasReachedEnd.Value = true;
            _gameSpeed = 0f;
            Debug.Log("Game Over");
            _gameOverEvent.Raise();
        }
    }

    /// <summary>
    /// Checks for the restart input and restarts the game if triggered
    /// </summary>
    private void CheckForRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _gameSpeed = 1.0f;
            _restartEvent.Raise(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}