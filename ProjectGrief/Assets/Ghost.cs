using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Enemy enemyPrefab;
    public string ghostName;  // Optional: hide from editing

    private void OnValidate()
    {
        if (enemyPrefab != null)
        {
            ghostName = enemyPrefab.enemyName;
        }
    }

    private void Start()
    {
        /*if (GameHandler.Instance.IsGhostCaptured(ghostName))
        {
            gameObject.SetActive(false);
        }*/
    }
}
