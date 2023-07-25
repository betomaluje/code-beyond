using DG.Tweening;
using UnityEngine;

public class BasicWanderScript : MonoBehaviour
{
    [SerializeField] private float moveTime = 20f;
    [SerializeField] private float rotationTime = 2f;
    [SerializeField] private float range = 10f;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float fixedYPos = 60f;

    private Vector3 wayPoint;

    void Start()
    {
        Wander();
    }

    void Update()
    {
        if ((transform.position - wayPoint).magnitude < minDistance)
        {
            // when the distance between us and the target is less than minDistance
            // create a new way point target
            Wander();
        }
    }

    private void Wander()
    {
        // does nothing except pick a new destination to go to

        float x = Random.Range(transform.position.x - range, transform.position.x + range);
        float y = Random.Range(fixedYPos - range, fixedYPos + range);
        float z = Random.Range(transform.position.z - range, transform.position.z + range);

        wayPoint = new Vector3(x, y, z);

        Quaternion lookOnLook = Quaternion.LookRotation(wayPoint - transform.position);

        transform.DOMove(wayPoint, moveTime);
        transform.DORotateQuaternion(lookOnLook, rotationTime);
    }
}
