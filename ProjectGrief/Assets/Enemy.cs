using UnityEngine;
using System;
using UnityEngine.Playables;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDied;
    public static event Action<Enemy, int, int> OnHealthChanged;
    public static event Action<Enemy> OnEnemyAttackStarted;
    public static event Action<Enemy> OnEnemyAttackEnded;

    [SerializeField] private PlayableDirector director; // Set in Inspector
    [SerializeField] private List<PlayableAsset> timelines;
    [SerializeField] private List<PlayableAsset> rageTimelines;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float rageMultiplier = 1.5f;
    private int currentHealth;

    public string enemyName;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        PlayerAttack.OnAttackFinished += HandleAttackFinished;
        director.stopped += OnTimelineFinished;
        Player.OnPlayerRespawned += HandlePlayerRespawn;
        Player.OnPlayerDied += HandlePlayerDead;
    }

    private void OnDisable()
    {
        PlayerAttack.OnAttackFinished -= HandleAttackFinished;
        director.stopped -= OnTimelineFinished;
        Player.OnPlayerRespawned -= HandlePlayerRespawn;
        Player.OnPlayerDied -= HandlePlayerDead;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(this, currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(this, currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{enemyName} died.");
        director.time = director.duration;
        OnEnemyDied?.Invoke(this);
    }

    public void Attack()
    {
        List<PlayableAsset> availableTimelines = timelines;

        float healthPercent = (float)currentHealth / maxHealth;
        if (healthPercent < 0.5f && rageTimelines != null && rageTimelines.Count > 0)
        {
            availableTimelines = rageTimelines;
        }

        if (availableTimelines == null || availableTimelines.Count == 0)
        {
            Debug.LogWarning("No timelines available for enemy attack.");
            return;
        }

        PlayableAsset randomTimeline = availableTimelines[UnityEngine.Random.Range(0, availableTimelines.Count)];
        director.playableAsset = randomTimeline;
        director.time = 0;
        director.Play();

        float speedMultiplier = healthPercent < 0.33f ? rageMultiplier : 1f;
        director.playableGraph.GetRootPlayable(0).SetSpeed(speedMultiplier);

        Debug.Log($"{gameObject.name} played timeline: {randomTimeline.name} at speed {speedMultiplier}x");
        OnEnemyAttackStarted?.Invoke(this);
    }


    private void OnTimelineFinished(PlayableDirector obj)
    {
        if (obj == director)
        {
            AttackEnded();
        }
    }

    private void HandleAttackFinished()
    {
        Attack();
    }

    private void AttackEnded()
    {
        Debug.Log($"{enemyName}'s timeline ended.");
        OnEnemyAttackEnded?.Invoke(this);
    }

    private void HandlePlayerDead()
    {
        director.time = director.duration;
        //director.Stop();
    }

    private void HandlePlayerRespawn()
    {
        Debug.Log($"{enemyName} was reset due to player respawn.");
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(this, currentHealth, maxHealth);
    }

}
