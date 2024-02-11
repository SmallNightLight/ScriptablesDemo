using ScriptableArchitecture.Core;
using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Spawns towers on the grid and handles player input related to towers like selecting, deselecting and previewing
/// </summary>
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
    [SerializeField] private Vector3IntReference _upgradeTowerEvent;
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

    [SerializeField] private List<TileBase> _pathTiles = new List<TileBase>();

    /// <summary>
    /// Initializes the tower spawner
    /// </summary>
    private void Start()
    {
        _grid = GetComponent<Grid>();
    }

    /// <summary>
    /// Handles player input for tower preview and selecting and deselecting
    /// </summary>
    private void Update()
    {
        if (_inTowerPreview.Value)
        {
            PreviewTower();
        }

        CheckInput();
    }

    /// <summary>
    /// Enables the tower preview feature with the values of the previewTower reference
    /// </summary>
    public void EnableTowerPreview()
    {
        _inTowerPreview.Value = true;
        _selectTowerEvent.Raise(_previewTower.Value.StartTower);
    }

    /// <summary>
    /// Disables the tower preview feature
    /// </summary>
    public void DisableTowerPreview()
    {
        _inTowerPreview.Value = false;
        _previewSprite.sprite = null;
    }

    /// <summary>
    /// Displays the tower preview with the correct visual representation when it is placable or not
    /// </summary>
    private void PreviewTower()
    {
        if (_previewSprite == null || _previewTower.Value.StartTower == null) return;

        _previewSprite.sprite = _previewTower.Value.StartTower.Sprite;

        Vector3Int cellPosition = GetCellPosition(_worldMousePosition.Value);
        bool isPlacable = !_towerCollection.Value.Towers.ContainsKey(cellPosition) && !IsPath(cellPosition) && HasCoins(_previewTower.Value.StartTower.Cost);

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

            if (!IsPath(GetCellPosition(_worldMousePosition.Value)) && _towerCollection.Value.TryGetTower(cellPosition, out TowerSingle tower))
            {
                _currentSelectedCell.Value = cellPosition;
                _upgradeTowerEvent.Value = cellPosition;
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

    /// <summary>
    /// Handles mouse events for tower placement
    /// </summary>
    public void MouseDown(Vector3 worldMousePosition)
    {
        if (_inTowerPreview.Value)
            PlaceTower(worldMousePosition);
    }

    /// <summary>
    /// Places a tower at the specified position with the given mouse position in world space
    /// </summary>
    public void PlaceTower(Vector3 worldMousePosition)
    {
        Vector3Int cellPosition = GetCellPosition(worldMousePosition);
        int cost = _previewTower.Value.StartTower.Cost;

        if (!HasCoins(cost) || IsPath(cellPosition) || !CanPlaceTower(cellPosition)) return;

        Vector3 position = GetSnappedPosition(cellPosition);
        GameObject newTower = Instantiate(_towerPrefab, position, Quaternion.identity);
        newTower.transform.SetParent(transform);

        Tower tower = newTower.GetComponent<Tower>();
        tower.TowerData.Value = _previewTower.Value;
        tower.CellPosition = cellPosition;

        _coins.Value -= cost;

        DisableTowerPreview();
    }

    /// <summary>
    /// Calculates the correct tower position for tower placement with a defined offset based on the cell position
    /// </summary>
    /// <returns>Tower position in world space</returns>
    private Vector3 GetSnappedPosition(Vector3Int cellPosition) => _grid.GetCellCenterWorld(cellPosition) + _objectOffset;

    /// <summary>
    /// Converts the given world mouse position to cell position
    /// </summary>
    /// <returns>Cell position in grid space.</returns>
    private Vector3Int GetCellPosition(Vector3 worldMousePosition) => _grid.WorldToCell(worldMousePosition + _mouseOffset);

    /// <summary>
    /// Checks if a tower can be placed at the specified cell position and the place is not already occupied by another tower
    /// </summary>
    /// <returns>True if a tower can be placed</returns>
    private bool CanPlaceTower(Vector3Int cellPosition)
    {
        return !_towerCollection.Value.Towers.ContainsKey(cellPosition);
    }

    /// <summary>
    /// Checks if the specified cell position is part of the path
    /// </summary>
    /// <returns>True if the cell is part of the path</returns>
    private bool IsPath(Vector3Int cellPosition)
    {
        Tilemap map = _groundTileMap.Value<Tilemap>();
        TileBase tile = map.GetTile(new Vector3Int(cellPosition.x, cellPosition.y, 0));
        return _pathTiles.Contains(tile);
    }

    /// <summary>
    /// Checks if the player has enough coins to for the given amount to be subtracted
    /// </summary>
    /// <returns>True if the player has enough coins</returns>
    private bool HasCoins(int amount) => _coins.Value >= amount;
}