using System;
using System.Collections;
using System.Collections.Generic;
using Beto.Health;
using UnityEngine;

public class NewPlayerAttack : MonoBehaviour
{
    [SerializeField] protected bool canLoop = true;
    [Range(0f, 1f)]
    [SerializeField] private float attackFrontThreshold = 0.7f;
    [SerializeField] private int maxAttackCombos = 5;
    [SerializeField] private int maxAttackWithWeaponCombos = 4;
    [SerializeField] private KeyCode blockKey = KeyCode.E;

    [SerializeField] private PlayerSO playerSO;
    [Header("Weapon Handling")]
    [SerializeField] private KeyCode changeWeaponKey = KeyCode.R;
    [SerializeField] protected bool hasWeapon = true;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private GameObject weaponSFX;
    [SerializeField] private bool useSlowMotion = false;

    protected int comboIndex = 0;
    private float resetTimer;
    private bool hasReset = true;
    private NewPlayerAnimations playerAnimations;

    private float fireRate;
    private int attackDamage;

    protected Action attackAction = delegate { };
    protected Action blockAction = delegate { };
    protected Action nextAttackAction = delegate { };
    public Action OnTargetHit = delegate { };

    private Dictionary<GameObject, AttackModel> nearEnemies = new Dictionary<GameObject, AttackModel>();

    protected virtual void Awake()
    {
        playerAnimations = GetComponent<NewPlayerAnimations>();

        fireRate = GetFireRate();
        attackDamage = GetAttackDamage();

        ChangeHasWeaponTriggers();
    }

    protected virtual void Update()
    {
        bool comboIndexInRange;
        if (hasWeapon)
        {
            comboIndexInRange = comboIndex <= maxAttackWithWeaponCombos;
        }
        else
        {
            comboIndexInRange = comboIndex <= maxAttackCombos;
        }

        // Left mouse click
        if (Input.GetMouseButtonDown(0) && comboIndexInRange)
        {
            Attack();
        }

        // Right mouse click
        if (Input.GetKeyDown(blockKey))
        {
            Block();
        }
        else if (Input.GetKeyUp(blockKey))
        {
            Reset();
        }

        if (Input.GetKeyDown(changeWeaponKey))
        {
            ToggleWeaponUsage();
            ChangeHasWeaponTriggers();
        }

        // Reset combo if the user has not clicked quickly enough
        resetTimer += Time.deltaTime;

        if (!hasReset && resetTimer > fireRate)
        {
            Reset();
        }
    }

    protected float GetFireRate()
    {
        return playerSO.fireRate;
    }

    protected int GetAttackDamage()
    {
        return playerSO.attackPower;
    }

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
                Debug.Log("hit " + enemy.enemy);

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

        playerAnimations.Block();

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

    protected void Reset()
    {
        hasReset = true;

        comboIndex = 0;
    }

    // Attack handling without weapon

    protected void AttackNormally()
    {
        playerAnimations.Attack(comboIndex);
    }

    protected void NextAttackNormally()
    {
        comboIndex = (comboIndex + 1) % maxAttackCombos;
    }

    // Attack handling with weapon

    protected void AttackUsingWeapon()
    {
        playerAnimations.AttackWithWeapon(comboIndex);
    }

    protected void NextAttackUsingWeapon()
    {
        comboIndex = (comboIndex + 1) % maxAttackWithWeaponCombos;
    }

    private void OnEnable()
    {
        OnTargetHit += AddImpact;
    }

    private void OnDisable()
    {
        OnTargetHit -= AddImpact;
    }

    private void ChangeHasWeaponTriggers()
    {
        if (hasWeapon)
        {
            attackAction = AttackUsingWeapon;
            blockAction = Reset;
            nextAttackAction = NextAttackUsingWeapon;
        }
        else
        {
            attackAction = AttackNormally;
            blockAction = Reset;
            nextAttackAction = NextAttackNormally;
        }

        if (weaponHolder != null)
        {
            Instantiate(weaponSFX, weaponHolder.transform.position, Quaternion.identity);
            weaponHolder.SetActive(hasWeapon);
        }
    }

    private void ToggleWeaponUsage()
    {
        hasWeapon = !hasWeapon;
    }

    private void AddImpact()
    {
        // stop time
        if (useSlowMotion && Time.timeScale != 0.2)
        {
            StartCoroutine(StartSlowmotion());
        }
        else if (!useSlowMotion && Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
    }

    private IEnumerator StartSlowmotion()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(playerSO.attackCountDown);
        Time.timeScale = 1;
    }
}
