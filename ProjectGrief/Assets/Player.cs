using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerDied;
    public static event Action OnPlayerRespawned;
    public static event Action<int, int> OnHealthChanged;
    public static event Action<int> OnPlayerHealed;
    public static Player Instance { get; private set; }

    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        currentHealth = maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died.");
        OnPlayerDied?.Invoke();
    }

    public void Heal(int amount)
    {
        int prevHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        int healedAmount = currentHealth - prevHealth;
        if (healedAmount > 0)
        {
            OnPlayerHealed?.Invoke(healedAmount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        Debug.Log("Player respawned.");
        OnPlayerRespawned?.Invoke();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
