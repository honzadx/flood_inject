using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private static readonly int _toColorPropertyID = Shader.PropertyToID("_ToColor");
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _material;
        
        private int _playerIndex;
        private CharacterTemplateSO _characterTemplate;
        private PlayerInputRelay _playerInputRelay;
        private Transform _transform;
        private Material _materialInstance;

        private float _horizontalMovement;
        
        public void Init(int playerIndex, CharacterTemplateSO characterTemplate, PlayerInputRelay playerInputRelay)
        {
            _playerIndex = playerIndex;
            _characterTemplate = characterTemplate;
            _playerInputRelay = playerInputRelay;
            _transform = transform;
            
            _materialInstance = new Material(_material);
            _materialInstance.SetColor(
                nameID: _toColorPropertyID, 
                value: ContextProvider<GameContext>.Ctx.Get<GameConfigSO>().PlayerColors[_playerIndex]);
            _spriteRenderer.material = _materialInstance;
            
            StartListeningToPlayerInput();
        }

        protected void OnDestroy()
        {
            StopListeningToPlayerInput();
        }

        private void StartListeningToPlayerInput()
        {
            _playerInputRelay.MoveEvent += OnMoveEvent;
            _playerInputRelay.Action1Event += OnAction1Event;
            _playerInputRelay.Action2Event += OnAction2Event;
        }
        
        private void StopListeningToPlayerInput()
        {
            if (_playerInputRelay == null) return;
            
            _playerInputRelay.MoveEvent += OnMoveEvent;
            _playerInputRelay.Action1Event += OnAction1Event;
            _playerInputRelay.Action2Event += OnAction2Event;
        }

        private void OnMoveEvent(Vector2 movement)
        {
            _horizontalMovement = movement.x;
            if (_horizontalMovement != 0)
            {
                _spriteRenderer.flipX = _horizontalMovement < 0;
            }
        }
        
        private void OnAction1Event() { }

        private void OnAction2Event() { }

        protected void FixedUpdate()
        {
            _transform.position += _horizontalMovement * Time.fixedDeltaTime * _characterTemplate.Speed * Vector3.right;
        }
    }
}