using UnityEngine;
using System.Collections.Generic;

public class DialogueHandler : MonoBehaviour
{
    private bool isDialogueActive;
    public bool playOnStart = false;

    [System.Serializable]
    public class DialogueBlock
    {
        public string text;
        public string color = "white";
    }

    public List<DialogueBlock> dialogueSequence = new List<DialogueBlock>();
    private int currentIndex = 0;

    void Start()
    {
        if (playOnStart)
        {
            if (dialogueSequence.Count == 0)
            {
                dialogueSequence.Add(new DialogueBlock { text = "Test line one", color = "cyan" });
                dialogueSequence.Add(new DialogueBlock { text = "Another line", color = "magenta" });
            }

            StartDialogue(dialogueSequence);
        }
        else
        {
            BasicTypewriter.Instance.textMesh.text = "";
        }
    }



    public void StartDialogue(List<DialogueBlock> newDialogue)
    {
        if (isDialogueActive) return;

        dialogueSequence = newDialogue;
        currentIndex = 0;
        isDialogueActive = true;
        ShowNextDialogue();
    }

    public void ShowNextDialogue()
    {
        if (BasicTypewriter.Instance == null || dialogueSequence.Count == 0) return;

        string currentDisplayed = BasicTypewriter.Instance.textMesh.text;
        string expectedDialogue = dialogueSequence[Mathf.Clamp(currentIndex, 0, dialogueSequence.Count - 1)].text;

        if (currentDisplayed != expectedDialogue)
        {
            BasicTypewriter.Instance.SkipTyping();
        }
        else
        {
            if (currentIndex < dialogueSequence.Count)
            {
                DialogueBlock block = dialogueSequence[currentIndex];
                string coloredText = $"<color={block.color}>{block.text}</color>";
                BasicTypewriter.Instance.StartTyping(coloredText);
                currentIndex++;
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogueSequence.Clear();
        currentIndex = 0;
    }

    public void SkipOrContinue()
    {
        if (BasicTypewriter.Instance.isTyping)
        {
            BasicTypewriter.Instance.SkipTyping();
        }
        else if (isDialogueActive)
        {
            ShowNextDialogue();
        }
    }
}
