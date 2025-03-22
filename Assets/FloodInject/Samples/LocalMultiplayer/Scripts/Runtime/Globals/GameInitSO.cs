using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/GameConfig")]
    public class GameInitSO : ScriptableObject
    {
        [SerializeField] private Color[] _playerColors;
        
        public Color[] PlayerColors => _playerColors; 
    }
}
