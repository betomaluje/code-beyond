using Beto.Sounds;
using UnityEngine;

public class NewPlayerAnimations : MonoBehaviour
{
    // checks if the player is currently attacking
    public bool isAttacking { get; private set; }

    private Animator animator;

    private readonly int IDLE = Animator.StringToHash("Happy Idle");
    private readonly int JUMPING = Animator.StringToHash("Jumping");
    private readonly int WALK = Animator.StringToHash("Walking");
    private readonly int RUN = Animator.StringToHash("Running");
    private readonly int VICTORY = Animator.StringToHash("Victory");
    private readonly int BLOCK = Animator.StringToHash("Block");

    private int currentState;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Idle()
    {
        ChangeAnimationState(IDLE);
    }

    public void Block()
    {
        isAttacking = false;
        animator.SetTrigger(BLOCK);
    }

    public void Jump()
    {
        ChangeAnimationState(JUMPING);
    }

    public void Move()
    {
        ChangeAnimationState(RUN);
    }

    public void ResetAnimations()
    {
        ChangeAnimationState(IDLE);
        isAttacking = false;
    }

    public void Attack(int index)
    {
        string attackTrigger = "Attack" + index;
        animator.SetTrigger(attackTrigger);
    }

    public void AttackWithWeapon(int index)
    {
        string attackTrigger = "WeaponAttack" + index;
        animator.SetTrigger(attackTrigger);
    }

    public void Victory()
    {
        ChangeAnimationState(VICTORY);
        SoundManager.instance.Play("PlayerVictory");
    }

    private void ChangeAnimationState(int newState)
    {
        // stop the same animation from interrupting itself
        if (currentState == newState) return;

        isAttacking = false;

        // play the animation
        animator.Play(newState);

        // reassign the current state
        currentState = newState;
    }
}
