using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPreview : MonoBehaviour
{
    public void DisplayDirection(Vector3 origin, Vector3 direction)
    {
        transform.position = origin;
        transform.eulerAngles = direction;
    }
}
