using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CombatManager : MonoBehaviour
{
    public static event Action OnBattleEnd;

    [Header("Player Timelines")]
    [SerializeField] private PlayableDirector playerDeathTimeline;
    [SerializeField] private PlayableDirector playerRespawnTimeline;

    [Header("PlayerAttack Timelines")]
    [SerializeField] private PlayableDirector playerMissTimeline;
    [SerializeField] private PlayableDirector playerHitTimeline;
    [SerializeField] private PlayableDirector playerAttackStartTimeline;
    [SerializeField] private PlayableDirector playerAttackFinishTimeline;

    [Header("Enemy Timelines")]
    [SerializeField] private PlayableDirector enemyDeathTimeline;
    [SerializeField] private PlayableDirector enemyAttackStartTimeline;
    [SerializeField] private PlayableDirector enemyAttackEndTimeline;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip missSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        Player.OnPlayerDied += HandlePlayerDied;
        Player.OnPlayerRespawned += HandlePlayerRespawn;
        Player.OnHealthChanged += HandlePlayerHealthChanged;

        PlayerAttack.OnMiss += HandlePlayerMiss;
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

    public void EndBattle()
    {
        OnBattleEnd?.Invoke();
    }

    private void HandlePlayerDied()
    {
        Debug.Log("Player died. Playing death timeline.");
        PlayTimeline(playerDeathTimeline);
        PlaySound(loseSound);
    }

    private void HandlePlayerRespawn()
    {
        Debug.Log("Player respawn");
        PlayTimeline(playerRespawnTimeline);
    }

    private void HandlePlayerHealthChanged(int current, int max)
    {
        Debug.Log($"Player health: {current}/{max}");

        if (current < max)
        {
            PlaySound(damageSound);
        }
    }

    private void HandlePlayerMiss()
    {
        PlayTimeline(playerMissTimeline);
        PlaySound(missSound);
    }

    private void HandlePlayerAttackStarted()
    {
        Debug.Log("Player attack started.");
        PlayTimeline(playerAttackStartTimeline);
    }

    private void HandlePlayerAttackFinished()
    {
        Debug.Log("Player attack finished.");
        PlayTimeline(playerAttackFinishTimeline);
        PlaySound(hitSound);
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        Debug.Log($"{enemy.name} died.");
        PlayTimeline(enemyDeathTimeline);
        PlaySound(winSound);
    }

    private void HandleEnemyHealthChanged(Enemy enemy, int current, int max)
    {
        Debug.Log($"{enemy.name} health: {current}/{max}");
    }

    private void HandleEnemyAttackStarted(Enemy enemy)
    {
        Debug.Log($"{enemy.name} attack started.");
        PlayTimeline(enemyAttackStartTimeline);
    }

    private void HandleEnemyAttackEnded(Enemy enemy)
    {
        Debug.Log($"{enemy.name} attack ended.");
        PlayTimeline(enemyAttackEndTimeline);
    }

    public void PlayTimeline(PlayableDirector tl)
    {
        if (tl == null)
        {
            //Debug.LogWarning("Timeline is null! Did you forget to assign it in the Inspector?");
            return;
        }

        tl?.Stop();
        tl?.Play();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
