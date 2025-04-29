using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI enemyHPText;
    [SerializeField] private Slider attackSlider;
    public static event Action<float> OnSliderValueUpdated;

    private void OnEnable()
    {
        Player.OnHealthChanged += UpdatePlayerHealth;
        Player.OnPlayerDied += ShowPlayerDied;
        Enemy.OnHealthChanged += UpdateEnemyHealth;
    }

    private void OnDisable()
    {
        Player.OnHealthChanged -= UpdatePlayerHealth;
        Player.OnPlayerDied -= ShowPlayerDied;
        Enemy.OnHealthChanged -= UpdateEnemyHealth;
    }

    // UPDATE PLAYER HP
    private void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
        playerHPText.text = $"HP: {currentHealth}/{maxHealth}";
    }

    // UPDATE ENEMY HP
    private void UpdateEnemyHealth(Enemy enemy, int currentHealth, int maxHealth)
    {
        enemyHPText.text = $"Enemy HP: {currentHealth}/{maxHealth}";
    }

    //PLAYER DIED SHOW IT
    private void ShowPlayerDied()
    {
        playerHPText.text = "YOU DIED";
    }

    public void OnSliderValueChanged(float _)
    {
        float current = attackSlider.value;
        //Debug.Log("Slider value is now: " + current);
        OnSliderValueUpdated?.Invoke(current);
    }
}
