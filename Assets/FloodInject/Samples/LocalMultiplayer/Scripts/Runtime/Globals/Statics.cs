using System.Collections.Generic;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public static class Statics
    {
        public static readonly int ToColorPropertyID = Shader.PropertyToID("_ToColor");
        public static Dictionary<string, InputDevice> ControlSchemeToInputDevice = new ()
        {
            { "Keyboard", InputDevice.Keyboard },
            { "Gamepad", InputDevice.Gamepad }
        };
    }
}