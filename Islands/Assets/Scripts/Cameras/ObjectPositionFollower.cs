using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositionFollower : MonoBehaviour
{
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
            Vector3 newPosition = transform.position;
            Vector3 targetPosition = target.transform.position;
            if (followX)
            {
                newPosition.x = targetPosition.x;
            }
            if (followY)
            {
                newPosition.y = targetPosition.y;
            }
            if (followZ)
            {
                newPosition.z = targetPosition.z;
            }
            transform.position = newPosition;
        }
    }
}
