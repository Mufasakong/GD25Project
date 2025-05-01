using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PaintingDisplay : MonoBehaviour
{
    [Header("Painting Variants by Ghosts Captured (0 to N)")]
    [SerializeField] private Sprite[] paintingStates;

    [Header("SpriteRenderer displaying the painting")]
    [SerializeField] private SpriteRenderer paintingRenderer;

    [Header("TextMeshPro to show remaining ghost count")]
    [SerializeField] private TextMeshProUGUI paintingText;

    [Header("Scenes where the painting should be destroyed")]
    [SerializeField] private string[] excludedScenes = { "EndScene", "Menu" };

    [Header("Scene where painting should be hidden")]
    [SerializeField] private string battleSceneName = "BattleScene";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (paintingText != null)
        {
            DontDestroyOnLoad(paintingText.gameObject);
        }
    }

    private void Start()
    {
        UpdateDisplay();
        UpdateVisibility(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void UpdateDisplay()
    {
        if (GameHandler.Instance == null)
        {
            Debug.LogWarning("GameHandler not found.");
            return;
        }

        int ghostsRemaining = GameHandler.Instance.ghostsRemaining;
        int ghostsCaptured = 5 - ghostsRemaining;
        ghostsCaptured = Mathf.Clamp(ghostsCaptured, 0, paintingStates.Length - 1);

        if (paintingRenderer != null && paintingStates.Length > ghostsCaptured)
        {
            paintingRenderer.sprite = paintingStates[ghostsCaptured];
        }

        if (paintingText != null)
        {
            paintingText.text = ghostsRemaining.ToString();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        foreach (string excluded in excludedScenes)
        {
            if (sceneName == excluded)
            {
                if (paintingText != null)
                {
                    Destroy(paintingText.gameObject);
                }
                Destroy(gameObject);
                return;
            }
        }

        UpdateDisplay();
        UpdateVisibility(sceneName);
    }

    private void UpdateVisibility(string sceneName)
    {
        bool visible = sceneName != battleSceneName;

        if (paintingRenderer != null)
            paintingRenderer.enabled = visible;

        if (paintingText != null)
            paintingText.gameObject.SetActive(visible);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
