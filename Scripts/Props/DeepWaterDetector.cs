using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DeepWaterDetector : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private Animator anim;
    private int playerInWater = Animator.StringToHash("PlayerInWater");
    private int playerOutWater = Animator.StringToHash("PlayerOutWater");

    private bool isPlaying = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlaying && LayerMaskUtils.LayerMatchesObject(playerLayer, other.gameObject))
        {
            isPlaying = true;
            anim.SetTrigger(playerInWater);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlaying && LayerMaskUtils.LayerMatchesObject(playerLayer, other.gameObject))
        {
            isPlaying = false;
            anim.SetTrigger(playerOutWater);
        }
    }

}
