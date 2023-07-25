using Beto.Health;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackManager : MonoBehaviour
{
    [SerializeField] protected bool canLoop = true;
    [Range(0f, 1f)]
    [SerializeField] private float attackFrontThreshold = 0.7f;
    [SerializeField] protected int maxAttackCombos = 5;
    [SerializeField] protected int maxAttackWithWeaponCombos = 4;

    protected int comboIndex = 0;
    private float resetTimer;
    private bool hasReset = true;

    private float fireRate;
    private int attackDamage;

    protected Action attackAction = delegate { };
    protected Action blockAction = delegate { };
    protected Action nextAttackAction = delegate { };
    public Action OnTargetHit = delegate { };

    private readonly Dictionary<GameObject, AttackModel> nearEnemies = new Dictionary<GameObject, AttackModel>();

    protected virtual void Awake()
    {
        fireRate = GetFireRate();
        attackDamage = GetAttackDamage();
    }

    protected virtual void Update()
    {
        // Reset combo if the user has not clicked quickly enough
        resetTimer += Time.deltaTime;

        if (!hasReset && resetTimer > fireRate)
        {
            Reset();
        }
    }

    protected abstract float GetFireRate();

    protected abstract int GetAttackDamage();

    public void AddEnemy(AttackModel enemy)
    {
        nearEnemies[enemy.enemy] = enemy;
        EnemyHealth enemyHealth = enemy.enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDead += RemoveDeadEnemy;
        }
    }

    public void RemoveEnemy(AttackModel enemy)
    {
        nearEnemies.Remove(enemy.enemy);
    }

    private void RemoveDeadEnemy(GameObject enemyObject)
    {
        nearEnemies.Remove(enemyObject);
        nearEnemies[enemyObject] = null;

        // we stop listening to this event
        EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDead -= RemoveDeadEnemy;
        }
    }

    /**
     * This method is called using an Animation Event on the Animation tab in the Editor
     * 
     * Got from https://answers.unity.com/questions/602044/melee-combat-collision-detection.html
     */
    public virtual void DamageEnemy()
    {
        Vector3 pos = transform.position;

        List<GameObject> keys = new List<GameObject>(nearEnemies.Keys);
        foreach (GameObject key in keys)
        {
            AttackModel enemy = nearEnemies[key];

            if (enemy == null || enemy.enemy == null)
            {
                continue;
            }

            Vector3 vec = enemy.enemy.transform.position;
            Vector3 direction = vec - pos;
            float dotProduct = Vector3.Dot(direction, transform.forward);
            if (dotProduct >= attackFrontThreshold)
            {
                OnTargetHit();

                BaseHealth baseHealth = enemy.enemy.GetComponent<BaseHealth>();

                if (baseHealth != null)
                {
                    baseHealth.Damage(attackDamage, direction);
                }
            }
        }
    }

    protected void Attack()
    {
        attackAction();

        CalculateNextAttack();
    }

    protected void Block()
    {
        blockAction();

        // we reset the combo index
        comboIndex = 0;
    }

    private void CalculateNextAttack()
    {
        hasReset = false;

        if (canLoop)
        {
            // If combo can loop
            nextAttackAction();
        }
        else
        {
            // If combo must not loop
            comboIndex++;
        }

        resetTimer = 0f;
    }

    protected virtual void Reset()
    {
        hasReset = true;
        comboIndex = 0;
    }

    protected abstract void ResetCombo();

    // Attack handling without weapon

    protected abstract void AttackNormally();

    protected abstract void NextAttackNormally();

    // Attack handling with weapon

    protected abstract void AttackUsingWeapon();

    protected abstract void NextAttackUsingWeapon();
}
