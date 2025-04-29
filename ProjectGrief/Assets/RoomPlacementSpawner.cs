using UnityEngine;

public class RoomPlacementSpawner : MonoBehaviour
{
    void Start()
    {
        string spawnPointName = GameManager.spawnPointName;

        if (!string.IsNullOrEmpty(spawnPointName))
        {
            GameObject spawnPoint = GameObject.Find(spawnPointName);
            if (spawnPoint != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = spawnPoint.transform.position;
                }
            }
            else
            {
                Debug.LogWarning("Spawn point not found: " + spawnPointName);
            }
        }
    }
}
