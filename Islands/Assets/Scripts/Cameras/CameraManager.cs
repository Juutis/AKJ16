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
    private CinemachineBrain brain;

    [SerializeField]
    private CinemachineVirtualCamera launcherVCam;
    [SerializeField]
    private CinemachineVirtualCamera launcherVCamInside;

    [SerializeField]
    private List<CinemachineVirtualCamera> launcherVCams;
    private CinemachineVirtualCamera currentLauncherVCam;

    private int defaultLauncherVCamIndex = 0;

    [SerializeField]
    private List<CinemachineVirtualCamera> flyingObjectVCams;
    private CinemachineVirtualCamera currentFlyingObjectVCam;


    private int maxVCamPriority = 100;
    private int launcherVCamDefaultPriority = 20;
    private int flyingObjectVCamDefaultPriority = 10;
    private Vector3 startFlyingPos;

    bool cameraIsAtLaunchPosition = false;
    public bool CameraIsAtLaunchPosition
    {
        get
        {
            return cameraIsAtLaunchPosition;
        }
    }

    public void Init()
    {
        currentLauncherVCam = launcherVCams[defaultLauncherVCamIndex];
        currentFlyingObjectVCam = flyingObjectVCams[Random.Range(0, flyingObjectVCams.Count - 1)];
        ResetFlyingCamera();
        ResetLauncherCamera();
        LookAtLauncher();
    }

    public void SwitchToLauncherVCam(int index)
    {
        if (index < launcherVCams.Count)
        {
            foreach (CinemachineVirtualCamera launcherCam in launcherVCams)
            {
                launcherCam.Priority = launcherVCamDefaultPriority;
            }
            currentLauncherVCam = launcherVCams[index];
        }
        LookAtLauncher();
    }

    public int LauncherVCamCount { get { return launcherVCams.Count; } }

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
                foreach (CinemachineVirtualCamera launcherCam in launcherVCams)
                {
                    launcherCam.Follow = followTarget;
                    launcherCam.LookAt = lookAt;
                }
            }
        }
        currentLauncherVCam.Priority = maxVCamPriority;
    }

    public void FollowFlyingObject(Transform objectTransform)
    {
        currentFlyingObjectVCam = flyingObjectVCams[Random.Range(0, flyingObjectVCams.Count - 1)];
        CinemachineTransposer tp = launcherVCam.GetCinemachineComponent<CinemachineTransposer>();
        currentFlyingObjectVCam.transform.position = startFlyingPos + tp.m_FollowOffset;
        currentFlyingObjectVCam.Follow = objectTransform.transform;
        currentFlyingObjectVCam.LookAt = objectTransform.transform;
        currentFlyingObjectVCam.Priority = maxVCamPriority;

        startFlyingPos = objectTransform.position;
    }

    public void ResetLauncherCamera()
    {
        currentLauncherVCam = launcherVCams[defaultLauncherVCamIndex];
        currentLauncherVCam.Priority = launcherVCamDefaultPriority;
    }
    public void ResetFlyingCamera()
    {
        CinemachineTransposer tp = currentLauncherVCam.GetCinemachineComponent<CinemachineTransposer>();
        currentFlyingObjectVCam.transform.position = startFlyingPos + tp.m_FollowOffset;
        currentFlyingObjectVCam.Priority = flyingObjectVCamDefaultPriority;
    }

    private CinemachineVirtualCamera prevVCam;
    void Update()
    {
        CinemachineVirtualCamera currentVCam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        cameraIsAtLaunchPosition = currentVCam != null && currentVCam == currentLauncherVCam && !brain.IsBlending;
    }

}
