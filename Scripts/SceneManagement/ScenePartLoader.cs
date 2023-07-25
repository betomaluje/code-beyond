using Beto.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CheckMethod
{
    Distance,
    Trigger
}
public class ScenePartLoader : MonoBehaviour
{
    [SerializeField] private LayerMask triggerMask;
    [SerializeField] private Transform player;
    [SerializeField] private CheckMethod checkMethod;
    [SerializeField] private float loadRange;
    [SerializeField] private Level sceneToLoad;

    //Scene state
    private bool isLoaded;
    private bool shouldLoad;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        //verify if the scene is already open to avoid opening a scene twice
        if (SceneManager.sceneCount > 0)
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex == (int)sceneToLoad.levelNumber)
                {
                    isLoaded = true;
                }
            }
        }
    }

    private void Update()
    {
        //Checking which method to use
        if (checkMethod == CheckMethod.Distance)
        {
            DistanceCheck();
        }
        else if (checkMethod == CheckMethod.Trigger)
        {
            TriggerCheck();
        }
    }

    private void DistanceCheck()
    {
        //Checking if the player is within the range
        if (Vector3.Distance(player.position, transform.position) < loadRange)
        {
            LoadScene();
        }
        else
        {
            UnLoadScene();
        }
    }

    private void LoadScene()
    {
        if (!isLoaded)
        {
            //Loading the scene
            gameManager.LoadScene(sceneToLoad);
            //We set it to true to avoid loading the scene twice
            isLoaded = true;
        }
    }

    private void UnLoadScene()
    {
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync((int)sceneToLoad.levelNumber);
            isLoaded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMaskUtils.LayerMatchesObject(triggerMask, other.gameObject))
        {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerMaskUtils.LayerMatchesObject(triggerMask, other.gameObject))
        {
            shouldLoad = false;
        }
    }

    private void TriggerCheck()
    {
        //shouldLoad is set from the Trigger methods
        if (shouldLoad)
        {
            LoadScene();
        }
        else
        {
            UnLoadScene();
        }
    }
}