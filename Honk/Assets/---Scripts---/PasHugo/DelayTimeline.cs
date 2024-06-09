using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class DelayTimeline : MonoBehaviour
{
    public PlayableDirector timeline;
    // Start is called before the first frame update
    IEnumerator StartTimelineWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // Ajustez le délai si nécessaire
        timeline.Play();
    }

    void Start()
    {
        StartCoroutine(StartTimelineWithDelay());
    }
}
