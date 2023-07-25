using UnityEngine;
using UnityEngine.SceneManagement;

namespace Beto.Level
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Level/Level Data", order = 51)]
    public class Level : ScriptableObject
    {
        [Tooltip("This Level Numbers should match with the Build Settings added Scenes")]
        public LevelNumber levelNumber;

        public string levelName;

        public LoadSceneMode loadSceneMode = LoadSceneMode.Additive;

        public bool shouldUnloadPrevious = true;

        /**
         * This Level Numbers should match with the Build Settings added Scenes
         */
        [System.Serializable]
        public enum LevelNumber
        {
            MainMenu = 0,
            IntroScene = 1,
            CutScene_Level_1 = 2,
            Level_1 = 3,
            Level_1_Cave = 4,
            Level_2 = 5
        }
    }
}