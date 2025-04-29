using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    public string sceneToLoad;
    public string targetSpawnPoint;

    [Header("UI and Audio")]
    public AudioClip doorSound;
    public GameObject enterPrompt;

    private bool playerInZone = false;

    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.spawnPointName = targetSpawnPoint;

            if (doorSound != null)
            {
                AudioSource.PlayClipAtPoint(doorSound, transform.position);
            }

            if (enterPrompt != null)
                enterPrompt.SetActive(false);

            Invoke("LoadScene", 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            if (enterPrompt != null)
                enterPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            if (enterPrompt != null)
                enterPrompt.SetActive(false);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
