using Beto.Sounds;
using UnityEngine;

public class ZoneBackgroundMusic : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private string songTheme;
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMaskUtils.LayerMatchesObject(playerMask, other.gameObject))
        {
            StartMusic();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMaskUtils.LayerMatchesObject(playerMask, other.gameObject))
        {
            ResumeMusic();
        }
    }

    private void StartMusic()
    {
        SoundManager.instance.StopAllBackground();
        if (audioSource == null)
        {
            SoundManager.instance.Play(songTheme);
        }
        else
        {
            audioSource.Play();
        }

    }

    private void ResumeMusic()
    {
        if (audioSource == null)
        {
            SoundManager.instance.Stop(songTheme);

        }
        else
        {
            audioSource.Stop();
        }

        SoundManager.instance.ResumeCurrentThemeSong();
    }
}
