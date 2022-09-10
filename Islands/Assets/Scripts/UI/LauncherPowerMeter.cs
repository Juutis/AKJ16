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

    [SerializeField]
    private LineRenderer line;

    private bool isMoving = false;
    private float stepTimer = 0f;
    private int currentStep = 1;
    private int direction = 1;

    //private Transform target;
    private Vector3 targetDirection;
    private Vector3 origin;
    public void SetTarget(Transform newTarget)
    {
        targetDirection = newTarget.forward;
        origin = newTarget.localPosition;
        //target = newTarget;
        isMoving = true;
    }
    public void Reset()
    {
        currentStep = 1;
        DrawLinePositions();
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
                DrawLinePositions();
                stepTimer = 0f;
            }
        }
    }

    private void DrawLinePositions()
    {
        line.positionCount = currentStep;
        Vector3[] positions = new Vector3[currentStep];
        Vector3 targetPos = origin + targetDirection * (currentStep * stepSize);
        for (int index = 0; index < currentStep; index += 1)
        {
            //positions[index] = new Vector3(startPos.x, startPos.y, index * stepSize);
            float t = (index * 1.0f) / (currentStep * 1.0f);
            positions[index] = Vector3.Lerp(origin, targetPos, t);
        }
        line.SetPositions(positions);
    }
}
