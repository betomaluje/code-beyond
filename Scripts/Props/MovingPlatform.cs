using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float rotateSpeed = 0.5f;

    private int CurrentPoint = 0;

    private GameObject target = null;
    private Vector3 offset;

    void Start()
    {
        target = null;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
            offset = target.transform.position - transform.position;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }

    void FixedUpdate()
    {
        if (transform.position != waypoints[CurrentPoint].transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[CurrentPoint].transform.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, waypoints[CurrentPoint].transform.rotation, rotateSpeed * Time.deltaTime);
        }

        if (transform.position == waypoints[CurrentPoint].transform.position)
        {
            CurrentPoint += 1;
        }
        if (CurrentPoint >= waypoints.Length)
        {
            CurrentPoint = 0;
        }

        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }
    }
}
