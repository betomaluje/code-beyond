using Beto.Level;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private string[] instructions;
    [SerializeField] private TextMeshProUGUI instructionsText;
    [SerializeField] private Button actionButton;
    [SerializeField] private float panelY;
    [SerializeField] private float animDuartion = 0.5f;
    [SerializeField] private float initialDelay = 2f;
    [SerializeField] private Level nextLevel;

    private int currentInstruction = 0;
    private int maxInstructions;
    private float initialY;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        maxInstructions = instructions.Length;
        initialY = transform.position.y;

        actionButton.onClick.AddListener(NextInstruction);

        Invoke("NextInstruction", initialDelay);
    }

    private void NextInstruction()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        transform.DOMoveY(initialY, animDuartion).OnComplete(() =>
        {
            if (currentInstruction < maxInstructions)
            {
                transform.DOMoveY(panelY, animDuartion);
                instructionsText.text = instructions[currentInstruction];

                currentInstruction++;
            }
            else
            {
                // show end/continue button
                Debug.Log("show end/continue button");
                transform.DOMoveY(panelY, animDuartion);
                instructionsText.text = "";

                actionButton.onClick.RemoveListener(NextInstruction);
                actionButton.onClick.AddListener(OnClickFinish);
            }
        });
    }

    private void OnClickFinish()
    {
        gameManager.LoadScene(nextLevel);
    }
}
