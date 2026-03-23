using System;
using Daadab;
using UnityEngine;
using UnityEngine.Assertions;

public class Health : MonoBehaviour, IDamageable
{
    public int Defense { get; set; }
    public float CurrentHealth { get; set; }
    [SerializeField] private float maxHealth = 3;
    public float MaxHealth { get; set; }

    public Action OnChangeHealth;
    public Action OnDie;

    private void Awake()
    {
        Assert.IsTrue(maxHealth > 0);

        MaxHealth = maxHealth;
        RestoreHealth();
    }

    public void TakeDamage()
    {
        CurrentHealth -= 1;

        if (CurrentHealth > 0)
        {
            OnChangeHealth?.Invoke();
            Debug.Log($"{name} change health to {CurrentHealth}");
        }
        else
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
}
