using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerMeter : MonoBehaviour
{
    [SerializeField]
    private Image imgMeter;
    [SerializeField]
    private Image oldImgMeter;
    [SerializeField]
    private Text txtKey;

    public void Init(KeyCode key)
    {
        txtKey.text = $"{key}";
    }

    public void UpdateMeter(float amount)
    {
        imgMeter.fillAmount = amount;
    }

    public void ClearMeter()
    {
        oldImgMeter.fillAmount = imgMeter.fillAmount;
        imgMeter.fillAmount = 0;
    }
}
