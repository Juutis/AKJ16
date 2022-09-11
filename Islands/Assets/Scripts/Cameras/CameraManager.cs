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

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        currentFlyingObjectVCam = flyingObjectVCams[Random.Range(0, flyingObjectVCams.Count - 1)];
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
        launcherVCam.Priority = launcherVCamDefaultPriority;
    }
    public void ResetFlyingCamera()
    {
        CinemachineTransposer tp = launcherVCam.GetCinemachineComponent<CinemachineTransposer>();
        currentFlyingObjectVCam.transform.position = startFlyingPos + tp.m_FollowOffset;
        currentFlyingObjectVCam.Priority = flyingObjectVCamDefaultPriority;
    }

    private CinemachineVirtualCamera prevVCam;
    void Update()
    {
        CinemachineVirtualCamera currentVCam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        cameraIsAtLaunchPosition = currentVCam == launcherVCam && !brain.IsBlending;
    }

}
