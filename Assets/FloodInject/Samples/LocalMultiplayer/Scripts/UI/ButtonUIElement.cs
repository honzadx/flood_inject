using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ButtonUIElement : MonoBehaviour
{
    public event Action ClickedEvent;
    
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Button _button;
    [SerializeField] string _displayText;

    [SerializeField] private ColorTemplate _textColor;

    public bool Interactable
    {
        get => _button.interactable;
        set => _button.interactable = value;
    }
    
    private void Start()
    {
        _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        ClickedEvent?.Invoke();
    }

    private void OnValidate()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
        }
        if (_text == null)
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (_textColor != null)
        {
            _text.color = _textColor.Color;
        }
        _text.text = _displayText;
    }
}