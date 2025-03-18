using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/HeroTemplate")]
    public class HeroTemplateSO : ScriptableObject
    {
        [SerializeField] private float _health;
        [SerializeField] private Sprite _portrait;
        [SerializeField] private SkillTemplateSO _skill1;
        [SerializeField] private SkillTemplateSO _skill2;

        public float Health => _health;
        public Sprite Portrait => _portrait;
        public SkillTemplateSO Skill1 => _skill1;
        public SkillTemplateSO Skill2 => _skill2;
    }
}