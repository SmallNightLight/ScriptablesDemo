using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "DoubleGameEvent", menuName = "Scriptables/GameEvents/DoubleGameEvent")]
    public class DoubleGameEvent : GameEventBase<double>
    {
    }
}