using System.Collections;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private float force = 30;
    [SerializeField] private int damage = 5;
    [SerializeField] private float timeToDestroy = 6f;
    [SerializeField] private GameObject fracturedPrefab;
    [SerializeField] private float breakForce = 6f;
    [SerializeField] private float enoughEnergy = 10f;

    private void Start()
    {
        StartCoroutine(ExplodeInTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMaskUtils.LayerMatchesObject(attackLayer, collision.gameObject))
        {
            float energy = KineticEnergy(GetComponent<Rigidbody>());
            Debug.Log("Energy is " + energy);

            ImpactReceiver impactReceiver = collision.gameObject.GetComponent<ImpactReceiver>();
            if (impactReceiver != null)
            {
                Vector3 dir = collision.transform.position - transform.position;
                impactReceiver.AddImpact(dir, force);
            }

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Damage(damage, collision.transform.position);
            }

            if (energy >= enoughEnergy)
            {
                Explode();
            }
        }
    }

    private float KineticEnergy(Rigidbody rb)
    {
        // mass in kg, velocity in meters per second, result is joules
        if (rb == null)
        {
            return enoughEnergy;
        }

        return 0.5f * rb.mass * rb.velocity.sqrMagnitude;
    }

    private IEnumerator ExplodeInTime()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Explode();
    }

    public void Explode()
    {
        Destroy(gameObject);
        GameObject frac = Instantiate(fracturedPrefab, transform.position, transform.rotation);
        foreach (Rigidbody rb in frac.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
            rb.AddForce(force);
        }
    }
}
