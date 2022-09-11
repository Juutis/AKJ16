using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 50f)]
    private float windMagnitude;

    public float Magnitude
    {
        get { return windMagnitude; }
        set { windMagnitude = value; }
    }
}
