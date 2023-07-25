using Beto.Health;
using System;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    [SerializeField] private GameObject damageParticles;
    [SerializeField] private EnemySO enemySO;

    public Action<GameObject> OnEnemyDead = delegate { };

    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public WaveSpawner waveSpawner;

    private Animator anim;
    private EnemyFollow enemyFollowScript;
    private Rigidbody rb;
    private int maxHealth;
    private int currentHealth = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        enemyFollowScript = GetComponentInParent<EnemyFollow>();

        maxHealth = enemySO.health;
        currentHealth = enemySO.health;
    }

    public override void GiveHealth(int health)
    {
        // not implemented
    }

    public override void Damage(int damage, Vector3 hitPos)
    {
        Instantiate(damageParticles, hitPos, Quaternion.identity);

        if (!isDead)
        {
            currentHealth -= damage;

            AddImpact(damage, hitPos);

            float healthPercentage = (float)currentHealth / (float)maxHealth;
            OnCurrentChange(currentHealth, maxHealth);
            OnHealthChanged(healthPercentage);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void AddImpact(int damage, Vector3 hitPos)
    {
        rb.AddForce(hitPos.normalized * damage * 1000, ForceMode.Force);
    }

    public override void Die()
    {
        rb.isKinematic = true;
        enemyFollowScript.DisableAgent();
        isDead = true;
        anim.SetTrigger("Die");

        OnEnemyDead(gameObject);

        if (waveSpawner != null)
        {
            waveSpawner.EnemyDead();
        }
    }
}
