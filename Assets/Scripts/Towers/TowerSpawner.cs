using ScriptableArchitecture.Core;
using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class TowerSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private IntReference _coins;
    [SerializeField] private BoolReference _canPlaceTower;
    [SerializeField] private BoolReference _inTowerPreview;
    [SerializeField] private Vector3Reference _worldMousePosition;
    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private TowerDataReference _previewTower;

    [SerializeField] private TowerCollectionReference _towerCollection;
    [SerializeField] private TowerSingleReference _selectTowerEvent;
    [SerializeField] private BoolReference _deselectTowerEvent;
    [SerializeField] private Vector3IntReference _currentSelectedCell;

    [SerializeField] private Color _previewPossible;
    [SerializeField] private Color _previewUnable;

    [Header("Snapping")]
    [SerializeField] private Vector3 _mouseOffset;
    [SerializeField] private Vector3 _objectOffset;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _previewSprite;
    [SerializeField] private Receiver _groundTileMap;
    private Grid _grid;

    private Dictionary<Vector3Int, TowerData> _currentTowerPositions = new Dictionary<Vector3Int, TowerData>();
    [SerializeField] private List<TileBase> _pathTiles = new List<TileBase>();

    private void Start()
    {
        _grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (_inTowerPreview.Value)
        {
            PreviewTower();
        }
        else
        {
            //CheckInput();
        }

        CheckInput();
    }

    public void EnableTowerPreview()
    {
        _inTowerPreview.Value = true;
        _selectTowerEvent.Raise(_previewTower.Value.StartTower);
    }

    public void DisableTowerPreview()
    {
        _inTowerPreview.Value = false;
        _previewSprite.sprite = null;
    }

    private void PreviewTower()
    {
        if (_previewSprite == null || _previewTower.Value.StartTower == null) return;

        _previewSprite.sprite = _previewTower.Value.StartTower.Sprite;

        Vector3Int cellPosition = GetCellPosition(_worldMousePosition.Value);
        bool isPlacable = !_currentTowerPositions.ContainsKey(cellPosition) && !IsPath(cellPosition) && HasCoins(_previewTower.Value.StartTower.Cost);

        _previewSprite.color = isPlacable ? _previewPossible : _previewUnable;
        _previewSprite.transform.position = GetSnappedPosition(cellPosition);
    }

    /// <summary>
    /// This function handles deselecting and selecting towers
    /// </summary>
    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int cellPosition = GetCellPosition(_worldMousePosition.Value);
            bool hasCellTower = _towerCollection.Value.Towers.ContainsKey(cellPosition) && !IsPath(GetCellPosition(_worldMousePosition.Value));

            if (hasCellTower && _towerCollection.Value.Towers.TryGetValue(cellPosition, out TowerSingle tower))
            {
                _currentSelectedCell.Value = cellPosition;
                _selectTowerEvent.Raise(tower);
            }
            else
            {
                _deselectTowerEvent.Raise(false);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            _deselectTowerEvent.Raise(true);
            DisableTowerPreview();
        }
    }

    public void MouseDown(Vector3 worldMousePosition)
    {
        if (_inTowerPreview.Value)
            PlaceTower(worldMousePosition);
    }

    public void PlaceTower(Vector3 worldMousePosition)
    {
        Vector3Int cellPosition = GetCellPosition(worldMousePosition);
        int cost = _previewTower.Value.StartTower.Cost;

        if (!HasCoins(cost) || IsPath(cellPosition) || !AddTowerPosition(cellPosition, _previewTower.Value))
            return;

        Vector3 position = GetSnappedPosition(cellPosition);
        GameObject newTower = Instantiate(_towerPrefab, position, Quaternion.identity);
        newTower.transform.SetParent(transform);

        Tower tower = newTower.GetComponent<Tower>();
        tower.TowerData = _previewTower;
        tower.CellPosition = cellPosition;

        _coins.Value -= cost;

        DisableTowerPreview();
    }

    private Vector3 GetSnappedPosition(Vector3Int cellPosition) => _grid.GetCellCenterWorld(cellPosition) + _objectOffset;

    private Vector3Int GetCellPosition(Vector3 worldMousePosition) => _grid.WorldToCell(worldMousePosition + _mouseOffset);

    private bool AddTowerPosition(Vector3Int cellPosition, TowerData tower)
    {
        if (_currentTowerPositions.ContainsKey(cellPosition))
            return false;

        _currentTowerPositions.Add(cellPosition, tower);
        return true;
    }

    private bool IsPath(Vector3Int cellPosition)
    {
        Tilemap map = _groundTileMap.Value<Tilemap>();
        TileBase tile = map.GetTile(new Vector3Int(cellPosition.x, cellPosition.y, 0));
        return _pathTiles.Contains(tile);
    }

    private bool HasCoins(int amount) => _coins.Value >= amount;
}