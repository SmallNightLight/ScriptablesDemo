using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private List<WaveDataReference> _waves;
    [SerializeField] private Vector2Reference Path;

    private void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
    {
        for(int wave = 0; wave < _waves.Count; wave++)
        {
            yield return StartCoroutine(SpawnWave(wave));
            //yield return new Wait //wait until player has set up for next wave
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
            enemy.BaseEnemyData = enemyWave.Enemy; //.Copy()
            enemy.Path = Path;

            yield return new WaitForSeconds(_waves[waveIndex].Value.SpawnSpeed);
        }
    }
}