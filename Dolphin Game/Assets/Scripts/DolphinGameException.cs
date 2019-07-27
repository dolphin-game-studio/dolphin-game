using System;
 


public class DolphinGameException : Exception
{
    public DolphinGameException(string message) : base(message)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

