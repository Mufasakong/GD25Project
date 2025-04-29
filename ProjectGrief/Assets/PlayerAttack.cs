using UnityEngine;
using UnityEngine.Playables;
using System;

public class PlayerAttack : MonoBehaviour
{
    public static event Action OnAttackStarted;
    public static event Action OnAttackFinished;
    public static event Action OnHit;
    public static event Action OnMiss;

    [SerializeField] private PlayableDirector director; // Drag in inspector
    [SerializeField] private PlayableAsset attackTimeline;
    [SerializeField] private Animator attackAnimator;
    [SerializeField] private float centerThreshold = 0.05f;
    [SerializeField] private float maxThreshold = 0.3f;
    [SerializeField] private int maxDamage = 10;

    [SerializeField] private float sliderValue = 0.0f;
    [SerializeField] private Enemy enemy;

    private void Start()
    {
        if (director != null)
            director.stopped += OnTimelineStopped;
    }

    private void OnEnable()
    {
        UIManager.OnSliderValueUpdated += SetSliderValue;
    }

    private void OnDisable()
    {
        UIManager.OnSliderValueUpdated -= SetSliderValue;
    }

    private void SetSliderValue(float value)
    {
        sliderValue = value;
    }

    public void PerformAttack()
    {
        if (director == null || attackTimeline == null)
        {
            Debug.LogWarning("PlayableDirector or TimelineAsset not assigned.");
            return;
        }

        if (director.state == PlayState.Playing)
        {
            director.Pause();
            Debug.Log("Attack timeline paused.");

            if (attackAnimator != null)
                attackAnimator.Play("Attack");

            StartCoroutine(EvaluateAfterFrames(20));
        }
        else
        {
            director.playableAsset = attackTimeline;
            director.time = 0;
            director.Play();
            OnAttackStarted?.Invoke();
            Debug.Log("Attack timeline started.");
        }
    }

    public void EvaluateSlider()
    {
        float distanceFromCenter = Mathf.Abs(sliderValue - 0.5f);
        float multiplier = 0f;

        if (distanceFromCenter <= centerThreshold)
        {
            multiplier = 1f; // Perfect hit
        }
        else if (distanceFromCenter <= maxThreshold)
        {
            float range = maxThreshold - centerThreshold;
            float normalizedDistance = (distanceFromCenter - centerThreshold) / range;
            multiplier = Mathf.Lerp(1f, 0f, normalizedDistance); // Gradually fall off
        }

        int damage = Mathf.RoundToInt(maxDamage * multiplier);

        enemy = FindFirstObjectByType<Enemy>();
        if (enemy != null && damage > 0)
        {
            enemy.TakeDamage(damage);
            OnHit?.Invoke();
        }
        else
        {
            OnMiss?.Invoke();
        }


        Debug.Log($"Slider evaluated at {sliderValue}. Multiplier: {multiplier:F2}, Damage: {damage}");

        if (director != null && director.state != PlayState.Playing)
        {
            director.Stop();
        }
    }


    private System.Collections.IEnumerator EvaluateAfterFrames(int frameCount)
    {
        for (int i = 0; i < frameCount; i++)
            yield return null;

        EvaluateSlider();
    }


    private void OnTimelineStopped(PlayableDirector d)
    {

        Debug.Log("Attack timeline ended.");
        OnAttackFinished?.Invoke();
    }

    private void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnTimelineStopped;
    }
}
