using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private BoolReference _canPlaceTower;
    [SerializeField] private Vector3Reference _worldMousePosition;
    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private TowerDataReference _defaultTowerData;

    [SerializeField] private Color _previewPossible;
    [SerializeField] private Color _previewUnable;

    [Header("Snapping")]
    [SerializeField] private Vector3 _mouseOffset;
    [SerializeField] private Vector3 _objectOffset;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _previewSprite;
    private Grid _grid;

    private HashSet<Vector3Int> _currentTowerPositions = new HashSet<Vector3Int>();

    private void Start()
    {
        _grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (_canPlaceTower.Value && _previewSprite)
        {
            //Preview
            _previewSprite.sprite = _defaultTowerData.Value.Sprite;

            Vector3Int cellPosition = GetCellPosition(_worldMousePosition.Value);
            _previewSprite.color = _currentTowerPositions.Contains(cellPosition) ? _previewUnable : _previewPossible;
            _previewSprite.transform.position = GetSnappedPosition(cellPosition);
        }
    }

    public void MouseDown(Vector3 worldMousePosition)
    {
        if (_canPlaceTower.Value)
            PlaceTower(worldMousePosition);
    }

    public void PlaceTower(Vector3 worldMousePosition)
    {
        Vector3Int cellPosition = GetCellPosition(worldMousePosition);

        if (!AddTowerPosition(cellPosition))
            return;

        Vector3 position = GetSnappedPosition(cellPosition);
        GameObject newTower = Instantiate(_towerPrefab, position, Quaternion.identity);
        newTower.transform.SetParent(transform);
        newTower.GetComponent<Tower>().TowerData = _defaultTowerData;
    }

    private Vector3 GetSnappedPosition(Vector3Int cellPosition) => _grid.GetCellCenterWorld(cellPosition) + _objectOffset;

    private Vector3Int GetCellPosition(Vector3 worldMousePosition) => _grid.WorldToCell(worldMousePosition + _mouseOffset);

    private bool AddTowerPosition(Vector3Int cellPosition)
    {
        if (_currentTowerPositions.Contains(cellPosition))
            return false;

        _currentTowerPositions.Add(cellPosition);
        return true;
    }
}