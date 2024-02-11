using ScriptableArchitecture.Data;
using System.Collections;
using UnityEngine;

/// <summary>
/// Represents a tower object with the ability to attack enemies
/// </summary>
public class Tower : MonoBehaviour
{
    [Header("Data")]
    public TowerDataReference TowerData;
    public Vector3Int CellPosition;
    [SerializeField] private EnemyDataReference _enemyList;
    [SerializeField] private TowerCollectionReference _towerCollection;

    [Header("Prefabs")]
    [SerializeField] private GameObject _projectilePrefab;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _towerRenderer;

    private TowerSingle _currentTower;
    private int _level;
    private EnemyData _currentTarget;

    /// <summary>
    /// Initializes the tower
    /// </summary>
    private void Start()
    {
        _level = 0;
        _currentTower = TowerData.Value.StartTower;
        UpdateTower();

        StartCoroutine(Attacking());
    }

    /// <summary>
    /// Updates the tower
    /// </summary>
    private void Update()
    {
        CheckForTowerDestruction();
    }

    /// <summary>
    /// Checks if tower has been destroyed and destroys it if necessary
    /// </summary>
    private void CheckForTowerDestruction()
    {
        if (!_towerCollection.Value.Towers.ContainsKey(CellPosition))
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Upgrades the tower if the given cell position and its own cell position match. In addition it checks wheter a tower can even be upgraded further
    /// </summary>
    public void Upgrade(Vector3Int towerCellPosition)
    {
        if (CellPosition != towerCellPosition) return;

        if (_level >= TowerData.Value.UpgradeTowers.Count) return;

        _level++;
        _currentTower = TowerData.Value.UpgradeTowers[_level - 1];
        UpdateTower();
    }

    /// <summary>
    /// Updates the reference tower data and updates the visuals
    /// </summary>
    private void UpdateTower()
    {
        if (_towerRenderer != null && _currentTower != null)
            _towerRenderer.sprite = _currentTower.Sprite;

        _towerCollection.Value.Towers[CellPosition] = (TowerData.Value, _level);
    }

    /// <summary>
    /// The main Attack loop that is always active and tries to find enemies in range to attack
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attacking()
    {
        while (true)
        {
            yield return Attack() ? new WaitForSeconds(_currentTower.Interval) : null;
        }
    }

    /// <summary>
    /// Checks if the current target enemy is still active and in range and spawns a projectile towards it. 
    /// If this is not the case it tries to find a new enemy target
    /// </summary>
    /// <returns>True if it is currently attacking an enemy</returns>
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

    /// <summary>
    /// Gets the next target enemy for the tower to attack
    /// </summary>
    /// <returns>True if a target is found</returns>
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

    /// <summary>
    /// Checks if the given enemy is within range of the tower
    /// </summary>
    /// <returns>True if the enemy is in range</returns>
    private bool TargetInRange(EnemyData enemyData)
    {
        return !enemyData.IsDead && Vector2.Distance(transform.position, enemyData.Position) < _currentTower.Range;
    }
}