using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private Team _team;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _health;

        public void SetHealth(int maxHealth, int health)
        {
            _maxHealth = maxHealth;
            _health = health;
        }

        public void SetHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;
        }

        public void SetTeam(Team team)
        {
            _team = team;
        }
    }
}