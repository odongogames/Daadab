using System;
using Daadab;
using UnityEngine;
using UnityEngine.Assertions;

public class Health : MonoBehaviour, IDamageable
{
    public uint Defense { get; set; }
    public uint CurrentHealth { get; set; }
    [SerializeField] private uint maxHealth = 3;
    public uint MaxHealth { get; set; }

    public Action<uint> OnChangeHealth;
    public Action OnDie;

    private void Awake()
    {
        Assert.IsTrue(maxHealth > 0);

        MaxHealth = maxHealth;
        RestoreHealth();
    }

    public void TakeDamage()
    {
        if (CurrentHealth <= 0)
        {
            Debug.Log($"{name} is already dead.");
            return;
        }

        ChangeHealth(-1);

        if (CurrentHealth <= 0)
        {
            Die();
            Debug.Log($"{name} is dead.");
        }
    }

    public void Die()
    {
        OnDie?.Invoke();
    }

    public void RestoreHealth()
    {
        CurrentHealth = MaxHealth;
    }

    public void AddHealth()
    {
        if (CurrentHealth <= 0)
        {
            Debug.Log($"{name} is already dead.");
            return;
        }
        
        ChangeHealth(1);
    }

    private void ChangeHealth(int amount)
    {
        var previousHealth = CurrentHealth;

        CurrentHealth += (uint) amount;
        CurrentHealth = (uint) Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        // only announce if health has changed
        if (CurrentHealth != previousHealth)
        {
            OnChangeHealth?.Invoke(CurrentHealth);
            Debug.Log($"{name} change health to {CurrentHealth}");
        }

    }
}
