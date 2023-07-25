using UnityEngine;
using UnityEngine.Playables;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector directorControl;

    public void PauseDirector()
    {
        directorControl.Pause();
    }

    public void ResumeDirector()
    {
        directorControl.Resume();
    }
}
