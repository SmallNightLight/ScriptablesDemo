using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Displays wave completion text for a specified duration on function call
/// </summary>
public class DisplayWaveText: MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _displayTime;

    /// <summary>
    /// Displays the wave completion message for the specified wave number
    /// </summary>
    public void Display(int waveNumber)
    {
        _text.text = $"Completed Wave {waveNumber}";
        _text.enabled = true;
        StartCoroutine(Hide());
    }

    /// <summary>
    /// Disables the text component after the display time
    /// </summary>
    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(_displayTime);
        _text.enabled = false;
    }
}