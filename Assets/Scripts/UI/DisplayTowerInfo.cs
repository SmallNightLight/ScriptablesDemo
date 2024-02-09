using ScriptableArchitecture.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayTowerInfo : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _cost;
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
        _range.text = $"Range: {tower.Range}";
        _interval.text = $"Interval: {tower.Interval}";
        _speed.text = $"Speed: {tower.ProjectileSpeed}";

        _panel.SetActive(true);
    }

    public void Deselect(bool immediated = false)
    {
        if (immediated || !EventSystem.current.IsPointerOverGameObject())
             _panel.SetActive(false);
    }
}