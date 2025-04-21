using UnityEngine;

public abstract class BaseAttackBehavior : MonoBehaviour
{
    protected Transform target;
    protected HeroBehavior owner;

    public virtual void Init(HeroBehavior inOwner, Transform inTarget)
    {
        owner = inOwner;
        target = inTarget;
    }
}