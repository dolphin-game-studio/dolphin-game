using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DolphinGameException : Exception
{
    public DolphinGameException(string message) : base(message)
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

