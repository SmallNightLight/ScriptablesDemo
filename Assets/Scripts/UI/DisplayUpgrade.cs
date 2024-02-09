using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgrade : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private BoolReference _inTowerPreview;
    [SerializeField] private Vector3IntReference _upgradeTowerEvent;
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

        bool isVisible = !_inTowerPreview.Value && CanBeUpgraded();
        _buttonObject.SetActive(isVisible);

        if (isVisible)
        {
            if (_button == null)
            {
                Debug.LogWarning("Button compoennt is null");
                return;
            }

            _button.interactable = CanBeBought();
        }
    }

    /// <summary>
    /// Upgarded the current selected tower if possible and updates the coins
    /// </summary>
    public void Upgrade()
    {
        //Check if enough coins are available to buy the upgrade
        if (CanBeUpgraded() && CanBeBought())
        {
            _coins.Value -= _selectedTower.Value.Cost;
            _upgradeTowerEvent.Raise();
        }
    }

    private bool CanBeUpgraded()
    {
        if (_towerCollection.Value.TowerBehaviour.TryGetValue(_upgradeTowerEvent.Value, out (TowerData, int) towerBehaviour))
        {
            TowerData towerData = towerBehaviour.Item1;
            int level = towerBehaviour.Item2;

            return level < towerData.UpgradeTowers.Count;
        }

        return false;        
    }

    private bool CanBeBought()
    {
        if (_towerCollection.Value.TowerBehaviour.TryGetValue(_upgradeTowerEvent.Value, out (TowerData, int) towerBehaviour))
        {
            TowerData towerData = towerBehaviour.Item1;
            int level = towerBehaviour.Item2;

            if (level + 1 > towerData.UpgradeTowers.Count) return false;

            TowerSingle tower = towerData.GetTower(level + 1);
            return _coins.Value >= tower.Cost;
        }

        return false;
    }
}