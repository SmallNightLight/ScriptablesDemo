using ScriptableArchitecture.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the build timer using a slider UI element
/// </summary>
public class DisplayBuildTimer : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FloatReference _buildTime;

    [Header("Settings")]
    [SerializeField] private float _targetHeight;

    [Header("Components")]
    [SerializeField] private Slider _slider;

    /// <summary>
    /// Initialization, hides the object by settings the height to 0
    /// </summary>
    private void Start()
    {
        SetHeight(0);
    }

    /// <summary>
    /// Starts the build phase
    /// </summary>
    public void StartBuildPhase()
    {
        StartCoroutine(BuildTimer());
    }

    /// <summary>
    /// Updates the slider to to move from 0 to 1 in the buildTime
    /// Hides the object at the end by setting the hieght to 0
    /// </summary>
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

    /// <summary>
    /// Sets the height of rect transform
    /// </summary>
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