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

    [SerializeField] private Color _previewPossible;
    [SerializeField] private Color _previewUnable;

    [Header("Snapping")]
    [SerializeField] private Vector3 _mouseOffset;
    [SerializeField] private Vector3 _objectOffset;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _previewSprite;
    [SerializeField] private Receiver _groundTileMap;
    private Grid _grid;

    private HashSet<Vector3Int> _currentTowerPositions = new HashSet<Vector3Int>();
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
    }

    public void EnableTowerPreview()
    {
        _inTowerPreview.Value = true;
    }

    private void PreviewTower()
    {
        if (_previewSprite == null || _previewTower.Value.StartTower == null) return;

        _previewSprite.sprite = _previewTower.Value.StartTower.Sprite;

        Vector3Int cellPosition = GetCellPosition(_worldMousePosition.Value);
        bool isPlacable = !_currentTowerPositions.Contains(cellPosition) && !IsPath(GetCellPosition(_worldMousePosition.Value));

        _previewSprite.color = isPlacable ? _previewPossible : _previewUnable;
        _previewSprite.transform.position = GetSnappedPosition(cellPosition);
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

        if (!HasCoins(cost) || IsPath(cellPosition) || !AddTowerPosition(cellPosition))
            return;

        Vector3 position = GetSnappedPosition(cellPosition);
        GameObject newTower = Instantiate(_towerPrefab, position, Quaternion.identity);
        newTower.transform.SetParent(transform);
        newTower.GetComponent<Tower>().TowerData = _previewTower;

        _coins.Value -= cost;

        _inTowerPreview.Value = false;
        _previewSprite.sprite = null;
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

    private bool IsPath(Vector3Int cellPosition)
    {
        Tilemap map = _groundTileMap.Value<Tilemap>();
        TileBase tile = map.GetTile(new Vector3Int(cellPosition.x, cellPosition.y, 0));
        return _pathTiles.Contains(tile);
    }

    private bool HasCoins(int amount) => _coins.Value >= amount;
}