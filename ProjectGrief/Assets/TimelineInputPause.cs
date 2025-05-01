using UnityEngine;
using UnityEngine.Playables;

public class TimelineInputPause : MonoBehaviour
{
    public PlayableDirector director;
    private bool waitingForInput = false;

    void Update()
    {
        if (waitingForInput && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            ResumeTimeline();
        }
    }

    // Called by the signal
    public void PauseTimelineWithInput()
    {
        if (director != null)
        {
            director.Pause();
            waitingForInput = true;
        }
    }

    private void ResumeTimeline()
    {
        if (director != null)
        {
            director.Play();
            waitingForInput = false;
        }
    }
}
