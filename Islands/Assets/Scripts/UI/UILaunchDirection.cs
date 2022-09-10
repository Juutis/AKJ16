using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILaunchDirection : MonoBehaviour
{
    [SerializeField]
    private Text txtAngle;
    [SerializeField]
    private Text txtDirection;

    public void Display(Vector3 direction) {
        txtAngle.text = $"Angle: {direction.y}°";
        txtDirection.text = $"Direction: {direction.x}°";
    }
}
