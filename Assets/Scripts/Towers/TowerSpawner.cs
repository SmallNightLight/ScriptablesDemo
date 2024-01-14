using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private BoolReference _canPlaceTower;
    [SerializeField] private Vector3Reference _worldMousePosition;
    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private TowerDataReference _defaultTowerData;

    [Header("Snapping")]
    [SerializeField] private Vector3 _mouseOffset;
    [SerializeField] private Vector3 _objectOffset;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _previewSprite;
    private Grid _grid;

    [SerializeField] private GameEvent GameEvent;

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
            _previewSprite.transform.position = GetSnappedPosition(_worldMousePosition.Value);
        }
    }

    public void MouseDown(Vector3 worldMousePosition)
    {
        if (_canPlaceTower.Value)
            PlaceTower(GetSnappedPosition(worldMousePosition));
    }

    public void PlaceTower(Vector3 position)
    {
        GameObject newTower = Instantiate(_towerPrefab, position, Quaternion.identity);
        newTower.transform.SetParent(transform);
        newTower.GetComponent<Tower>().TowerData = _defaultTowerData;
    }

    private Vector3 GetSnappedPosition(Vector3 worldMousePosition)
    {
        Vector3Int cellPosition = _grid.WorldToCell(worldMousePosition + _mouseOffset);
        return _grid.GetCellCenterWorld(cellPosition) + _objectOffset;
    }
}