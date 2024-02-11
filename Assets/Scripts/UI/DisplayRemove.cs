using ScriptableArchitecture.Data;
using UnityEngine;

/// <summary>
/// Displays and handles removal a tower from the grid
/// </summary>
public class DisplayRemove : MonoBehaviour
{
    [Header("Data")]
    
    [SerializeField] private BoolReference _inTowerPreview;
    [SerializeField] private BoolReference _deselectTowerInfoEvent;
     
    [SerializeField] private TowerCollectionReference _towerCollection;
    [SerializeField] private Vector3IntReference _currentSelectedCell;

    [Header("Components")]
    [SerializeField] private GameObject _buttonObject;

    /// <summary>
    /// Updates the button visibility
    /// </summary>
    private void Update()
    {
        SetButtonVisibility();
    }

    /// <summary>
    /// Sets the visibility of the button based whether the currently selected tower is a preview tower
    /// </summary>
    private void SetButtonVisibility()
    {
        if (_buttonObject == null)
        {
            Debug.LogWarning("Button object is null");
            return;
        }

        _buttonObject.SetActive(!_inTowerPreview.Value);
    }

    /// <summary>
    /// Remove the tower at the given cell from the list
    /// </summary>
    public void Remove()
    {
        _towerCollection.Value.Towers.Remove(_currentSelectedCell.Value);
        _deselectTowerInfoEvent.Raise(true);
    }
}