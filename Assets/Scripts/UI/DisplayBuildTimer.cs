using ScriptableArchitecture.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBuildTimer : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FloatReference _buildTime;

    [Header("Settings")]
    [SerializeField] private float _targetHeight;

    [Header("Components")]
    [SerializeField] private Slider _slider;

    private void Start()
    {
        SetHeight(0);
    }

    public void StartBuildPhase()
    {
        StartCoroutine(BuildTimer());
    }

    private IEnumerator BuildTimer()
    {
        SetHeight(_targetHeight);
        float elapsedTime = 0f;

        while (elapsedTime < _buildTime.Value)
        {
            elapsedTime += Time.deltaTime;
            _slider.value = elapsedTime / _buildTime.Value;
            yield return null;
        }

        _slider.value = 1;
        SetHeight(0);
    }

    private void SetHeight(float newHeight)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogWarning("RectTransform not found");
            return;
        }

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);

    }
}