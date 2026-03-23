using System;
using UnityEngine;

namespace Daadab
{
    public interface IDamageable
    {
        public uint CurrentHealth { get; set; }
        public uint MaxHealth { get; set; }
        public uint Defense { get; set; }
        public void Die();
        public void TakeDamage();
        public void AddHealth();
        public void RestoreHealth();
    }
}