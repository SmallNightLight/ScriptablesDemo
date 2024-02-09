using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<WaveDataReference> _waves;
    [SerializeField] private Vector2Reference _path;
    [SerializeField] private EnemyDataReference _enemyList;
    [SerializeField] private IntReference _nextWaveEvent;
    [SerializeField] private IntReference _startBuildPhasevent;
    [SerializeField] private IntReference _endBuildPhasevent;

    [Header("Settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private FloatReference _buildTime;


    private void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
    {
        for(int wave = 0; wave < _waves.Count; wave++)
        {
            _nextWaveEvent.Raise(wave + 1);
            yield return StartCoroutine(SpawnWave(wave));

            yield return WaitForEndOfWave();

            _startBuildPhasevent.Raise(wave + 1);
            yield return new WaitForSeconds(_buildTime.Value);
            _endBuildPhasevent.Raise(wave + 1);
        }
    }

    private IEnumerator SpawnWave(int waveIndex)
    {
        for (int i = 0; i < _waves[waveIndex].Value.EnemyData.Count; i++)
        {
            yield return StartCoroutine(SpawnEnemyWave(waveIndex, i));
            yield return new WaitForSeconds(_waves[waveIndex].Value.EnemyData[i].Time);
        }
    }

    private IEnumerator SpawnEnemyWave(int waveIndex, int enemyWaveIndex)
    {
        WaveData.EnemyWave enemyWave = _waves[waveIndex].Value.EnemyData[enemyWaveIndex];

        for (int i = 0; i < _waves[waveIndex].Value.EnemyData[enemyWaveIndex].Count; i++)
        {
            //Spawn enemy
            Enemy enemy = Instantiate(_enemyPrefab, transform).GetComponent<Enemy>();
            enemy.BaseEnemyData = enemyWave.Enemy;
            enemy.Path = _path;

            yield return new WaitForSeconds(_waves[waveIndex].Value.SpawnSpeed);
        }
    }

    private IEnumerator WaitForEndOfWave()
    {
        while (true)
        {
            if (_enemyList.RuntimeSet.Count == 0) break;

            yield return null;
        }
    }
}