using System.Collections;
using TMPro;
using UnityEngine;

public class DisplayWaveText: MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _displayTime;

    public void Display(int waveNumber)
    {
        _text.text = $"Completed Wave {waveNumber}";
        _text.enabled = true;
        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(_displayTime);
        _text.enabled = false;
    }
}