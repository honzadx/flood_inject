using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/InputController")]
    public class InputControllerSO : ScriptableObject
    {
        [SerializeField] private bool _isBound;
        [SerializeField] private int _index;
        
        public bool IsBound => _isBound;
        public int Index => _index;
    }
}