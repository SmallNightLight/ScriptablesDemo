using UnityEngine;

public class DisplayHealth : MonoBehaviour
{
    public float StartHealth;
    public float CurrentHealth;

    private void LateUpdate()
    {
        transform.localScale = new Vector3(CurrentHealth / (StartHealth == 0 ? 0.0001f : StartHealth), 1, 1);
    }
}