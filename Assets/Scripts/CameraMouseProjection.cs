using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent (typeof(Camera))]
public class CameraMouseProjection : MonoBehaviour
{
    [Header("Set Scriptables")]
    [SerializeField] private Vector3Reference _worldMousePosition;
    [SerializeField] private Vector3GameEvent _mouseDownEvent;

    [Header("Components")]
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        _worldMousePosition.Value = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
            _mouseDownEvent.Raise(_worldMousePosition.Value);
    }
}