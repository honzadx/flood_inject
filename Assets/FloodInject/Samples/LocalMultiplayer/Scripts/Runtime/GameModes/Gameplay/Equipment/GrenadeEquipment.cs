using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class GrenadeEquipment : IEquipment
    {
        private PlayerBehaviour _owner;
        private GrenadeTemplateSO _template;
        private float _cooldown;

        public GrenadeEquipment(GrenadeTemplateSO template, PlayerBehaviour owner)
        {
            _template = template;
            _owner = owner;
        }

        float IEquipment.Cooldown
        {
            get => _cooldown;
            set => _cooldown = value;
        }

        public void Update(float deltaTime)
        {
            _cooldown = Mathf.Max(_cooldown - deltaTime, 0.0f);
        }

        public void ActionStart()
        {
            _cooldown = _template.Cooldown;
            // TODO: (hdx) Throw the grenade in an arc 
        }

        public void ActionEnd()
        {
            // noop
        }
    }
}