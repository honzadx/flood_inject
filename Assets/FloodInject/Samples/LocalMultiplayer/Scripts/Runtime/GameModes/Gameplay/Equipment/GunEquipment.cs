using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class GunEquipment : IEquipment
    {
        private PlayerBehaviour _owner;
        private GunTemplateSO _template;
        private float _cooldown;
        private bool _triggerHeld;

        public GunEquipment(GunTemplateSO template, PlayerBehaviour owner)
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
            if (_cooldown > 0)
            {
                _cooldown = Mathf.Max(_cooldown - deltaTime, 0.0f);
                return;
            }
            if (_triggerHeld)
            {
                ShootBullet();
            }
        }

        public void ActionStart()
        {
            _triggerHeld = true;
        }

        public void ActionEnd()
        {
            _triggerHeld = false;
        }

        private void ShootBullet()
        {
            _cooldown = _template.Cooldown;
            var prefab = _template.BulletPrefab;
            var position = _owner.transform.position + new Vector3(_template.BulletOffset.x * _owner.HeadingDirection, _template.BulletOffset.y, 0);
            var bulletInstance = Object.Instantiate(prefab, position, Quaternion.identity);
            bulletInstance.Init(Team.Player);
            if (_owner.HeadingDirection < 0)
            {
                bulletInstance.InvertDirection();
            }
        }
    }
}