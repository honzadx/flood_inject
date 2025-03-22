using System;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action<int> HealthChangedEvent;
        
        [SerializeField] private Team _team;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _health;
        
        public int MaxHealth => _maxHealth;
        public int Health => _health;

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

        public void ModifyHealth(int amount)
        {
            _health = Mathf.Clamp(_health + amount, 0, _maxHealth);
            HealthChangedEvent?.Invoke(_health);
        }
    }
}