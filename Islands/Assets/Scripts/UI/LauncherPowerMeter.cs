using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherPowerMeter : MonoBehaviour
{
    [SerializeField]
    private float timePerStep = 0.2f;
    [SerializeField]
    private float stepSize = 0.5f;
    [SerializeField]
    private int maxSteps = 10;

    private bool isMoving = false;
    private float stepTimer = 0f;
    private int currentStep = 1;
    private int direction = 1;

    public void StartMoving()
    {
        //target = newTarget;
        if (isMoving)
        {
            return;
        }
        UIManager.main.ClearPowerMeter();
        isMoving = true;
    }
    public void Reset()
    {
        currentStep = 1;
    }
    public void StopMoving()
    {
        isMoving = false;
        Reset();
    }

    public float GetPower()
    {
        return (float)currentStep / (float)maxSteps;
    }

    private void Update()
    {
        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer > timePerStep)
            {
                int nextStep = currentStep + direction;
                if (nextStep > maxSteps || nextStep < 0)
                {
                    direction = -direction;
                }
                currentStep += direction;
                UIManager.main.UpdatePowerMeter(GetPower());
                stepTimer = 0f;
            }
        }
    }
}
