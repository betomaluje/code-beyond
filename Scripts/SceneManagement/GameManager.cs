using Beto.Level;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Level defaultFirstLevel;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private GameObject[] objectsToHide;
    [Header("Progress Bar")]
    [SerializeField] private Image progressImage;
    [SerializeField] private float updateSpeedSeconds = 0.2f;
    [Space]
    [Header("Background")]
    [SerializeField] private Image backgroundHolder;
    [SerializeField] private Sprite[] backgrounds;
    [Space]
    [Header("Tips")]
    [SerializeField] private TextMeshProUGUI tipsText;
    [SerializeField] private CanvasGroup tipsGroup;
    [SerializeField] private string[] tips;

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private float totalSceneProgress;

    private int tipCount;
    private int previousLevelToLoad = -1;

    private CheckpointSO lastSavedCheckpoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        lastSavedCheckpoint = Resources.Load<CheckpointSO>("Checkpoint/LastSavedCheckpoint");
    }

    private void ToggleObjects(bool enable)
    {
        foreach (GameObject go in objectsToHide)
        {
            go.SetActive(enable);
        }
    }

    public void LoadScene(Level levelToLoad)
    {
        if (levelToLoad.loadSceneMode == LoadSceneMode.Single)
        {
            backgroundHolder.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
            loadingScreen.SetActive(true);
            StartCoroutine(GenerateTips());
        }

        if (levelToLoad.shouldUnloadPrevious && previousLevelToLoad != -1)
        {
            // we unload previous scene
            Debug.Log("Unloading Scene -> " + previousLevelToLoad);
            scenesLoading.Add(SceneManager.UnloadSceneAsync(previousLevelToLoad));
        }

        int sceneToLoad = (int)levelToLoad.levelNumber;
        Debug.Log("Loading Scene -> " + sceneToLoad);

        if (levelToLoad.shouldUnloadPrevious)
        {
            previousLevelToLoad = sceneToLoad;

            Debug.Log("Adding Scene as Previous -> " + sceneToLoad);
        }

        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneToLoad, levelToLoad.loadSceneMode));

        if (levelToLoad.loadSceneMode == LoadSceneMode.Single)
        {
            StartCoroutine(GetSceneLoadProgress());
        }
    }

    public void StartNewGame()
    {
        backgroundHolder.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
        loadingScreen.SetActive(true);
        StartCoroutine(GenerateTips());
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)defaultFirstLevel.levelNumber));

        previousLevelToLoad = (int)defaultFirstLevel.levelNumber;

        if (lastSavedCheckpoint != null)
        {
            lastSavedCheckpoint.SaveLastLevel(defaultFirstLevel);
            lastSavedCheckpoint.ResetToLevel1();
        }

        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadGame()
    {
        if (lastSavedCheckpoint != null)
        {
            int lastLevelIndex = lastSavedCheckpoint.savedLevelIndex;
            Debug.LogFormat("Loaded last level {0} ", lastLevelIndex);

            backgroundHolder.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
            loadingScreen.SetActive(true);
            StartCoroutine(GenerateTips());
            scenesLoading.Add(SceneManager.LoadSceneAsync(lastLevelIndex));

            previousLevelToLoad = lastLevelIndex;

            StartCoroutine(GetSceneLoadProgress());
        }
    }

    private IEnumerator GenerateTips()
    {
        tipCount = Random.Range(0, tips.Length);

        tipsText.text = tips[tipCount];

        while (loadingScreen.activeInHierarchy)
        {
            yield return new WaitForSeconds(3f);

            tipsGroup.DOFade(0, 0.5f);

            yield return new WaitForSeconds(0.5f);

            tipCount++;
            if (tipCount >= tips.Length)
            {
                tipCount = 0;
            }

            tipsText.text = tips[tipCount];
            tipsGroup.DOFade(1, 0.5f);
        }
    }

    public IEnumerator GetSceneLoadProgress()
    {
        foreach (var scene in scenesLoading)
        {
            while (!scene.isDone)
            {
                totalSceneProgress = 0;

                ToggleObjects(false);

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

                string textPercentage = System.Math.Round(totalSceneProgress, 1, System.MidpointRounding.AwayFromZero).ToString("0.0");
                loadingText.text = "Loading " + textPercentage + "% ...";

                StartCoroutine(ChangePercentage(totalSceneProgress));

                yield return null;
            }
        }

        loadingScreen.SetActive(false);
    }

    private IEnumerator ChangePercentage(float healthPercentage)
    {
        float prePercentage = progressImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            progressImage.fillAmount = Mathf.Lerp(prePercentage, healthPercentage, elapsed / updateSpeedSeconds);
            yield return null;
        }

        progressImage.fillAmount = healthPercentage;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
