using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : AttackManager
{
    [SerializeField] private PlayerSO playerSO;

    [SerializeField] private KeyCode blockKey = KeyCode.E;

    [Header("Weapon Handling")]
    [SerializeField] private KeyCode changeWeaponKey = KeyCode.R;
    [SerializeField] private bool hasWeapon = true;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform weaponSFX;
    [SerializeField] private bool useSlowMotion = false;

    protected NewPlayerAnimations playerAnimations;
    
    protected override void Awake()
    {
        base.Awake();
        playerAnimations = GetComponent<NewPlayerAnimations>();
        ChangeHasWeaponTriggers();
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
            blockAction = BlockAction;
            nextAttackAction = NextAttackUsingWeapon;
        }
        else
        {
            attackAction = AttackNormally;
            blockAction = BlockAction;
            nextAttackAction = NextAttackNormally;
        }

        if (weaponHolder != null)
        {
            Instantiate(weaponSFX, weaponHolder.transform.position, Quaternion.identity);
            weaponHolder.gameObject.SetActive(hasWeapon);
        }
    }

    private void ToggleWeaponUsage()
    {
        hasWeapon = !hasWeapon;
    }

    protected override void Update()
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

        // Block actions
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

        // we reset the animation using the base Update
        base.Update();
    }

    protected override void Reset()
    {
        base.Reset();
        playerAnimations.ResetAnimations();
    }

    private void AddImpact()
    {
        // stop time
        if (useSlowMotion && Math.Abs(Time.timeScale - 0.2f) > .1f)
        {
            StartCoroutine(StartSlowmotion());
        }
        else if (!useSlowMotion && Math.Abs(Time.timeScale - 1) > .1f)
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

    protected override float GetFireRate()
    {
        return playerSO.fireRate;
    }

    protected override int GetAttackDamage()
    {
        return playerSO.attackPower;
    }

    protected override void AttackNormally()
    {
        playerAnimations.Attack(comboIndex);
    }

    protected override void AttackUsingWeapon()
    {
        playerAnimations.AttackWithWeapon(comboIndex);
    }

    private void BlockAction()
    {
        playerAnimations.Block();
        // we reset the combo index
        comboIndex = 0;
    }

    protected override void ResetCombo()
    {
        playerAnimations.Idle();
    }

    protected override void NextAttackNormally()
    {
        comboIndex = (comboIndex + 1) % maxAttackCombos;
    }

    protected override void NextAttackUsingWeapon()
    {
        comboIndex = (comboIndex + 1) % maxAttackWithWeaponCombos;
    }
}
