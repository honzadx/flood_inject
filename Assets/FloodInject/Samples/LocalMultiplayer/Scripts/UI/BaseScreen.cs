using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseScreen : MonoBehaviour
{
    [SerializeField] Image _background;
    [SerializeField] ColorTemplate _backgroundColor;

    protected virtual void OnValidate()
    {
        if (_background == null)
        {
            _background = GetComponent<Image>();
        }
        if (_backgroundColor != null)
        {
            _background.color = _backgroundColor.Color;
        }
    }
}