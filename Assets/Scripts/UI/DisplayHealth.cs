using UnityEngine;

/// <summary>
/// Displays health using the scale of the transform
/// </summary>
public class DisplayHealth : MonoBehaviour
{
    public float StartHealth;
    public float CurrentHealth;

    /// <summary>
    /// Updates the scale of the object based on the given start and current health
    /// </summary>
    private void LateUpdate()
    {
        transform.localScale = new Vector3(CurrentHealth / (StartHealth == 0 ? 0.0001f : StartHealth), 1, 1);
    }
}