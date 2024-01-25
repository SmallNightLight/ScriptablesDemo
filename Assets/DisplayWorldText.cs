using UnityEngine;
using TMPro;
using ScriptableArchitecture.Data;

public class DisplayWorldText : MonoBehaviour
{
    [SerializeField] private GameObject _textPrefab;

    public void Display(WorldTextMessage message)
    {
        TMP_Text textAsset = Instantiate(_textPrefab, message.Position, Quaternion.identity, transform).GetComponent<TMP_Text>();

        textAsset.text = message.Message;
        Destroy(textAsset.gameObject, message.Duration);
    }
}