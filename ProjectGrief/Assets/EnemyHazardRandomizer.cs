using UnityEngine;

public class EnemyHazardRandomizer : MonoBehaviour
{
    private void OnEnable()
    {
        RandomizeHazards();
    }

    private void RandomizeHazards()
    {
        EnemyHazard[] hazards = GetComponentsInChildren<EnemyHazard>(includeInactive: true);

        if (hazards.Length <= 1)
            return;

        int randomIndex = Random.Range(0, hazards.Length);

        for (int i = 0; i < hazards.Length; i++)
        {
            bool shouldEnable = (i != randomIndex);
            hazards[i].enabled = shouldEnable;

            Collider2D col = hazards[i].GetComponent<Collider2D>(); // Get Collider2D
            if (col != null)
            {
                col.enabled = shouldEnable; // Enable/Disable collider
            }

            SpriteRenderer sr = hazards[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = shouldEnable ? Color.white : new Color32(75, 0, 255, 255);
            }
        }

        Debug.Log($"EnemyHazardRandomizer: Disabled hazard -> {hazards[randomIndex].gameObject.name}");
    }

}
