using ScriptableArchitecture.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Data")]
    public EnemyDataReference BaseEnemyData;
    [SerializeField] private EnemyData _currentEnemyData;
    public Vector2Reference Path;

    [Header("Effects")]
    [SerializeField] private List<Effect> _currentEffects = new List<Effect>();

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
        UpdateEffect();

        _healthDisplayer.CurrentHealth = _currentEnemyData.Health;
    }

    private void UpdateEffect()
    {
        if (_currentEffects == null) return;

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

    public Effect baseEf;

    [ContextMenu("GFF")]
    public void GFF() => AddEffect(baseEf);

    public void AddEffect(Effect baseEffect)
    {
        if (baseEffect == null)
        {
            Debug.LogWarning("Cannot add effect - baseEffect is null");
            return;
        }

        Effect newEffect = Instantiate(baseEffect);
        newEffect.EnemyData = _currentEnemyData;
        _currentEffects.Add(newEffect);
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
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_reachedEnd) return;

        EnemyDeathData deathData = new EnemyDeathData();
        deathData.EnemyData = _currentEnemyData;
        deathData.Position = transform.position;

        _destroyEvent.Raise(deathData);
    }
}