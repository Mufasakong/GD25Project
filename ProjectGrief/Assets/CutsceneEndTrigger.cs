using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneEndTrigger : MonoBehaviour
{
    public PlayableDirector director;
    public string nextSceneName;

    void Start()
    {
        if (director != null)
            director.stopped += OnCutsceneEnded;
    }

    void OnCutsceneEnded(PlayableDirector pd)
    {
        if (pd == director)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
