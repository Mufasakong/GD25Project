using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GhostDialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(2, 5)] public string text;
        public Color textColor;
    }

    [Header("Dialogue Settings")]
    public string uniqueId = "GhostRoomIntro";
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
    public float typingSpeed = 0.02f;

    [Header("UI Reference")]
    public TextMeshProUGUI dialogueText;

    private int index = 0;
    private bool isTyping = false;

    void Start()
    {
        if (PlayerPrefs.GetInt(uniqueId, 0) == 1)
        {
            Destroy(gameObject);
            return;
        }

        PlayerPrefs.SetInt(uniqueId, 1);
        PlayerPrefs.Save();

        dialogueText.gameObject.SetActive(true);
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (dialogueText.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[index].text;
                isTyping = false;
            }
            else
            {
                index++;
                if (index < dialogueLines.Count)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        DialogueLine current = dialogueLines[index];

        dialogueText.color = current.textColor;
        dialogueText.text = "";

        foreach (char c in current.text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueText.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
