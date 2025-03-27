using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public abstract class BaseEquipmentTemplateSO : ScriptableObject
    {
        public abstract IEquipment Build(PlayerBehaviour owner);
    }
}