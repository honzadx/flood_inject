using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalMultiplayer.Runtime
{
    public class ButtonUI : MonoBehaviour
    {
        public event Action ButtonPressedEvent;

        [SerializeField] private Button _button;
        [SerializeField] private string _text;
        [SerializeField] private TextMeshProUGUI _textElement;

        public void SetInteractable(bool interactable)
        {
            _button.interactable = interactable;
        }
        
        protected void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        protected void OnValidate()
        {
            _textElement.text = _text;
        }
        
        private void OnClick()
        {
            ButtonPressedEvent?.Invoke();
        }
    }
}
