using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgrade : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private BoolReference _inTowerPreview;
    [SerializeField] private Vector3IntReference _upgradeTowerEvent;
    [SerializeField] private Vector3IntReference _upgradeTowerFinishedEvent;
    [SerializeField] private IntReference _coins;
    [SerializeField] private TowerSingleReference _selectedTower;
    [SerializeField] private TowerCollectionReference _towerCollection;

    [Header("Components")]
    [SerializeField] private GameObject _buttonObject;
    [SerializeField] private Button _button;

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

        bool isVisible = !_inTowerPreview.Value && _towerCollection.Value.CanBeUpgraded(_upgradeTowerEvent.Value);
        _buttonObject.SetActive(isVisible);

        if (isVisible)
        {
            if (_button == null)
            {
                Debug.LogWarning("Button compoennt is null");
                return;
            }

            _button.interactable = _towerCollection.Value.CanBeBoughtNext(_upgradeTowerEvent.Value, _coins.Value);
        }
    }

    /// <summary>
    /// Upgarded the current selected tower if possible and updates the coins
    /// </summary>
    public void Upgrade()
    {
        //Check if enough coins are available to buy the upgrade
        if (_towerCollection.Value.TryGetTower(_upgradeTowerEvent.Value, out TowerSingle tower, 1) && _towerCollection.Value.CanBeBoughtNext(_upgradeTowerEvent.Value, _coins.Value))
        {
            _coins.Value -= tower.Cost;
            _upgradeTowerEvent.Raise();
            _upgradeTowerFinishedEvent.Raise(_upgradeTowerEvent.Value);
        }
    }
}