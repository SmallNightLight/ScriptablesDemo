using ScriptableArchitecture.Data;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Data")]
    public TowerDataReference TowerData;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _towerRenderer;

    private void Start()
    {
        if(_towerRenderer != null)
            _towerRenderer.sprite = TowerData.Value.Sprite;
    }
}