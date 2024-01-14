using UnityEngine;

namespace ScriptableArchitecture.Core
{
    public abstract class Variable<T> : Variable
    {
        [SerializeField]
        public T Value;
        public InitializeType Type;
        public T StartValue;

        //OnEnable is called when the first scene is loaded, regardles if the scene has a reference to it (build game)
        private void OnEnable()
        {
            //This is only for the editor as the build game does not save Scriptable objects across sessions
            if (Type == InitializeType.ResetOnGameStart)
                Value = StartValue;
        }

        public void Set(T value) => Value = value;
    }

    public abstract class Variable : ScriptableObject { }

    public enum InitializeType
    {
        Normal, ResetOnGameStart, ReadOnly
    }
}