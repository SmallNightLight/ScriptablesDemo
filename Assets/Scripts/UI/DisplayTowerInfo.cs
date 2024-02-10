using ScriptableArchitecture.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayTowerInfo : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private TowerCollectionReference _towerCollection;
    [SerializeField] private Vector3IntReference _currentSelectedCell;
    [SerializeField] private BoolReference _inTowerPreview;

    [Header("Components")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private TMP_Text _upgradeCost;
    [SerializeField] private TMP_Text _range;
    [SerializeField] private TMP_Text _interval;
    [SerializeField] private TMP_Text _speed;
   

    private void Start()
    {
        _panel.SetActive(false);
    }

    public void SelectTower(TowerSingle tower)
    {
        _name.text = tower.Name;
        _cost.text = $"Cost: {tower.Cost}";

        if (_inTowerPreview.Value)
        {
            _upgradeCost.gameObject.SetActive(false);
        }
        else if (_towerCollection.Value.TryGetTower(_currentSelectedCell.Value, out TowerSingle nextTower, 1))
        {
            _upgradeCost.gameObject.SetActive(true);
            _upgradeCost.text = $"Upgrade cost: {nextTower.Cost}";
        }
        else
        {
            _upgradeCost.gameObject.SetActive(true);
            _upgradeCost.text = $"Completly upgraded";
        }
        
        _range.text = $"Range: {tower.Range}";
        _interval.text = $"Interval: {tower.Interval}";
        _speed.text = $"Speed: {tower.ProjectileSpeed}";

        _panel.SetActive(true);
    }

    public void UpgradeTower(Vector3Int towerCellPosition)
    {
        if (_towerCollection.Value.TryGetTower(towerCellPosition, out TowerSingle tower))
        {
            SelectTower(tower);
        }
    }

    public void Deselect(bool immediated = false)
    {
        if (immediated || !EventSystem.current.IsPointerOverGameObject())
             _panel.SetActive(false);
    }
}