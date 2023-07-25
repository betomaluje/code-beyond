using Beto.Sounds;
using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private float fixedTransitionDuration = 0.3f;
    [SerializeField] private float animationEndThreshold = 1f;

    // checks if the player is currently attacking
    public bool isAttacking { get; private set; }

    private Animator animator;

    readonly int IDLE = Animator.StringToHash("Happy Idle");
    readonly int JUMPING = Animator.StringToHash("Jumping");
    readonly int WALK = Animator.StringToHash("Walking");
    readonly int RUN = Animator.StringToHash("Running");
    readonly int VICTORY = Animator.StringToHash("Victory");
    readonly int BLOCK = Animator.StringToHash("Center Block");

    // melee attacks
    readonly int PUNCH_1 = Animator.StringToHash("Punching");
    readonly int PUNCH_2 = Animator.StringToHash("Elbow Punching");
    readonly int PUNCH_3 = Animator.StringToHash("Hook");
    readonly int PUNCH_4 = Animator.StringToHash("Roundhouse Kick");
    readonly int PUNCH_5 = Animator.StringToHash("Kicking");

    // melee w/ sword attacks

    readonly int SWORD_1 = Animator.StringToHash("Sword And Shield Slash");
    readonly int SWORD_2 = Animator.StringToHash("Stable Sword Inward Slash");
    readonly int SWORD_3 = Animator.StringToHash("Stable Sword Outward Slash");
    readonly int SWORD_4 = Animator.StringToHash("Sword And Shield Slash Rotate");

    private int currentState;

    private Coroutine lastResetCoroutine = null;
    private Coroutine lastAttackCoroutine = null;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Idle()
    {
        ChangeAnimationState(IDLE);
    }

    public void Block()
    {
        ChangeAnimationState(BLOCK);
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
        StartCoroutine(ResetAttackState(0));
    }

    public void Attack(int index)
    {
        switch (index)
        {
            case 0:
                ChangeAttackAnimationState(PUNCH_1);
                break;
            case 1:
                ChangeAttackAnimationState(PUNCH_2);
                break;
            case 2:
                ChangeAttackAnimationState(PUNCH_3);
                break;
            case 3:
                ChangeAttackAnimationState(PUNCH_4);
                break;
            case 4:
                ChangeAttackAnimationState(PUNCH_5);
                break;
            default:
                Debug.LogWarning("Animation index " + index + " Not implemented!");
                break;
        }
    }

    public void AttackWithWeapon(int index)
    {
        switch (index)
        {
            case 0:
                ChangeAttackAnimationState(SWORD_1);
                break;
            case 1:
                ChangeAttackAnimationState(SWORD_2);
                break;
            case 2:
                ChangeAttackAnimationState(SWORD_3);
                break;
            case 3:
                ChangeAttackAnimationState(SWORD_4);
                break;
            default:
                Debug.LogWarning("Animation index " + index + " Not implemented!");
                break;
        }
    }

    public void Victory()
    {
        ChangeAnimationState(VICTORY);
        SoundManager.instance.Play("PlayerVictory");
    }

    private void CancelPreviousAnimations()
    {
        // if we have already a reset coroutine we need to cancel it
        if (lastResetCoroutine != null)
        {
            StopCoroutine(lastResetCoroutine);
        }

        // if we are already in the combo, we need to cancel the reset
        if (lastAttackCoroutine != null)
        {
            StopCoroutine(lastAttackCoroutine);
        }
    }

    private void ChangeAnimationState(int newState)
    {
        // stop the same animation from interrupting itself
        if (currentState == newState) return;

        CancelPreviousAnimations();

        isAttacking = false;

        // play the animation
        animator.Play(newState);

        // reassign the current state
        currentState = newState;
    }

    private void ChangeAttackAnimationState(int newState)
    {
        // stop the same animation from interrupting itself
        if (currentState == newState) return;

        CancelPreviousAnimations();

        lastAttackCoroutine = StartCoroutine(PlayUntilFinished(newState));

        // reassign the current state
        currentState = newState;
    }

    private IEnumerator PlayUntilFinished(int newState)
    {
        isAttacking = true;

        // play the animation        
        animator.CrossFadeInFixedTime(newState, fixedTransitionDuration);

        //Wait until Up is done Playing the play down
        float counter = 0;
        float waitTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        //Now, Wait until the current state is done playing
        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        // we reset to go to IDLE state
        lastResetCoroutine = StartCoroutine(ResetAttackState(animationEndThreshold));
    }

    private IEnumerator ResetAttackState(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ChangeAnimationState(IDLE);
        isAttacking = false;
        lastResetCoroutine = null;
    }
}
