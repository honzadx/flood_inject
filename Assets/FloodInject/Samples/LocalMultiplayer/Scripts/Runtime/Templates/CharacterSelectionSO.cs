using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/" + nameof(CharacterSelectionSO))]
    public class CharacterSelectionSO : ScriptableObject
    {
        [SerializeField] private CharacterTemplateSO[] _characters;
        
        public CharacterTemplateSO[] Characters => _characters;
    }
}