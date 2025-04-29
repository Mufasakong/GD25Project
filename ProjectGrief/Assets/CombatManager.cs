using UnityEngine;
using UnityEngine.Playables;

public class CombatManager : MonoBehaviour
{
    [Header("Player Timelines")]
    [SerializeField] private PlayableDirector playerDeathTimeline;

    [Header("PlayerAttack Timelines")]
    [SerializeField] private PlayableDirector playerMissTimeline;
    [SerializeField] private PlayableDirector playerHitTimeline;
    [SerializeField] private PlayableDirector playerAttackStartTimeline;
    [SerializeField] private PlayableDirector playerAttackFinishTimeline;

    [Header("Enemy Timelines")]
    [SerializeField] private PlayableDirector enemyDeathTimeline;
    [SerializeField] private PlayableDirector enemyAttackStartTimeline;
    [SerializeField] private PlayableDirector enemyAttackEndTimeline;

    private void OnEnable()
    {
        Player.OnPlayerDied += HandlePlayerDied;
        Player.OnHealthChanged += HandlePlayerHealthChanged;
        PlayerAttack.OnAttackStarted += HandlePlayerAttackStarted;
        PlayerAttack.OnAttackFinished += HandlePlayerAttackFinished;

        Enemy.OnEnemyDied += HandleEnemyDied;
        Enemy.OnHealthChanged += HandleEnemyHealthChanged;
        Enemy.OnEnemyAttackStarted += HandleEnemyAttackStarted;
        Enemy.OnEnemyAttackEnded += HandleEnemyAttackEnded;
    }

    private void OnDisable()
    {
        Player.OnPlayerDied -= HandlePlayerDied;
        Player.OnHealthChanged -= HandlePlayerHealthChanged;
        PlayerAttack.OnAttackStarted -= HandlePlayerAttackStarted;
        PlayerAttack.OnAttackFinished -= HandlePlayerAttackFinished;

        Enemy.OnEnemyDied -= HandleEnemyDied;
        Enemy.OnHealthChanged -= HandleEnemyHealthChanged;
        Enemy.OnEnemyAttackStarted -= HandleEnemyAttackStarted;
        Enemy.OnEnemyAttackEnded -= HandleEnemyAttackEnded;
    }

    private void HandlePlayerDied()
    {
        Debug.Log("Player died. Playing death timeline.");
        playerDeathTimeline?.Play();
    }

    private void HandlePlayerHealthChanged(int current, int max)
    {
        Debug.Log($"Player health: {current}/{max}");
        // Optional: Play a damage flash timeline or UI response
    }

    private void HandlePlayerAttackStarted()
    {
        Debug.Log("Player attack started.");
        playerAttackStartTimeline?.Play();
    }

    private void HandlePlayerAttackFinished()
    {
        Debug.Log("Player attack finished.");
        playerAttackFinishTimeline?.Play();
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        Debug.Log($"{enemy.name} died.");
        enemyDeathTimeline?.Play();
    }

    private void HandleEnemyHealthChanged(Enemy enemy, int current, int max)
    {
        Debug.Log($"{enemy.name} health: {current}/{max}");
    }

    private void HandleEnemyAttackStarted(Enemy enemy)
    {
        Debug.Log($"{enemy.name} attack started.");
        enemyAttackStartTimeline?.Play();
    }

    private void HandleEnemyAttackEnded(Enemy enemy)
    {
        Debug.Log($"{enemy.name} attack ended.");
        enemyAttackEndTimeline?.Play();
    }
}
