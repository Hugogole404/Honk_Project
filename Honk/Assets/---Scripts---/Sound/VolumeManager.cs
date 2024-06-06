using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public float ActualTimerMusic;

    [SerializeField] Slider _sliderVolume;
    [SerializeField] AudioSource _globalMusic;

    private void Start()
    {
        AudioListener.volume = 0.5f;
    }
    public void SetVolume()
    {
        PlayerPrefs.SetFloat("SoundSliderValue", _sliderVolume.value);
        AudioListener.volume = _sliderVolume.value;
    }


    public void GetTimerMusic()
    {
        ActualTimerMusic = _globalMusic.time;
        PlayerPrefs.SetFloat("TimerMusic", ActualTimerMusic);
    }
    public void SetTimerMusic()
    {
        ActualTimerMusic = PlayerPrefs.GetFloat("TimerMusic");
        _globalMusic.time = ActualTimerMusic;
    }
    public void ResetTimerMusic()
    {
        PlayerPrefs.SetFloat("TimerMusic", 0f);
    }
}