using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns waves of enemies based on on the provided waves and calls the wave events
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<WaveDataReference> _waves;
    [SerializeField] private Vector2Reference _path;
    [SerializeField] private EnemyDataReference _enemyList;
    [SerializeField] private IntReference _nextWaveEvent;
    [SerializeField] private IntReference _startBuildPhasevent;
    [SerializeField] private IntReference _endBuildPhasevent;
    [SerializeField] private GameEvent _gameFinishedEvent;

    [Header("Settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private FloatReference _buildTime;

    /// <summary>
    /// Initializes spawning of waves on start
    /// </summary>
    private void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }

    /// <summary>
    /// Spawns all waves in order
    /// </summary>
    private IEnumerator SpawnAllWaves()
    {
        if (_waves == null) yield break;

        for(int wave = 0; wave < _waves.Count; wave++)
        {
            _nextWaveEvent.Raise(wave + 1);
            yield return StartCoroutine(SpawnWave(wave));

            yield return WaitForEndOfWave();

            if (wave + 1 < _waves.Count)
            {
                _startBuildPhasevent.Raise(wave + 1);
                yield return new WaitForSeconds(_buildTime.Value);
                _endBuildPhasevent.Raise(wave + 1);
            }
        }

        _gameFinishedEvent.Raise();
    }

    /// <summary>
    /// Spawns a wave of enemies
    /// </summary>
    private IEnumerator SpawnWave(int waveIndex)
    {
        if (_waves[waveIndex].Value.EnemyData == null) yield break;

        for (int i = 0; i < _waves[waveIndex].Value.EnemyData.Count; i++)
        {
            yield return StartCoroutine(SpawnEnemyWave(waveIndex, i));
            yield return new WaitForSeconds(_waves[waveIndex].Value.EnemyData[i].Time);
        }
    }

    /// <summary>
    /// Spawns enemies for a sub wave
    /// </summary>
    private IEnumerator SpawnEnemyWave(int waveIndex, int enemyWaveIndex)
    {
        WaveData.EnemyWave enemyWave = _waves[waveIndex].Value.EnemyData[enemyWaveIndex];

        if (enemyWave == null) yield break;

        for (int i = 0; i < _waves[waveIndex].Value.EnemyData[enemyWaveIndex].Count; i++)
        {
            //Spawn enemy
            Enemy enemy = Instantiate(_enemyPrefab, transform).GetComponent<Enemy>();
            enemy.BaseEnemyData = enemyWave.Enemy;
            enemy.Path = _path;

            yield return new WaitForSeconds(_waves[waveIndex].Value.SpawnSpeed);
        }
    }

    /// <summary>
    /// Waits for the end of the wave before proceeding to the next action
    /// </summary>
    private IEnumerator WaitForEndOfWave()
    {
        while (true)
        {
            if (_enemyList.RuntimeSet.Count == 0) break;

            yield return null;
        }
    }
}