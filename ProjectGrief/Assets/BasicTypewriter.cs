using System.Collections;
using TMPro;
using UnityEngine;

public class BasicTypewriter : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    private string currentFullText;
    public bool isTyping;

    public static BasicTypewriter Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartTyping(string fullText)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentFullText = fullText;  // No formatting, just use the plain text
        typingCoroutine = StartCoroutine(TypeText(currentFullText));
    }

    private IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        textMesh.text = "";

        int i = 0;
        while (i < fullText.Length)
        {
            if (fullText[i] == '<')
            {
                // Handle HTML-like tags without formatting
                int tagEnd = fullText.IndexOf('>', i);
                if (tagEnd == -1) break;

                string tag = fullText.Substring(i, tagEnd - i + 1);
                textMesh.text += tag;
                i = tagEnd + 1;
            }
            else
            {
                textMesh.text += fullText[i];
                i++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
    }

    public void SkipTyping()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textMesh.text = currentFullText;
            isTyping = false;
        }
    }
}
