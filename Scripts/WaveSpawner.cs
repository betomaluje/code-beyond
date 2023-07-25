using System.Collections;
using Beto.Sounds;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private TextMeshProUGUI waveCountText;
    [SerializeField] private float timeBetweenWaves = 3.0f;
    [SerializeField] private int enemiesPerWave;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private GameObject spawnFX;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform triggerTransform;
    [SerializeField] private bool randomPosition = true;
    [SerializeField] private Transform spawnPosition;
    [Header("Victory Objects")]
    [SerializeField] private GameObject wallWhenReady;
    [SerializeField] private bool isWallHiddenAtFirst = false;
    [Space]
    [Header("Sound")]
    [SerializeField] private string songTheme = null;

    private float spawnRate = 1.0f;
    private int waveCount = 1;
    private bool waveIsDone = true, playerIsNear = false;
    private int totalEnemies = 0;
    private int deadEnemies = 0;

    private void OnEnable()
    {
        totalEnemies = enemiesPerWave;

        if (triggerTransform == null)
        {
            triggerTransform = transform;
        }

        if (isWallHiddenAtFirst)
        {
            HideWall();
        }
    }

    private void Update()
    {
        if (!playerIsNear)
        {
            Collider[] colliders = Physics.OverlapSphere(triggerTransform.position, spawnRadius, attackLayer);
            if (colliders != null && colliders.Length > 0)
            {
                playerIsNear = true;
                if (isWallHiddenAtFirst)
                {
                    ShowWall();
                }
                StartMusic();
            }
        }

        if (!playerIsNear)
        {
            return;
        }

        if (waveCountText != null)
        {
            waveCountText.text = "Wave: " + waveCount.ToString();
        }

        if (waveIsDone && totalEnemies <= maxEnemies)
        {
            StartCoroutine(waveSpawner());
        }
    }

    private void HideWall()
    {
        if (wallWhenReady != null)
        {
            wallWhenReady.SetActive(false);
        }
    }

    private void ShowWall()
    {
        if (wallWhenReady != null)
        {
            wallWhenReady.SetActive(true);
        }
    }

    private void StartMusic()
    {
        if (songTheme != null && songTheme.Length > 0)
        {
            SoundManager.instance.StopAllBackground();
            SoundManager.instance.Play(songTheme);
        }
    }

    private IEnumerator waveSpawner()
    {
        waveIsDone = false;

        // Debug.Log("Wave " + waveCount.ToString() + ". Spawning " + enemiesPerWave + " enemies!");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector3 newPosition;

            if (randomPosition || spawnPosition == null)
            {
                newPosition = Random.insideUnitCircle * spawnRadius;
                newPosition.x += transform.position.x;
                newPosition.y = transform.position.y; // we use this game objects y position (need to be setted on Editor)
                newPosition.z += transform.position.z;
            }
            else
            {
                newPosition = spawnPosition.position;
            }

            GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(randomEnemy, newPosition, Quaternion.identity);

            EnemyHealth enemyHealth = enemy.GetComponentInChildren<EnemyHealth>(true);
            if (enemyHealth != null)
            {
                enemyHealth.waveSpawner = this;
            }

            if (spawnFX != null)
            {
                Instantiate(spawnFX, newPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnRate);
        }

        spawnRate -= 0.1f;
        totalEnemies += enemiesPerWave;
        waveCount += 1;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (triggerTransform != null)
        {
            // Draw a red sphere at the transform's position
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(triggerTransform.position, spawnRadius);
        }
    }

    public void EnemyDead()
    {
        deadEnemies++;

        if (deadEnemies >= maxEnemies)
        {
            Debug.Log("All enemies dead!");
            if (wallWhenReady != null)
            {
                Destroy(wallWhenReady);
            }

            if (songTheme != null && songTheme.Length > 0)
            {
                SoundManager.instance.Stop(songTheme);
                SoundManager.instance.ResumeCurrentThemeSong();
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerAnimations playerAnimations = player.GetComponent<PlayerAnimations>();
            if (player != null && playerAnimations != null)
            {
                playerAnimations.Victory();
            }
        }
    }

}
