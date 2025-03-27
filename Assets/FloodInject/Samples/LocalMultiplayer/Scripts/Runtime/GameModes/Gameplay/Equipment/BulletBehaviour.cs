using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class BulletBehaviour : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [SerializeField] private float _lifetime;

        private Team _team;
        private float _timeElapsed;
        private Transform _transform;

        public void Init(Team team)
        {
            _team = team;
            _transform = transform;
        }

        public void InvertDirection()
        {
            _transform.Rotate(Vector3.up, 180);
        }

        protected void FixedUpdate()
        {
            _timeElapsed += Time.fixedDeltaTime;
            _transform.position += _speed * Time.fixedDeltaTime * _transform.right;

            if (_timeElapsed > _lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}