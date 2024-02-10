using ScriptableArchitecture.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Data")]
    public EnemyDataReference BaseEnemyData;
    [SerializeField] private EnemyDataReference _enemyList;
    public Vector2Reference Path;

    [SerializeField] private EnemyData _currentEnemyData;

    [Header("Settings")]
    [SerializeField] private float _targetMargin;
    [SerializeField] private EnemyDeathDataReference _destroyEvent;
    [SerializeField] private IntReference _endCount;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _enemyRenderer;
    [SerializeField] private DisplayHealth _healthDisplayer;

    private int _pathIndex = 0;
    private bool _reachedEnd;

    private void Start()
    {
        _currentEnemyData = BaseEnemyData.Value.Copy();
        _enemyList.Add(_currentEnemyData);

        if (_enemyRenderer != null)
            _enemyRenderer.sprite = _currentEnemyData.Sprite;

        if (Path.RuntimeSet.Count() > 0)
            transform.position = Path.RuntimeSet[0];

        if (_healthDisplayer != null)
        {
            _healthDisplayer.StartHealth = _currentEnemyData.Health;
            _healthDisplayer.CurrentHealth = _currentEnemyData.Health;
        }
    }

    private void Update()
    {
        MoveTowardsTarget();
        UpdatePosition();
        _currentEnemyData?.UpdateEffects();

        if (_currentEnemyData.Health <= 0.5f)
            Death();

        _healthDisplayer.CurrentHealth = Mathf.Max(_currentEnemyData.Health, 0);
    }

    private void UpdatePosition()
    {
        _currentEnemyData.Position = transform.position;
    }

    private void MoveTowardsTarget()
    {
        if (_pathIndex < Path.RuntimeSet.Count())
        {
            //Move along the path until it reaches the next target
            transform.position = Vector2.MoveTowards(transform.position, Path.RuntimeSet[_pathIndex], _currentEnemyData.Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, Path.RuntimeSet[_pathIndex]) < _targetMargin)
                _pathIndex++;
        }
        else
        {
            //Reached end of path
            Debug.Log("Enemy reached end");
            _reachedEnd = true;
            _endCount.Value++;
            Death();
        }
    }

    private void Death()
    {
        _currentEnemyData.IsDead = true;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _enemyList.Remove(_currentEnemyData);

        if (_reachedEnd) return;

        EnemyDeathData deathData = new EnemyDeathData();
        deathData.EnemyData = _currentEnemyData;
        deathData.Position = transform.position;

        _destroyEvent.Raise(deathData);
    }
}