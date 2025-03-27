using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [CreateAssetMenu(menuName = "LocalMultiplayer/" + nameof(GrenadeTemplateSO))]
    public class GrenadeTemplateSO : BaseEquipmentTemplateSO
    {
        [SerializeField] private string _displayName;
        [TextArea]
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _cooldown;
        [SerializeField] private Sprite _visual;
        [SerializeField] private float _radius;
        [SerializeField] private int _damage;
        
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public float Cooldown => _cooldown;
        public Sprite Visual => _visual;
        public float Radius => _radius;
        public int Damage => _damage;
        
        public override IEquipment Build(PlayerBehaviour owner)
        {
            return new GrenadeEquipment(this, owner);
        }
    }
}