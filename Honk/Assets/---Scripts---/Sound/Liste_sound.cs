using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liste_sound : MonoBehaviour
{
    public AudioClip[] jump;
    public AudioSource JumpSound;
    public AudioSource paw_snow;
    public AudioSource paw_snow_grotte;
    public AudioSource paw_grass;
    public AudioSource paw_grass_grotte;
    public AudioSource paw_grotte;
    public AudioSource depot_petit;
    public AudioSource depot_petit_grotte;

    public paw_grotte PawGrotteSoundRandom;
    public paw_grotte PawGrassSoundRandom;
    public paw_grotte PawSnowSoundRandom;
    public Tremblement Tremblement;

    public bool WalkInCave;
    public bool WalkInGrass;
    public bool WalkInSnow;
}