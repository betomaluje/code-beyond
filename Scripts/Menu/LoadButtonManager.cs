using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadButtonManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;

    void Start()
    {
        CheckpointSO lastSavedCheckpoint = Resources.Load<CheckpointSO>("Checkpoint/LastSavedCheckpoint");
        if (lastSavedCheckpoint != null)
        {
            descriptionText.text = "Continue at " + lastSavedCheckpoint.savedLevelName;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
