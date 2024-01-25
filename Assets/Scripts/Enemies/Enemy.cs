using ScriptableArchitecture.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Data")]
    public EnemyDataReference EnemyData;
    public Vector2Reference Path;

    [Header("Settings")]
    [SerializeField] private float _targetMargin;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _enemyRenderer;

    private int _pathIndex;

    private void Start()
    {
        if (_enemyRenderer != null)
            _enemyRenderer.sprite = EnemyData.Value.Sprite;

        if (Path.RuntimeSet.Count() > 0)
            transform.position = Path.RuntimeSet[0];
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (_pathIndex < Path.RuntimeSet.Count())
        {
            //Move along the path until it reaches the next target
            transform.position = Vector2.MoveTowards(transform.position, Path.RuntimeSet[_pathIndex], EnemyData.Value.Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, Path.RuntimeSet[_pathIndex]) < _targetMargin)
                _pathIndex++;
        }
        else
        {
            //Reached end of path
            Debug.Log("Game over");
        }
    }
}