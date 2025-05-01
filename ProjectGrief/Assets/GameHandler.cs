using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;
    public static event Action OnGhostsCaptured;
    public static event Action OnAllGhostsCaptured;

    private Enemy currentEnemy;
    public int ghostsRemaining = 5;

    private void OnEnable()
    {
        BattleTrigger.OnBattleStart += HandleBattleStart;
    }

    private void OnDisable()
    {
        BattleTrigger.OnBattleStart -= HandleBattleStart;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        if (currentEnemy != null)
        {
            Enemy[] allEnemiesInScene = FindObjectsOfType<Enemy>(true); // include inactive

            foreach (Enemy enemy in allEnemiesInScene)
            {
                bool isMatch = enemy.enemyName == currentEnemy.enemyName;
                enemy.gameObject.SetActive(isMatch);

                if (isMatch)
                {
                    Debug.Log("Activated enemy: " + enemy.name);
                }
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void HandleBattleStart(Enemy enemy)
    {
        currentEnemy = enemy;
        Debug.Log("Battle starting with: " + enemy.name);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        if (Instance == null)
        {
            GameObject prefab = Resources.Load<GameObject>("GameManager");
            if (prefab != null)
            {
                Instantiate(prefab);
            }
        }
    }

    public void OnGhostCaptured()
    {
        ghostsRemaining--;
        OnGhostsCaptured?.Invoke();
        Debug.Log("Ghost captured! Remaining: " + ghostsRemaining);

        if (ghostsRemaining <= 0)
        {
            HandleAllGhostsCaptured();
        }
    }

    private void HandleAllGhostsCaptured()
    {
        Debug.Log("All ghosts captured! The painting is restored.");
        OnAllGhostsCaptured?.Invoke();
    }
}
