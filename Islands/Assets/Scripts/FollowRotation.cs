using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool followX;
    [SerializeField]
    private bool followY;
    [SerializeField]
    private bool followZ;
    public void Follow(Transform newTarget)
    {
        target = newTarget;
    }
    private void Update()
    {
        if (target != null)
        {
            Vector3 newRotation = transform.localEulerAngles;
            Vector3 targetRotation = target.transform.localEulerAngles;
            if (followX)
            {
                newRotation.x = targetRotation.x;
            }
            if (followY)
            {
                newRotation.y = targetRotation.y;
            }
            if (followZ)
            {
                newRotation.z = targetRotation.z;
            }
            transform.localEulerAngles = newRotation;
        }
    }
}
