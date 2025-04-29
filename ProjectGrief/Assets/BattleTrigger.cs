using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTrigger : MonoBehaviour
{
    [Header("Battle Settings")]
    public string battleSceneName;
    public string targetSpawnPointAfterBattle;

    [Header("UI and Audio")]
    public GameObject battlePrompt;  // "Press E to start battle"
    public AudioClip battleStartSound;

    private bool playerInZone = false;

    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.spawnPointName = targetSpawnPointAfterBattle;

            if (battleStartSound != null)
                AudioSource.PlayClipAtPoint(battleStartSound, transform.position);

            if (battlePrompt != null)
                battlePrompt.SetActive(false);

            Invoke("StartBattle", 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            if (battlePrompt != null)
                battlePrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            if (battlePrompt != null)
                battlePrompt.SetActive(false);
        }
    }

    private void StartBattle()
    {
        SceneManager.LoadScene(battleSceneName);
    }
}
