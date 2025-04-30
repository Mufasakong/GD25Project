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
        public List<DialogueOption> options;
    }

    public TextMeshProUGUI dialogueText;
    public List<Button> choiceButtons;
    public Button continueButton;
    public List<DialogueNode> dialogueNodes;
    public float typingSpeed = 0.02f;

    private int currentNodeIndex = 0;
    private string fullText = "";
    private bool isTyping = false;

    void Start()
    {
        DisplayNode(currentNodeIndex);
    }

    void DisplayNode(int index)
    {
        StopAllCoroutines();
        DialogueNode node = dialogueNodes[index];
        fullText = node.narrativeText;

        continueButton.gameObject.SetActive(false);

        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }

        StartCoroutine(TypeText(fullText, () =>
        {
            if (node.options.Count == 0)
            {
                continueButton.gameObject.SetActive(true);
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(AdvanceDialogue);
            }
            else
            {
                for (int i = 0; i < node.options.Count; i++)
                {
                    int capturedIndex = i;
                    Button btn = choiceButtons[i];

                    btn.gameObject.SetActive(true);
                    btn.interactable = true;
                    btn.GetComponentInChildren<TextMeshProUGUI>().text = node.options[i].choiceText;

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

    void AdvanceDialogue()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }

        continueButton.gameObject.SetActive(false);

        currentNodeIndex++;

        if (currentNodeIndex < dialogueNodes.Count)
            DisplayNode(currentNodeIndex);
        else
            EndScene();
    }

    void EndScene()
    {
        dialogueText.text = "Eziekel looks to the horizon. One last visit to the manor...";

        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }

        continueButton.gameObject.SetActive(true);
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("EntryScene");
        });
    }
}
