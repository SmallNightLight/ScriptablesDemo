using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, ISetupManager, IUpdateManager
{
    [SerializeField] private IntReference _coins;

    [Header("Data")]
    [SerializeField] private IntReference _enemyEndCount; //The amount of enemies that need to reach the end for game over
    [SerializeField] private IntReference _endCounter; //The amount of enemies that reached the end
    [SerializeField] private GameEvent _gameOverEvent;
    [SerializeField] private BoolReference _hasReachedEnd;
    [SerializeField] private BoolReference _restartEvent;
    [SerializeField] private BoolReference _selectingTower;

    [SerializeField, Range(0.0f, 5.0f)] private float _gameSpeed = 1.0f;

    public void Setup()
    {
        _restartEvent.Value = false;
        _gameSpeed = 1.0f;
    }

    public void EnemyDeath(EnemyDeathData enemyDeathData)
    {
        _coins.Value += enemyDeathData.EnemyData.Coins;
    }

    public void Update()
    {
        Time.timeScale = _gameSpeed;

        if (!_hasReachedEnd.Value && _endCounter.Value >= _enemyEndCount.Value)
        {
            _hasReachedEnd.Value = true;
            _gameSpeed = 0f;
            Debug.Log("Game Over");
            _gameOverEvent.Raise();   
        }

        CheckForRestart();
    }

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