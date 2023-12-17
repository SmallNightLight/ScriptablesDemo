using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent (typeof(Camera))]
public class CameraMouseProjection : MonoBehaviour
{
    public GameObject test;

    [Header("Set Scriptables")]
    [SerializeField] private Vector3Reference _worldMousePosition;
    [SerializeField] private Vector3Reference _currentMouseCellPosition;

    [Header("Get Scriptables")]


    [Header("Snapping")]
    [SerializeField] private Vector3 _mouseOffset;
    [SerializeField] private Vector3 _objectOffset;

    [Header("Components")]
    private Camera _camera;
    private Grid _grid;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _grid = FindObjectOfType<Grid>();
    }

    private void Update()
    {
        _worldMousePosition.Value = _camera.ScreenToWorldPoint(Input.mousePosition) + _mouseOffset;
        Vector3Int cellPosition = _grid.WorldToCell(_worldMousePosition.Value);
        _currentMouseCellPosition.Value = _grid.GetCellCenterWorld(cellPosition) + _objectOffset;

        test.transform.position = _currentMouseCellPosition.Value;
    }
}