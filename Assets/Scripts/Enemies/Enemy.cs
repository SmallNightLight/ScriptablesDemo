using ScriptableArchitecture.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Data")]
    public EnemyDataReference EnemyData;
    public Vector2Reference Path;

    [Header("Effects")]
    [SerializeField] private List<Effect> _currentEffects;

    [Header("Settings")]
    [SerializeField] private float _targetMargin;
    [SerializeField] private EnemyDeathDataReference _destroyEvent;
    [SerializeField] private IntReference _endCount;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _enemyRenderer;
    [SerializeField] private DisplayHealth _healthDisplayer;

    private int _pathIndex = 0;
    private float _currentHealth;
    private bool _reachedEnd;

    private void Start()
    {
        if (_enemyRenderer != null)
            _enemyRenderer.sprite = EnemyData.Value.Sprite;

        if (Path.RuntimeSet.Count() > 0)
            transform.position = Path.RuntimeSet[0];

        _currentHealth = EnemyData.Value.Heath;

        if (_healthDisplayer != null)
        {
            _healthDisplayer.StartHealth = _currentHealth;
            _healthDisplayer.CurrentHealth = _currentHealth;
        }
    }

    private void Update()
    {
        MoveTowardsTarget();
        UpdateEffect();

        _healthDisplayer.CurrentHealth = _currentHealth;
    }

    private void UpdateEffect()
    {
        for (int i = _currentEffects.Count - 1; i >= 0; i--)
        {
            if (_currentEffects[i] == null || _currentEffects[i].IsEnded())
            {
                _currentEffects.RemoveAt(i);
                continue;
            }

            _currentEffects[i].UpdateEffect(Time.deltaTime);
        }
    }

    public void AddEffect(Effect baseEffect)
    {
        if (baseEffect == null)
        {
            Debug.LogWarning("Cannot add effect - baseEffect is null");
            return;
        }

        Effect newEffect = Instantiate(baseEffect);
        newEffect.EnemyData = EnemyData.Value;
        _currentEffects.Add(newEffect);
    }

    private void MoveTowardsTarget()
    {
        if (_pathIndex < Path.RuntimeSet.Count())
        {
            //Move along the path until it reaches the next target
            transform.position = Vector2.MoveTowards(transform.position, Path.RuntimeSet[_pathIndex], EnemyData.Value.Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, Path.RuntimeSet[_pathIndex]) < _targetMargin)
                _pathIndex++;
        }
        else
        {
            //Reached end of path
            Debug.Log("Enemy reached end");
            _reachedEnd = true;
            _endCount.Value++;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_reachedEnd) return;

        EnemyDeathData deathData = new EnemyDeathData();
        deathData.EnemyData = EnemyData.Value;
        deathData.Position = transform.position;

        _destroyEvent.Raise(deathData);
    }
}