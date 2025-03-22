using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private enum ControlState
        {
            Main = 0,
            Action = 1,
        }
        
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private Material _material;
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpVelocity;
        
        private int _playerIndex;
        private float _horizontalMovement;
        private bool _isGrounded;
        private CharacterTemplateSO _characterTemplate;
        private PlayerInputRelay _playerInputRelay;
        private Transform _transform;
        private Material _materialInstance;
        private ControlState _controlState;
        private ActionBehaviour _action1Behaviour;
        private ActionBehaviour _action2Behaviour;

        public HealthComponent HealthComponent => _healthComponent;
        public CharacterTemplateSO CharacterTemplate => _characterTemplate;
        public int PlayerIndex => _playerIndex;
        
        public void Init(int playerIndex, CharacterTemplateSO characterTemplate, PlayerInputRelay playerInputRelay)
        {
            _playerIndex = playerIndex;
            _characterTemplate = characterTemplate;
            _playerInputRelay = playerInputRelay;
            _transform = transform;

            _healthComponent.SetHealth(characterTemplate.Health);
            
            _materialInstance = new Material(_material);
            _materialInstance.SetColor(
                nameID: Statics.ToColorPropertyID, 
                value: ContextProvider<GameContext>.Ctx.Get<GameInitSO>().PlayerColors[_playerIndex]);
            _spriteRenderer.material = _materialInstance;
            
            _action1Behaviour = Instantiate(_characterTemplate.Action1.Behaviour, _transform);
            _action2Behaviour = Instantiate(_characterTemplate.Action2.Behaviour, _transform);
            _action1Behaviour.CompleteEvent += RestoreMainControl;
            _action2Behaviour.CompleteEvent += RestoreMainControl;
            StartListeningToPlayerInput();
        }

        private void RestoreMainControl()
        {
            _controlState = ControlState.Main;
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
            if (_controlState != ControlState.Main)
            {
                return;
            }

            if (_isGrounded && movement.y > 0)
            {
                Jump();
            }
            
            _horizontalMovement = movement.x;
            if (_horizontalMovement != 0)
            {
                _spriteRenderer.flipX = _horizontalMovement < 0;
            }
        }

        private void OnAction1Event()
        {
            _controlState = ControlState.Action;
            _action1Behaviour.Play(this);
        }

        private void OnAction2Event()
        {
            _controlState = ControlState.Action;
            _action2Behaviour.Play(this);
        }

        private void Jump()
        {
            _rigidbody2D.linearVelocityY = _jumpVelocity;
        }

        protected void FixedUpdate()
        {
            _isGrounded = Physics2D.BoxCast(_transform.position, new Vector2(0.5f, 0.01f), 0.0f, Vector2.down, 0.1f,
                Constants.LEVEL_LAYER_MASK);
            
            if (_controlState == ControlState.Main)
            {
                _rigidbody2D.linearVelocityX = _horizontalMovement * _speed * Time.fixedDeltaTime;
            }
        }
        
        protected void OnDestroy()
        {
            _action1Behaviour.CompleteEvent -= RestoreMainControl;
            _action2Behaviour.CompleteEvent -= RestoreMainControl;
            StopListeningToPlayerInput();
        }
    }
}