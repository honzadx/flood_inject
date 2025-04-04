using UnityEngine;

[CreateAssetMenu(menuName = "Samples/Local Multiplayer/" + nameof(HeroSelection), order = 1)]
public class HeroSelection : ScriptableObject
{
    [field: SerializeField] public HeroTemplate[] Heroes { get; private set; }
}
