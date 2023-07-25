using UnityEngine;

namespace Beto.Sounds
{
    public class SceneThemeSound : MonoBehaviour
    {
        [SerializeField] private string ThemeSound;

        void Start()
        {
            SoundManager.instance.StopAllBackground();
            SoundManager.instance.Play(ThemeSound);
        }
    }
}