using UnityEngine;

public class FloatingBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask liquidLayer;
    [SerializeField] private float radius = 2;

    public float positionSpeed;
    public float positionAmount;

    public float rotationSpeed;
    public float rotationAmount;

    private bool isFloating;

    private void Update()
    {
        isFloating = Physics.CheckSphere(transform.position, radius, liquidLayer);
        if (isFloating)
        {
            float addToPos = (Mathf.Sin(Time.time * positionSpeed) * positionAmount);
            transform.position += Vector3.up * addToPos * Time.deltaTime;

            float xRot = (Mathf.Sin(Time.time * rotationSpeed) * rotationAmount);

            float zRot = (Mathf.Sin((Time.time - 1.0f) * rotationSpeed) * rotationAmount);

            transform.eulerAngles = new Vector3(xRot, transform.eulerAngles.y, zRot);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
