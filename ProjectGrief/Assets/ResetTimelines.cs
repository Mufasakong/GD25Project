using UnityEngine;
using UnityEngine.Playables;

public class ResetTimelines : MonoBehaviour
{
    public void ResetAllTimelinesExceptEquipped()
    {
        // Get the PlayableDirector on this GameObject
        PlayableDirector equippedDirector = GetComponent<PlayableDirector>();
        if (equippedDirector == null)
        {
            Debug.LogWarning("No PlayableDirector found on this GameObject.");
            return;
        }

        PlayableAsset equippedTimeline = equippedDirector.playableAsset;

        // Find all PlayableDirectors in the scene
        PlayableDirector[] allDirectors = FindObjectsOfType<PlayableDirector>();

        foreach (PlayableDirector director in allDirectors)
        {
            // Skip the one with the equipped timeline
            if (director.playableAsset == equippedTimeline) continue;

            // Stop and evaluate to reset
            director.Stop();
            //director.Evaluate();
        }

        Debug.Log("All timelines reset except the equipped one.");
    }
}
