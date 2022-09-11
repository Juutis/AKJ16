using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerMeter : MonoBehaviour
{
    [SerializeField]
    private Image imgMeter;

    public void UpdateMeter(float amount)
    {
        imgMeter.fillAmount = amount;
    }

    public void ClearMeter()
    {
        imgMeter.fillAmount = 0;
    }
}
