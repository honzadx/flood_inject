using UnityEngine;

[CreateAssetMenu(menuName = "Samples/Local Multiplayer/" + nameof(ColorTemplate))]
public class ColorTemplate : ScriptableObject
{
    [field: SerializeField] public Color Color { get; private set; }
}
