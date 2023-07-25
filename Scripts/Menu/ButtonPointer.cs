using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPointer : MonoBehaviour
{
    [SerializeField] private float animDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI menuText;
    [SerializeField] private TextMeshProUGUI secondaryText;

    private Image backgroundImage;

    private bool isHovered = false;

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
        if (secondaryText != null)
        {
            secondaryText.transform.DOLocalMoveY(0f, 0);
        }
    }

    public void OnHoverOn()
    {
        if (!isHovered)
        {
            isHovered = true;
            backgroundImage.DOFade(1f, animDuration);
            menuText.DOColor(Color.black, animDuration);
            if (secondaryText != null)
            {
                secondaryText.DOFade(1, animDuration);
                secondaryText.transform.DOLocalMoveY(-70f, animDuration);
            }
            transform.DOScale(1.2f, animDuration);
        }
    }

    public void OnHoverOff()
    {
        if (isHovered)
        {
            isHovered = false;
            backgroundImage.DOFade(0f, animDuration);
            menuText.DOColor(Color.white, animDuration);
            if (secondaryText != null)
            {
                secondaryText.DOFade(0, animDuration);
                secondaryText.transform.DOLocalMoveY(-25f, animDuration);
            }
            transform.DOScale(1f, animDuration);
        }
    }
}
