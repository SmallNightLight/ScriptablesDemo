using ScriptableArchitecture.Data;
using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Data")]
    public TowerDataReference TowerData;
    [SerializeField] private EnemyDataReference _enemyList;

    [Header("Prefabs")]
    [SerializeField] private GameObject _projectilePrefab;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _towerRenderer;

    private TowerSingle _currentTower;
    private int _level;

    private EnemyData _currentTarget;

    private void Start()
    {
        _level = 0;
        _currentTower = TowerData.Value.StartTower;
        UpdateTower();

        StartCoroutine(Attacking());
    }

    public void Upgrade()
    {
        _level++;
        _currentTower = TowerData.Value.UpgradeTowers[_level - 1];
        UpdateTower();
    }

    private void UpdateTower()
    {
        if (_towerRenderer != null && _currentTower != null)
            _towerRenderer.sprite = _currentTower.Sprite;
    }

    private IEnumerator Attacking()
    {
        while (true)
        {
            yield return Attack() ? new WaitForSeconds(_currentTower.Interval) : null;
        }
    }

    private bool Attack()
    {
        if ((_currentTarget != null && TargetInRange(_currentTarget)) || GetNextTarget(out _currentTarget))
        {
            Projectile projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();

            if (projectile == null)
            {
                Debug.LogWarning("No projectile component found on prefab");
                return false;
            }

            projectile.EnemyData = _currentTarget;
            projectile.TowerData = _currentTower;

            return true;
        }

        return false;
    }

    private bool GetNextTarget(out EnemyData enemyData)
    {
        for(int i = 0; i < _enemyList.RuntimeSet.Count; i++)
        {
            if (TargetInRange(_enemyList.RuntimeSet[i]))
            {
                enemyData = _enemyList.RuntimeSet[i];
                return true;
            }
        }

        enemyData = null;
        return false;
    }

    private bool TargetInRange(EnemyData enemyData)
    {
        return !enemyData.IsDead && Vector2.Distance(transform.position, enemyData.Position) < _currentTower.Range;
    }
}