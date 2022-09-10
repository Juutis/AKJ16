using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager main;
    private void Awake()
    {
        main = this;
    }

    [SerializeField]
    private string launcherVCamTargetTag;

    [SerializeField]
    private CinemachineVirtualCamera launcherVCam;
    [SerializeField]
    private CinemachineVirtualCamera flyingObjectVCam;

    private int maxVCamPriority = 100;
    private int launcherVCamDefaultPriority = 20;
    private int flyingObjectVCamDefaultPriority = 10;


    private void Start()
    {
        Init();
    }
    private void Init()
    {
        launcherVCam.Priority = launcherVCamDefaultPriority;
        flyingObjectVCam.Priority = flyingObjectVCamDefaultPriority;
        if (launcherVCamTargetTag != "")
        {
            GameObject followTarget = GameObject.FindGameObjectWithTag(launcherVCamTargetTag);
            if (followTarget != null)
            {
                launcherVCam.Follow = followTarget.transform;
            }
        }
    }

    public void FollowFlyingObject(Transform objectTransform)
    {
        flyingObjectVCam.Follow = objectTransform;
        flyingObjectVCam.LookAt = objectTransform;
        flyingObjectVCam.Priority = maxVCamPriority;
    }

}
