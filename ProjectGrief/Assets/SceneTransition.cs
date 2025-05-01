using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTransition : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    public string sceneToLoad;
    public string targetSpawnPoint;
    public int requiredGhostsCaptured = 0;

    [Header("UI and Audio")]
    public AudioClip doorSound;
    public GameObject enterPrompt;           // Text object like "Press E to Enter"
    public GameObject lockedMessageText;     // Text object like "The room is locked for now"

    private bool playerInZone = false;

    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            int ghostsCaptured = GameHandler.Instance != null ? 5 - GameHandler.Instance.ghostsRemaining : 0;

            if (ghostsCaptured >= requiredGhostsCaptured)
            {
                GameManager.spawnPointName = targetSpawnPoint;

                if (doorSound != null)
                {
                    AudioSource.PlayClipAtPoint(doorSound, transform.position);
                }

                if (enterPrompt != null)
                    enterPrompt.SetActive(false);

                if (lockedMessageText != null)
                    lockedMessageText.SetActive(false);

                Invoke("LoadScene", 0.5f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInZone = true;

        int ghostsCaptured = GameHandler.Instance != null ? 5 - GameHandler.Instance.ghostsRemaining : 0;
        bool doorIsOpen = ghostsCaptured >= requiredGhostsCaptured;

        if (enterPrompt != null)
            enterPrompt.SetActive(doorIsOpen);

        if (lockedMessageText != null)
            lockedMessageText.SetActive(!doorIsOpen);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInZone = false;

        if (enterPrompt != null)
            enterPrompt.SetActive(false);

        if (lockedMessageText != null)
            lockedMessageText.SetActive(false);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
