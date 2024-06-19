using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactiveCursorAtStart : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }
}