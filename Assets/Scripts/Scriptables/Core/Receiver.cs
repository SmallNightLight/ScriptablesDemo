using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    public class Receiver<T> : Receiver
    {
        [SerializeField]
        public T Value;

        public T Get()
        {
            return Value;
        }
    }

    public abstract class Receiver : ScriptableObject
    {
        
    }
}