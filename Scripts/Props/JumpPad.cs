using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float speed = 1000f;
    [SerializeField] private ParticleSystem particles;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * speed, ForceMode.Impulse);

            if (anim != null) 
            {
                anim.SetTrigger("Squash");
            }

            if (particles != null) 
            {
                particles.gameObject.SetActive(true);
                particles.Play();
            }
        }
    }
}
