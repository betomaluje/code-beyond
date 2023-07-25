using Beto.Health;
using System;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    [SerializeField] private PlayerSO playerSO;

    private int maxHealth;
    private int currentHealth;
    private FreeLookCameraShake cameraShake;

    private void Start()
    {
        cameraShake = GetComponentInChildren<FreeLookCameraShake>();

        maxHealth = playerSO.health;
        currentHealth = maxHealth;
    }

    private void HealthChanged(int newHealth)
    {
        bool wasPlayerDamaged = newHealth < currentHealth;
        currentHealth = newHealth;

        float healthPercentage = (float)currentHealth / (float)maxHealth;
        Debug.Log("Health %: " + healthPercentage);
        OnCurrentChange(currentHealth, maxHealth);
        OnHealthChanged(healthPercentage);

        if (wasPlayerDamaged)
        {
            cameraShake.ShakeIt();

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public override void GiveHealth(int health)
    {
        int newHealth = currentHealth + health;

        if (newHealth > maxHealth)
        {
            newHealth = maxHealth;
        }

        HealthChanged(newHealth);
    }

    public override void Damage(int damage, Vector3 hitPos)
    {
        OnDamagePerformed(damage);

        int newHealth = currentHealth - damage;

        // if hp is lower than 0, set it to 0.
        if (newHealth < 0)
        {
            newHealth = 0;
        }

        HealthChanged(newHealth);
    }

    public override void Die()
    {
        Debug.Log("Player is dead!");
    }
}
