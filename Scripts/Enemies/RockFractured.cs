using UnityEngine;

public class RockFractured : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 6f;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
