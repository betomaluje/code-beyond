using UnityEngine;

public class TargetObjectReached : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private ObjectArrowPointer arrowPointer;

    void Start()
    {
        if (arrowPointer == null)
        {
            arrowPointer = FindObjectOfType<ObjectArrowPointer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMaskUtils.LayerMatchesObject(playerLayer, other.gameObject))
        {
            arrowPointer.GoToNextObject(other.transform);
            ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
                Destroy(other.gameObject, 4f);
            }

            // save player position
            SavePlayerCheckpoint(transform.position, other.gameObject);
        }
    }

    private void SavePlayerCheckpoint(Vector3 playerPosition, GameObject checkpoint)
    {
        CheckpointSO lastSavedCheckpoint = Resources.Load<CheckpointSO>("Checkpoint/LastSavedCheckpoint");
        SaveCheckpoint saveCheckpoint = checkpoint.GetComponentInParent<SaveCheckpoint>();

        if (lastSavedCheckpoint != null && saveCheckpoint != null)
        {
            lastSavedCheckpoint.SaveLastCheckpointPosition(playerPosition.x, playerPosition.y, playerPosition.z);
            lastSavedCheckpoint.SaveLastLevel(saveCheckpoint.currentLevel);

            Destroy(checkpoint);
        }
    }
}
