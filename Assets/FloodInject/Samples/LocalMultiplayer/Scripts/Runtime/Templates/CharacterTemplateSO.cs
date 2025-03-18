using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/CharacterTemplate")]
    public class CharacterTemplateSO : ScriptableObject
    {
        [SerializeField] private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private PlayerBehaviour _prefab;
        [SerializeField] private ActionTemplateSO _action1;
        [SerializeField] private ActionTemplateSO _action2;

        public float Health => _health;
        public Sprite Portrait => _portrait;
        public PlayerBehaviour Prefab => _prefab;
        public ActionTemplateSO Action1 => _action1;
        public ActionTemplateSO Action2 => _action2;
    }
}