using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/" + nameof(CharacterTemplateSO))]
    public class CharacterTemplateSO : ScriptableObject
    {
        [SerializeField] private int _health;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private BaseEquipmentTemplateSO _action1;
        [SerializeField] private BaseEquipmentTemplateSO _action2;

        public int Health => _health;
        public Sprite Portrait => _portrait;
        public BaseEquipmentTemplateSO Action1 => _action1;
        public BaseEquipmentTemplateSO Action2 => _action2;
    }
}