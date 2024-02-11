using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    /// <summary>
    /// Data to broadcast a message with a position and duration
    /// </summary>
    [System.Serializable]
    public class WorldTextMessage : IDataPoint
    {
        public string Message;
        public Vector2 Position;
        public float Duration = 2.0f;
    }
}