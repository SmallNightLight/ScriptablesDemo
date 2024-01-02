using ScriptableArchitecture.Data;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerDataReference TowerData;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _towerRenderer;

    private void Start()
    {
        _towerRenderer.sprite = TowerData.Value.Sprite;
    }
}