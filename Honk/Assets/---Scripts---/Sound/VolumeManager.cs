using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] Slider _sliderVolume;
    private void Start()
    {
        AudioListener.volume = 0.5f;
    }   
    public void SetVolume()
    {
        AudioListener.volume = _sliderVolume.value; 
    }
}