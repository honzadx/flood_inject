using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] SpriteRenderer _portraitRenderer;
    [SerializeField] float _movementSpeed;
    
    private PlayerController _playerController;
    private HeroTemplate _heroTemplate;
    
    public void Init(PlayerController playerController, HeroTemplate heroTemplate)
    {
        _playerController = playerController;
        _heroTemplate = heroTemplate;
        _portraitRenderer.sprite = _heroTemplate.Portrait;
    }

    protected void FixedUpdate()
    {
        transform.Translate(_movementSpeed * Time.fixedDeltaTime * _playerController.MovementDirection);
    }
}