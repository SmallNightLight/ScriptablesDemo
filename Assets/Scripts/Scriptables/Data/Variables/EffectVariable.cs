using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "EffectVariable", menuName = "Scriptables/Variables/Effect")]
    public class EffectVariable : Variable<Effect>
    {
    }
}