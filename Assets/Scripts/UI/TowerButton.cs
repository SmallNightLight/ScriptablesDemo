using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TowerButton : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private TowerDataReference _tower;
    
    [Header("Settings")]
    [SerializeField] private TowerDataReference _previewTowerEvent;

    private void Start()
    {
        Image image = GetComponent<Image>();
        image.sprite = _tower.Value.StartTower.Sprite;
    }

    public void Pressed()
    {
        _previewTowerEvent.Raise(_tower.Value);
    }
}