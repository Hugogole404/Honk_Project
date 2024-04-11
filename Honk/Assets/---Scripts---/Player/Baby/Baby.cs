using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    private HoldBaby _holdBaby;

    private void Awake()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
    }
}