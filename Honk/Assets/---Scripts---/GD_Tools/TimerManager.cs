using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public void IncreaseTimer(float timer)
    {
        timer += Time.deltaTime;
    }
    public void ResetTimer(float timer)
    {
        timer = 0;
    }
}