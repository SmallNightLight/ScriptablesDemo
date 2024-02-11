using ScriptableArchitecture.Data;
using UnityEngine;

/// <summary>
/// Projects mouse position from screen space to world space and updates the reference
/// </summary>
[RequireComponent (typeof(Camera))]
public class CameraMouseProjection : MonoBehaviour
{
    [Header("Set Scriptables")]
    [SerializeField] private Vector3Reference _worldMousePosition;

    [Header("Components")]
    private Camera _camera;

    /// <summary>
    /// Initializes the camera component
    /// </summary>
    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    /// <summary>
    /// Updates the world mouse position based on screen mouse position.
    /// </summary>
    private void Update()
    {
        _worldMousePosition.Value = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
            _worldMousePosition.Raise();
    }
}