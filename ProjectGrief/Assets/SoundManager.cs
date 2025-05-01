using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip mainMusic;
    public AudioClip bossMusic;

    [Header("Scene Configuration")]
    public string bossSceneName = "BattleScene";
    private AudioClip currentClip = null;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        string initialScene = SceneManager.GetActiveScene().name;
        HandleMusicForScene(initialScene);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleMusicForScene(scene.name);
    }

    void HandleMusicForScene(string sceneName)
    {
        if (sceneName == bossSceneName)
        {
            if (currentClip != bossMusic)
            {
                currentClip = bossMusic;
                audioSource.clip = bossMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (currentClip != mainMusic)
            {
                currentClip = mainMusic;
                audioSource.clip = mainMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }
}
