using UnityEngine;

public class SimpleAttackBehavior : BaseAttackBehavior
{
    private Transform _transform;
    
    [SerializeField] private float _speed;
    [SerializeField] private float _baseDamage;

    private void Start()
    {
        _transform = transform;
    }

    protected void FixedUpdate()
    {
        var direction = (target.position - _transform.position).normalized;
        _transform.Translate(_speed * Time.fixedDeltaTime * direction);
    }
}
