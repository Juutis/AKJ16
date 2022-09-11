using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILauncherCameraSingle : MonoBehaviour
{
    private Color originalColor;
    [SerializeField]
    private Color currentColor;
    [SerializeField]
    private Image imgBg;
    [SerializeField]
    private Text txtValue;
    public void Init(int num)
    {
        originalColor = imgBg.color;
        txtValue.text = $"{num}";
    }

    public void SetCurrent()
    {
        imgBg.color = currentColor;
    }

    public void Reset()
    {
        imgBg.color = originalColor;
    }
}
