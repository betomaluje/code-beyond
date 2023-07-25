using Cinemachine;
using System.Collections;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField] private float restTime = 0.5f;
    [SerializeField] private float activateTime = 0.5f;
    [SerializeField] private CinemachineVirtualCamera enemyCamera;
    [SerializeField] private GameObject[] toActivate;

    private bool portalCameraShown = false;

    private WaitForSeconds waitForSeconds;

    private void Start()
    {
        waitForSeconds = new WaitForSeconds(restTime);
    }

    public void ShowPortal()
    {
        if (!portalCameraShown)
        {
            portalCameraShown = true;
            StartCoroutine(ActivateObjects());
            StartCoroutine(ChangeToPortalCamera());
        }
    }

    private IEnumerator ActivateObjects()
    {
        yield return new WaitForSeconds(activateTime);
        foreach (GameObject item in toActivate)
        {
            item.SetActive(true);
        }
    }

    private IEnumerator ChangeToPortalCamera()
    {
        enemyCamera.Priority = 11;
        yield return waitForSeconds;
        enemyCamera.Priority = 9;
    }
}
