using Beto.Level;
using UnityEngine;

[CreateAssetMenu(fileName = "New Checkpoint", menuName = "Level/Level Checkpoint", order = 51)]
public class CheckpointSO : ScriptableObject
{
    public float playerX = 0.0f;
    public float playerY = 0.0f;
    public float playerZ = 0.0f;

    public int savedLevelIndex = (int)Level.LevelNumber.IntroScene;
    public string savedLevelName = Level.LevelNumber.IntroScene.ToString();

    public Vector3 LoadLastCheckpointPosition()
    {
        Vector3 playerPosition = Vector3.zero;
        playerPosition.x = playerX;
        playerPosition.y = playerY;
        playerPosition.z = playerZ;

        Debug.LogFormat("Loading last poistion {0}", playerPosition);

        return playerPosition;
    }

    public void SaveLastCheckpointPosition(float x, float y, float z)
    {
        playerX = x;
        playerY = y;
        playerZ = z;

        Debug.LogFormat("Saving last position ({0}, {1}, {2})", playerX, playerY, playerZ);
    }

    public void SaveLastLevel(Level level)
    {
        savedLevelIndex = (int)level.levelNumber;
        savedLevelName = level.levelName;

        Debug.LogFormat("Saving last level {0} -> {1}", savedLevelIndex, savedLevelName);
    }

    public void ResetToLevel1()
    {
        SaveLastCheckpointPosition(134, 8, 30);
    }
}
