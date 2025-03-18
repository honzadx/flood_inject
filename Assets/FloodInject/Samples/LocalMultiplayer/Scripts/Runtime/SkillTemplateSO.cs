using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/SkillTemplate")]
    public class SkillTemplateSO : ScriptableObject
    {
        [SerializeField] private string _displayName;
        [TextArea]
        [SerializeField] private string _description;

        public string DisplayName => _displayName;
        public string Description => _description;
    }
}