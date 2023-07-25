using Beto.Level;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private Level currentLevel;

    [SerializeField] private GameObject playerObject;

    [SerializeField] private GameObject[] levelStartingObjects;

    private Transform levelStartingPosition;

    void Start()
    {
        levelStartingPosition = playerObject.transform;

        LoadLastCheckpoint();
    }

    private void LoadLastCheckpoint()
    {
        CheckpointSO lastSavedCheckpoint = Resources.Load<CheckpointSO>("Checkpoint/LastSavedCheckpoint");
        if (lastSavedCheckpoint != null)
        {
            Vector3 playerPosition = lastSavedCheckpoint.LoadLastCheckpointPosition();

            // if the saved current position is not the level start position, we need to disable cutscenes or any other thing
            if ((int)currentLevel.levelNumber == lastSavedCheckpoint.savedLevelIndex && playerPosition != levelStartingPosition.position)
            {
                Debug.Log("Level has saved data!");

                playerObject.transform.position = playerPosition;

                // we toggle the game objects
                foreach (GameObject go in levelStartingObjects)
                {
                    go.SetActive(!go.activeInHierarchy);
                }
            }
            else
            {
                Debug.Log("Level is starting fresh!");
            }
        }
    }
}
