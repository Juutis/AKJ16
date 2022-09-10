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
    private Vector3 startFlyingPos;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        ResetFlyingCamera();
        ResetLauncherCamera();
        Invoke("LookAtLauncher", 1);
    }

    public void LookAtLauncher()
    {
        ResetFlyingCamera();
        if (launcherVCamTargetTag != "")
        {
            var followTarget = GameObject.FindGameObjectWithTag(launcherVCamTargetTag).transform;
            followTarget = followTarget.transform.Find("LaunchPreview");
            var lookAt = followTarget.Find("CameraAimTarget");
            if (followTarget != null)
            {
                launcherVCam.Follow = followTarget;
                launcherVCam.LookAt = lookAt;
            }
        }
        launcherVCam.Priority = maxVCamPriority;
    }

    public void FollowFlyingObject(Transform objectTransform)
    {
        flyingObjectVCam.Follow = objectTransform.transform;
        flyingObjectVCam.LookAt = objectTransform.transform;
        flyingObjectVCam.Priority = maxVCamPriority;
        startFlyingPos = objectTransform.position;
    }

    public void ResetLauncherCamera()
    {
        launcherVCam.Priority = launcherVCamDefaultPriority;
    }
    public void ResetFlyingCamera()
    {
        CinemachineTransposer tp = launcherVCam.GetCinemachineComponent<CinemachineTransposer>();
        flyingObjectVCam.transform.position = startFlyingPos + tp.m_FollowOffset;
        flyingObjectVCam.Priority = flyingObjectVCamDefaultPriority;
    }

}
