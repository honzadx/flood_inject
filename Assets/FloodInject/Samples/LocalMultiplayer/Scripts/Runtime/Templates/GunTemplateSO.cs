using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/" + nameof(GunTemplateSO))]
    public class GunTemplateSO : BaseEquipmentTemplateSO
    {
        [SerializeField] private string _displayName;
        [TextArea]
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _visual;
        [SerializeField] private float _fireRate;
        [SerializeField] private BulletBehaviour _bulletPrefab;
        [SerializeField] private Vector3 _bulletOffset;
        
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public Sprite Visual => _visual;
        public float Cooldown => 1.0f / _fireRate;
        public BulletBehaviour BulletPrefab => _bulletPrefab;
        public Vector3 BulletOffset => _bulletOffset;
        
        public override IEquipment Build(PlayerBehaviour owner)
        {
            return new GunEquipment(this, owner);
        }
    }
}