using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class WorldTextMessage : IDataPoint
    {
        public string Message;
        public Vector2 Position;
    }
}