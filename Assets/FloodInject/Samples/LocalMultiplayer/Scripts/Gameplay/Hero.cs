using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    
    private PlayerController _playerController;
    private HeroTemplate _heroTemplate;
    
    public void Init(PlayerController playerController, HeroTemplate heroTemplate)
    {
        _playerController = playerController;
        _heroTemplate = heroTemplate;
        _spriteRenderer.color = _heroTemplate.Color;
    }
}