using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private AudioSource barkSource;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private float rotSpeed = 3.0f;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float runThreshold = 0.15f;
    [SerializeField] private float maxVelocity = 200f;

    private Animator animator;
    private Vector3 pos, velocity;

    private void Start()
    {
        animator = GetComponent<Animator>();

        pos = transform.position;
    }

    private void Update()
    {
        // Look at the Player
        Quaternion newRotation = Quaternion.LookRotation(objectToFollow.position - transform.position);
        newRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotSpeed * Time.deltaTime);

        velocity = (transform.position - pos) / Time.deltaTime;
        pos = transform.position;

        float normalisedVelocity = Mathf.Clamp01(velocity.sqrMagnitude / maxVelocity);

        animator.SetFloat("Speed", normalisedVelocity);

        // Move towards Player
        if (Vector3.Distance(transform.position, objectToFollow.position) > maxDistance)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    public void Bark()
    {
        barkSource.Play();
    }
}
