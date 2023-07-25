using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    [SerializeField] private Rigidbody buttonBody;
    [SerializeField] private Collider buttonCollider;

    public UnityEvent OnPushed = new UnityEvent();

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            buttonBody.useGravity = true;
        }
        else if (other.gameObject.CompareTag("PushButton"))
        {
            buttonBody.isKinematic = true;
            buttonCollider.isTrigger = true;

            OnPushed.Invoke();
        }
    }
}
