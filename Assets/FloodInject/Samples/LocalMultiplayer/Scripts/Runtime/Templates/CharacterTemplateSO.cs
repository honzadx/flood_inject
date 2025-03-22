using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/" + nameof(CharacterTemplateSO))]
    public class CharacterTemplateSO : ScriptableObject
    {
        [SerializeField] private int _health;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private ActionTemplateSO _action1;
        [SerializeField] private ActionTemplateSO _action2;

        public int Health => _health;
        public Sprite Portrait => _portrait;
        public ActionTemplateSO Action1 => _action1;
        public ActionTemplateSO Action2 => _action2;
    }
}