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

        private Vector3 lastPlayerPosition;
        private string lastSceneName;

        private List<string> capturedGhostNames = new List<string>();

        private void OnEnable()
        {
            BattleTrigger.OnBattleStart += HandleBattleStart;
            CombatManager.OnBattleEnd += HandleBattleEnd;
        }

        private void OnDisable()
        {
            BattleTrigger.OnBattleStart -= HandleBattleStart;
            CombatManager.OnBattleEnd -= HandleBattleEnd;
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

            Ghost[] ghosts = FindObjectsOfType<Ghost>(true);
            foreach (Ghost ghost in ghosts)
            {
                if (ghost.enemyPrefab != null && capturedGhostNames.Contains(ghost.enemyPrefab.enemyName))
                {
                    ghost.gameObject.SetActive(false);
                }
            }

            if (currentEnemy != null)
            {
                Enemy[] allEnemiesInScene = FindObjectsOfType<Enemy>(true);
                foreach (Enemy enemy in allEnemiesInScene)
                {
                    bool isMatch = enemy.enemyName == currentEnemy.enemyName;
                    enemy.gameObject.SetActive(isMatch);

                    if (isMatch)
                    {
                        Debug.Log("Activated enemy: " + enemy.name);
                    }
                }

            // Restore player position if we're back in the original scene
            if (scene.name == lastSceneName && Player.Instance != null)
                {
                    Player.Instance.transform.position = lastPlayerPosition;
                    Debug.Log("Restored player to previous position.");
                }



                currentEnemy = null; // Optional: clear this after handling
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

            lastSceneName = SceneManager.GetActiveScene().name;
            lastPlayerPosition = Player.Instance.transform.position;
        }

        private void HandleBattleEnd()
        {
            Debug.Log("Battle ended. Returning to: " + lastSceneName);
            SceneManager.LoadScene(lastSceneName);
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

        public void OnGhostCaptured(Enemy ghost)
        {
            ghostsRemaining--;
            capturedGhostNames.Add(ghost.enemyName);
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
