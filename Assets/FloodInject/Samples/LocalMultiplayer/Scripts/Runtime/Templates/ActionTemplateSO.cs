using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/" + nameof(ActionTemplateSO))]
    public class ActionTemplateSO : ScriptableObject
    {
        [SerializeField] private string _displayName;
        [TextArea]
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ActionBehaviour _behaviour;
        
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public ActionBehaviour Behaviour => _behaviour;
    }
}