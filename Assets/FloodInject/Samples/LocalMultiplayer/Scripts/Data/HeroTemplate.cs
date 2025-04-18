using UnityEngine;

[CreateAssetMenu(menuName = "Samples/Local Multiplayer/" + nameof(HeroTemplate))]
public class HeroTemplate : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Portrait { get; private set; }
}
