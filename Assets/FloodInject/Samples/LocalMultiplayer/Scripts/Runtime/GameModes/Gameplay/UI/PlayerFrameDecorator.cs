using FloodInject.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalMultiplayer.Runtime.UI
{
    public class PlayerFrameDecorator : MonoBehaviour
    {
        [SerializeField] private Image _characterPortrait;
        [SerializeField] private Transform _healthContainer;
        [SerializeField] private Material _material;
        [SerializeField] private TextMeshProUGUI _text;

        private HealthComponent _healthComponent;
        private int _playerIndex;
        
        public void Init(PlayerBehaviour player)
        {
            _playerIndex = player.PlayerIndex;
            _healthComponent = player.HealthComponent;
            _healthComponent.HealthChangedEvent += OnHealthChangedEvent;
            var playerColor = ContextProvider<GameContext>.Ctx
                .Get<GameInitSO>()
                .PlayerColors[_playerIndex];
            var materialInstance = new Material(_material);
            materialInstance.SetColor(Statics.ToColorPropertyID, playerColor);
            _characterPortrait.material = materialInstance;
            _characterPortrait.sprite = player.CharacterTemplate.Portrait;
            
            _text.SetText($"PLAYER {_playerIndex}");

            Refresh();
        }

        public void Refresh()
        {
            int healthIndex = 0;
            int health = _healthComponent.Health;
            foreach (Transform healthPiece in _healthContainer)
            {
                healthPiece.gameObject.SetActive(healthIndex++ < health);
            }
        }
        
        private void OnHealthChangedEvent(int currentHealth)
        {
            Refresh();
        }

        protected void OnDestroy()
        {
            if (_healthComponent == null) return;
            _healthComponent.HealthChangedEvent -= OnHealthChangedEvent;
        }
    }
}