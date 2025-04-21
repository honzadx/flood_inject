using UnityEngine;

[CreateAssetMenu(menuName = "Samples/Local Multiplayer/" + nameof(HeroTemplate))]
public class HeroTemplate : ScriptableObject
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Portrait { get; private set; }
    
    [Header("Combat")]
    [field: SerializeField] public BaseAttackBehavior AttackBehavior { get; private set; }
    [field: SerializeField] public float AttackCooldown { get; private set; } = 0.5f;
}
