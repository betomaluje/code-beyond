using System.Collections.Generic;
using UnityEngine;

public class ObjectArrowPointer : MonoBehaviour
{
    [SerializeField] private Transform[] objectsToFollow;
    [SerializeField] private Texture2D tex;    // Texture to be rotated
    [SerializeField] private Vector2 pivot;    // Where to place the center of the texture

    [Range(1, 4)]
    [SerializeField] private float imageScale = 1f;

    private Rect rect;
    private Camera cam;
    private int currentObject = 0;

    private List<Transform> alreadyDoneObjectives;

    void Awake()
    {
        cam = Camera.main;

        float targetWidth = tex.width * imageScale;
        float targetHeight = tex.height * imageScale;

        rect = new Rect(pivot.x - targetWidth / 2f, pivot.y - targetHeight / 2f, targetWidth, targetHeight);
        alreadyDoneObjectives = new List<Transform>();
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.Repaint && currentObject < objectsToFollow.Length)
        {
            Vector2 dir = cam.WorldToScreenPoint(objectsToFollow[currentObject].position);
            dir.y = Screen.height - dir.y;
            dir = dir - pivot;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Matrix4x4 matrixBackup = GUI.matrix;
            GUIUtility.RotateAroundPivot(angle, pivot);
            GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit);
            GUI.matrix = matrixBackup;
        }
    }

    public void GoToNextObject(Transform objectTransform)
    {
        // we check if we have already touched this objective
        if (objectTransform.gameObject != null && !alreadyDoneObjectives.Contains(objectTransform))
        {
            Debug.Log("Adding " + objectTransform.gameObject + " added to list");

            alreadyDoneObjectives.Add(objectTransform);

            if (currentObject < objectsToFollow.Length)
            {
                currentObject++;
                Debug.Log("Current objective is " + currentObject);
            }
        }
    }
}
