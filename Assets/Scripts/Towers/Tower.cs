using ScriptableArchitecture.Data;
using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Data")]
    public TowerDataReference TowerData;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _towerRenderer;

    private TowerSingle _currentTower;
    private int _level;

    private void Start()
    {
        _level = 0;
        _currentTower = TowerData.Value.StartTower;
        UpdateTower();
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
}