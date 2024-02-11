using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the logic for a button that selects a tower to preview
/// </summary>
[RequireComponent(typeof(Image))]
public class TowerButton : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private TowerDataReference _tower;
    
    [Header("Settings")]
    [SerializeField] private TowerDataReference _previewTowerEvent;

    /// <summary>
    /// Initializes the image to the correct tower sprite and adjusts its height to fit the aspec ratio
    /// </summary>
    private void Start()
    {
        Image image = GetComponent<Image>();
        RectTransform rectTransform = GetComponent<RectTransform>();

        image.sprite = _tower.Value.StartTower.Sprite;

        float aspectRatio = _tower.Value.StartTower.Sprite.rect.width / _tower.Value.StartTower.Sprite.rect.height;
        float width = rectTransform.rect.width;
        float height = width / aspectRatio;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    /// <summary>
    /// Raises the event for previewing the tower when the button is pressed
    /// </summary>
    public void Pressed()
    {
        _previewTowerEvent.Raise(_tower.Value);
    }
}