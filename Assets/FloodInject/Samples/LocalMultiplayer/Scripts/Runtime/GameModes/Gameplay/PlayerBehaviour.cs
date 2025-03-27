using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private Material _material;
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpVelocity;
        
        private int _headingDirection;
        private int _playerIndex;
        private float _horizontalMovement;
        private bool _isGrounded;
        private CharacterTemplateSO _characterTemplate;
        private PlayerInputRelay _playerInputRelay;
        private Transform _transform;
        private Material _materialInstance;
        private IEquipment _action1Equipment;
        private IEquipment _action2Equipment;

        public HealthComponent HealthComponent => _healthComponent;
        public CharacterTemplateSO CharacterTemplate => _characterTemplate;
        public int PlayerIndex => _playerIndex;
        public int HeadingDirection => _headingDirection;
        
        public void Init(int playerIndex, CharacterTemplateSO characterTemplate, PlayerInputRelay playerInputRelay)
        {
            _playerIndex = playerIndex;
            _characterTemplate = characterTemplate;
            _playerInputRelay = playerInputRelay;
            _transform = transform;

            _healthComponent.SetHealth(characterTemplate.Health);

            _action1Equipment = characterTemplate.Action1.Build(this);
            _action2Equipment = characterTemplate.Action2.Build(this);
            
            _materialInstance = new Material(_material);
            _materialInstance.SetColor(
                nameID: Statics.ToColorPropertyID, 
                value: ContextProvider<GameContext>.Ctx.Get<GameInitSO>().PlayerColors[_playerIndex]);
            _spriteRenderer.material = _materialInstance;
            
            StartListeningToPlayerInput();
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
            if (_isGrounded && movement.y > 0)
            {
                Jump();
            }
            
            _horizontalMovement = movement.x;
            if (_horizontalMovement != 0)
            {
                _spriteRenderer.flipX = _horizontalMovement < 0;
                _headingDirection = (int)Mathf.Sign(_horizontalMovement);
            }
        }

        private void OnAction1Event(bool pressedDown)
        {
            if (pressedDown)
            {
                _action1Equipment.ActionStart();
            }
            else
            {
                _action1Equipment.ActionEnd();
            }
        }

        private void OnAction2Event(bool pressedDown)
        {
            if (pressedDown)
            {
                _action2Equipment.ActionStart();
            }
            else
            {
                _action2Equipment.ActionEnd();
            }
        }

        private void Jump()
        {
            _rigidbody2D.linearVelocityY = _jumpVelocity;
        }

        protected void Update()
        {
            _action1Equipment.Update(Time.deltaTime);
            _action2Equipment.Update(Time.deltaTime);
        }

        protected void FixedUpdate()
        {
            _isGrounded = Physics2D.BoxCast(_transform.position, new Vector2(0.5f, 0.01f), 0.0f, Vector2.down, 0.1f,
                Constants.LEVEL_LAYER_MASK);
            
            _rigidbody2D.linearVelocityX = _horizontalMovement * _speed * Time.fixedDeltaTime;
        }
        
        protected void OnDestroy()
        {
            StopListeningToPlayerInput();
        }
    }
}