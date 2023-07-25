using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyAttack : AttackManager
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] protected string[] comboParams;
    [SerializeField] protected string[] weaponComboParams;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float timeBetweenAttacks = 2f;

    private bool shouldAttack = false;
    private bool isAnimating = false;
    private bool playerWalkedAway = false;
    private Animator animator;

    private EnemyHealth enemyHealth;
    private EnemyFollow enemyFollowScript;

    protected override int GetAttackDamage()
    {
        return enemySO.attackPower;
    }

    protected override float GetFireRate()
    {
        return enemySO.fireRate;
    }

    protected override void Awake()
    {
        if (comboParams == null || comboParams.Length == 0)
        {
            comboParams = new string[] { "Attack1", "Attack2", "Attack3", "Attack4", "Attack5" };
        }

        animator = GetComponent<Animator>();

        base.Awake();

        attackAction = AttackNormally;
        nextAttackAction = NextAttackNormally;
    }

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyFollowScript = GetComponentInParent<EnemyFollow>();
    }

    protected override void Update()
    {
        if (enemyHealth.isDead)
        {
            return;
        }

        if (shouldAttack)
        {
            bool comboIndexInRange = comboIndex < comboParams.Length;

            // Left mouse click
            if (comboIndexInRange && !isAnimating)
            {
                StartCoroutine(DelayedAttack());
            }
        }
        else
        {
            if (!playerWalkedAway && !enemyHealth.isDead)
            {
                enemyFollowScript.shouldAgentBeEnabled = true;
                playerWalkedAway = true;
                Reset();
            }
        }
    }

    private IEnumerator DelayedAttack()
    {
        isAnimating = true;
        Attack();
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAnimating = false;
    }

    private void FixedUpdate()
    {
        shouldAttack = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position, enemySO.attackRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (!shouldAttack && LayerMaskUtils.LayerMatchesObject(targetLayer, nearbyObject.gameObject))
            {
                shouldAttack = true;
                playerWalkedAway = false;
                enemyFollowScript.shouldAgentBeEnabled = false;
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemySO.attackRadius);
    }

    protected override void AttackNormally()
    {
        //Debug.Log("attack normally " + comboIndex);
        animator.SetTrigger(comboParams[comboIndex]);
    }

    protected override void AttackUsingWeapon()
    {
        // do nothing
    }

    protected override void ResetCombo()
    {
        // do nothing
    }

    protected override void NextAttackNormally()
    {
        comboIndex = (comboIndex + 1) % comboParams.Length;
    }

    protected override void NextAttackUsingWeapon()
    {
        // do nothing
    }
}
