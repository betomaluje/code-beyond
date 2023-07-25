using System.Collections;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class FreeLookCameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float shakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float shakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    // Cinemachine Shake
    private CinemachineFreeLook freeLookCamera;

    // Use this for initialization
    private void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    public void ShakeIt()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Noise(shakeAmplitude, shakeFrequency);
        yield return new WaitForSeconds(shakeDuration);
        Noise(0, 0);
    }

    private void Noise(float amplitude, float frequency)
    {
        int numberOfRigs = 3;
        for (int i = 0; i < numberOfRigs; i++)
        {
            CinemachineBasicMultiChannelPerlin multiChannelPerlin = freeLookCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            multiChannelPerlin.m_AmplitudeGain = amplitude;
            multiChannelPerlin.m_FrequencyGain = frequency;
        }
    }
}
