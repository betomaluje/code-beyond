using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private float radius = 20f;
    [SerializeField] private float wonderingDistance = 5f;
    [SerializeField] private float playerDistanceThreshold = 2f;

    [HideInInspector]
    public bool shouldAgentBeEnabled = true;

    protected NavMeshAgent agent;

    private int maxNumberOfRetries = 10;
    private int currentNumberOfRetries = 1;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        agent.Warp(transform.position);

        WonderAround();
    }

    protected virtual void FixedUpdate()
    {
        if (agent == null || !shouldAgentBeEnabled || !agent.enabled)
        {
            return;
        }

        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            Quaternion rotation = Quaternion.LookRotation(agent.velocity.normalized);
            rotation.x = 0;
            transform.rotation = rotation;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, attackLayer);
        if (colliders != null && colliders.Length > 0)
        {
            Vector3 playerPosition = colliders[0].transform.root.position;

            float dist = Vector3.Distance(playerPosition, transform.position);

            // follow the player if it's not already or if the distance is bigger than the threshold (player moved)
            if (dist >= playerDistanceThreshold)
            {
                //Debug.Log("Destination PLAYER: " + playerPosition);
                agent.destination = playerPosition;
            }
        }
        else
        {
            // no player near, we need to wonder around
            if (!agent.hasPath || agent.remainingDistance <= 0.1f)
            {
                // here we need to make enemy wonder around
                WonderAround();
            }
        }
    }

    private Vector3 GetRandomPoint()
    {
        float rangeX = Random.Range(-wonderingDistance, wonderingDistance);
        float rangeZ = Random.Range(-wonderingDistance, wonderingDistance);
        Vector3 newPosition = transform.position;
        newPosition.x = newPosition.x + rangeX;
        newPosition.z = newPosition.z + rangeZ;

        return newPosition;
    }

    private void WonderAround()
    {
        // Set the agent to go to the currently selected destination.
        Vector3 randomPosition = GetRandomPoint();
        //Debug.Log("Destination RANDOM: " + randomPosition);
        if (agent != null)
        {
            if (agent.isOnNavMesh)
            {
                agent.destination = randomPosition;
            }
            else
            {
                if (currentNumberOfRetries <= maxNumberOfRetries)
                {
                    currentNumberOfRetries++;
                    WonderAround();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void DisableAgent()
    {
        agent.enabled = false;
    }
}
