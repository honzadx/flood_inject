using System;
using UnityEngine;
using UnityEngine.UI;

namespace LocalMultiplayer.Runtime
{
    public class InputIconElement : MonoBehaviour
    {
        [Serializable]
        private struct DeviceInputIconMapping 
        {
            public InputDevice inputDevice;
            public Sprite icon;
        }
        
        [SerializeField] private DeviceInputIconMapping[] _mappings;
        [SerializeField] private Image _icon;
        
        public void Init(InputDevice inputDevice)
        {
            foreach (var mapping in _mappings)
            {
                if (mapping.inputDevice == inputDevice)
                {
                    _icon.sprite = mapping.icon;
                    break;
                }
            }
        }
    }
}
