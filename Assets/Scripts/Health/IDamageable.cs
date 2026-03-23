using System;
using UnityEngine;

namespace Daadab
{
    public interface IDamageable
    {
        public float CurrentHealth { get; set; }
        public float MaxHealth { get; set; }
        public int Defense { get; set; }
        public void Die();
        public void TakeDamage();
        public void AddHealth();
        public void RestoreHealth();
    }
}