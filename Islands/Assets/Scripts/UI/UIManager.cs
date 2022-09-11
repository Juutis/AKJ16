using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;
    void Awake()
    {
        main = this;
    }
    [SerializeField]
    private UILaunchDirection uiLaunchDirection;
    [SerializeField]
    private UIPowerMeter uiPowerMeter;


    public void DisplayLaunchDirection(Vector3 direction)
    {
        uiLaunchDirection.Display(direction);
    }

    public void UpdatePowerMeter(float amount)
    {
        uiPowerMeter.UpdateMeter(amount);
    }

    public void ClearPowerMeter()
    {
        uiPowerMeter.ClearMeter();
    }
}
