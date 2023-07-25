using Beto.Health;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private BaseHealth baseHealth;
    [SerializeField] private Image foregroundImage;
    [SerializeField] private float updateSpeedSeconds = 0.2f;

    private BaseHealth healthScript;

    private void Awake()
    {
        if (baseHealth == null)
        {
            PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            healthScript = playerHealth;
        }
        else
        {
            healthScript = baseHealth;

            if (healthScript is EnemyHealth health)
            {
                health.OnEnemyDead = (enemyObject) =>
                {
                    StartCoroutine(DestroyHealthBar());
                };
            }
        }
    }

    private void OnEnable()
    {
        healthScript.OnHealthChanged += HandleHealth;
    }

    private void OnDisable()
    {
        healthScript.OnHealthChanged -= HandleHealth;
    }

    private void HandleHealth(float healthPercentage)
    {
        StartCoroutine(ChangePercentage(healthPercentage));
    }

    private IEnumerator ChangePercentage(float healthPercentage)
    {
        float prePercentage = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(prePercentage, healthPercentage, elapsed / updateSpeedSeconds);
            yield return null;
        }

        foregroundImage.fillAmount = healthPercentage;
    }

    private IEnumerator DestroyHealthBar()
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.DOFade(0, 1).OnComplete(() => { Destroy(image); });
        }

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }
}