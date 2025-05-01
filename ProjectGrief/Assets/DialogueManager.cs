using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueOption
    {
        public string choiceText;
        [TextArea] public string resultText;
    }

    [System.Serializable]
    public class DialogueNode
    {
        [TextArea] public string narrativeText;
        public Sprite sprite;
        public List<DialogueOption> options;
    }

    public TextMeshProUGUI dialogueText;
    public List<Button> choiceButtons;
    public Button continueButton;
    public List<DialogueNode> dialogueNodes;
    public float typingSpeed = 0.02f;

    public bool playAtStart = true;
    public bool deactivateAtEnd = false;

    public Image targetSpriteRenderer;

    private int currentNodeIndex = 0;
    private string fullText = "";
    private bool isTyping = false;

    private PlayerMovement playerMovement;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (GameHandler.Instance != null && GameHandler.Instance.HasDialoguePlayed(sceneName))
        {
            gameObject.SetActive(false); // Dialogue already played, skip this object
            return;
        }

        // Find and disable PlayerMovement
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }
        }

        if (playAtStart)
            DisplayNode(currentNodeIndex);
    }

    void DisplayNode(int index)
    {
        StopAllCoroutines();

        if (index >= dialogueNodes.Count)
        {
            Debug.LogWarning("Index out of bounds in dialogueNodes.");
            return;
        }

        DialogueNode node = dialogueNodes[index];
        fullText = node.narrativeText;

        // Update sprite
        if (targetSpriteRenderer != null)
        {
            if (node.sprite != null)
            {
                targetSpriteRenderer.sprite = node.sprite;
                targetSpriteRenderer.gameObject.SetActive(true);
            }
            else
            {
                targetSpriteRenderer.sprite = null;
                targetSpriteRenderer.gameObject.SetActive(false);
            }
        }

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        if (choiceButtons != null)
        {
            foreach (var button in choiceButtons)
            {
                if (button != null)
                {
                    button.gameObject.SetActive(false);
                    button.onClick.RemoveAllListeners();
                    button.interactable = false;
                }
            }
        }

        StartCoroutine(TypeText(fullText, () =>
        {
            if (node.options == null || node.options.Count == 0)
            {
                if (continueButton != null)
                {
                    continueButton.gameObject.SetActive(true);
                    continueButton.onClick.RemoveAllListeners();
                    continueButton.onClick.AddListener(AdvanceDialogue);
                }
            }
            else
            {
                for (int i = 0; i < node.options.Count; i++)
                {
                    if (i >= choiceButtons.Count || choiceButtons[i] == null)
                    {
                        Debug.LogWarning($"Choice button at index {i} is missing.");
                        continue;
                    }

                    int capturedIndex = i;
                    Button btn = choiceButtons[i];
                    DialogueOption option = node.options[i];

                    btn.gameObject.SetActive(true);
                    btn.interactable = true;

                    var textComponent = btn.GetComponentInChildren<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        textComponent.text = option.choiceText;
                    }
                    else
                    {
                        Debug.LogWarning($"No TextMeshProUGUI found on button {btn.name}");
                    }

                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => OnChoiceSelected(capturedIndex));
                }
            }
        }));
    }

    void OnChoiceSelected(int choiceIndex)
    {
        DialogueOption selectedOption = dialogueNodes[currentNodeIndex].options[choiceIndex];
        fullText = selectedOption.resultText;

        foreach (var button in choiceButtons)
        {
            button.interactable = false;
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }

        StartCoroutine(DisplayResultWithContinue(fullText));
    }

    IEnumerator DisplayResultWithContinue(string resultText)
    {
        yield return TypeText(resultText, () =>
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() =>
            {
                continueButton.gameObject.SetActive(false);
                AdvanceDialogue();
            });
        });
    }

    IEnumerator TypeText(string text, System.Action onComplete)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        onComplete?.Invoke();
    }

    public void AdvanceDialogue()
    {
        if (isTyping) return;

        if (choiceButtons != null)
        {
            foreach (var button in choiceButtons)
            {
                button.gameObject.SetActive(false);
                button.onClick.RemoveAllListeners();
                button.interactable = false;
            }
        }

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        currentNodeIndex++;

        if (currentNodeIndex < dialogueNodes.Count)
            DisplayNode(currentNodeIndex);
        else
            EndScene();
    }

    void EndScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (GameHandler.Instance != null)
        {
            GameHandler.Instance.MarkDialoguePlayed(sceneName);
        }

        if (playerMovement != null)
        {
            playerMovement.ResetInputs();
            playerMovement.enabled = true;
        }

        if (deactivateAtEnd)
            gameObject.SetActive(false);

        dialogueText.text = "";

        if (choiceButtons != null)
        {
            foreach (var button in choiceButtons)
            {
                button.gameObject.SetActive(false);
                button.onClick.RemoveAllListeners();
                button.interactable = false;
            }
        }

        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("IntroScene");
            });
        }
    }
}
