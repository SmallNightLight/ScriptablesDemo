using ScriptableArchitecture.Data;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRemove : MonoBehaviour
{
    [Header("Data")]
    
    [SerializeField] private BoolReference _inTowerPreview;
    [SerializeField] private BoolReference _deselectTowerInfoEvent;
     
    [SerializeField] private TowerCollectionReference _towerCollection;
    [SerializeField] private Vector3IntReference _currentSelectedCell;

    [Header("Components")]
    [SerializeField] private GameObject _buttonObject;

    private void Update()
    {
        SetButtonVisibility();
    }

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
        _towerCollection.Value.TowerBehaviour.Remove(_currentSelectedCell.Value);
        _deselectTowerInfoEvent.Raise(true);
    }
}