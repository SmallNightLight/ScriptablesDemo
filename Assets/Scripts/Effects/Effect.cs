using ScriptableArchitecture.Data;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    [HideInInspector] public EnemyData EnemyData;

    [Tooltip("Duration in seconds to apply the effect - use 0 for instant effect")] public int Duration;

    private float _timer;
    private bool _effectStarted;
    private bool _effectEnded;

    public void ApplyEffect()
    {
        if (EnemyData == null)
        {
            Debug.LogWarning("No EnemyData - no effect applied!");
            return;
        }

        OnEnemyHit();

        _timer = 0f;
    }

    public void UpdateEffect(float deltaTime)
    {
        if (_effectEnded) return;

        if (EnemyData == null)
        {
            Debug.LogWarning("No EnemyData - no effect applied!");
            return;
        }

        if (!_effectStarted)
        {
            ApplyEffect();
            _effectStarted = true;
            return;
        }

        _timer += deltaTime;

        if (_timer >= Duration)
        {
            OnEffectEnd();
            _effectEnded = true;
            return;
        }

        OnUpdate(deltaTime);
    }

    public bool IsEnded() => _effectEnded;

    protected virtual void OnEnemyHit() { }
    protected virtual void OnUpdate(float deltaTime) { }
    protected virtual void OnEffectEnd() { }
}