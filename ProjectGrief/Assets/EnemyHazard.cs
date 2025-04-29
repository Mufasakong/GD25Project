using UnityEngine;
using System.Collections.Generic;

public class EnemyHazard : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float damageInterval = 1.0f;
    [SerializeField] private bool destroyOnHit = false;
    [SerializeField] private bool useTickDamage = false;

    private ParticleSystem ps;
    private List<ParticleSystem.Particle> insideParticles = new List<ParticleSystem.Particle>();

    private float damageTimer = 0f;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!useTickDamage)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);

                if (destroyOnHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (useTickDamage)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                damageTimer -= Time.deltaTime;

                if (damageTimer <= 0f)
                {
                    player.TakeDamage(damageAmount);
                    damageTimer = damageInterval; // Reset timer
                }

                if (destroyOnHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (useTickDamage)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                damageTimer = 0f; // Reset timer when player leaves
            }
        }
    }

    private void OnParticleTrigger()
    {
        int enterCount = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, insideParticles);

        for (int i = 0; i < enterCount; i++)
        {
            ParticleSystem.Particle p = insideParticles[i];

            Collider2D[] hits = Physics2D.OverlapCircleAll(p.position, 0.1f);

            foreach (Collider2D hit in hits)
            {
                Player player = hit.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damageAmount);

                    if (destroyOnHit)
                    {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
        }

        // Re-apply the list if needed
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, insideParticles);
    }

}
