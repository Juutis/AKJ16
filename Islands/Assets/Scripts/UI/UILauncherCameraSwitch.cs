using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILauncherCameraSwitch : MonoBehaviour
{
    private int numCameras = 1;
    [SerializeField]
    private Animator animator;

    private bool isEnabled = false;
    private bool isShown = false;
    private KeyCode toggleKey;
    [SerializeField]

    private Text txtKey;

    private int currentCamera = 1;

    private float minScroll = 0.01f;

    [SerializeField]
    private List<UILauncherCameraSingle> launcherCams;

    public void Init(KeyCode key, int currentCamera)
    {
        this.currentCamera = currentCamera;
        toggleKey = key;
        numCameras = CameraManager.main.LauncherVCamCount;
        int index = 1;
        foreach (UILauncherCameraSingle launcherCam in launcherCams)
        {
            launcherCam.Init(index);
            index += 1;
        }
        txtKey.text = $"{toggleKey}";
        launcherCams[currentCamera - 1].SetCurrent();
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void ShowFinished()
    {
        isEnabled = true;
    }

    public void Hide()
    {
        isEnabled = false;
        animator.SetTrigger("Hide");
    }

    private void Switch(int direction)
    {
        SwitchToNum(currentCamera + direction);
    }

    private void SwitchToNum(int num)
    {
        if (num > numCameras)
        {
            num = 1;
        }
        else if (num < 1)
        {
            num = numCameras;
        }
        CameraManager.main.SwitchToLauncherVCam(num - 1);
        foreach (UILauncherCameraSingle launcherCam in launcherCams)
        {
            launcherCam.Reset();
        }
        launcherCams[num - 1].SetCurrent();
        currentCamera = num;
    }

    private void Update()
    {
        if (!isShown && !isEnabled && UIManager.main.CanUsePower())
        {
            isShown = true;
            Show();
        }
        if (isShown && isEnabled && !UIManager.main.CanUsePower())
        {
            isShown = false;
            Hide();
        }
        if (isEnabled && isShown)
        {
            bool scrolledUp = Input.GetAxis("Mouse ScrollWheel") > minScroll;
            bool scrolledDown = Input.GetAxis("Mouse ScrollWheel") < -minScroll;
            if (scrolledUp || Input.GetKeyDown(toggleKey))
            {
                Switch(1);
            }
            else if (scrolledDown)
            {
                Switch(-1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchToNum(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchToNum(2);
            }
        }
    }
}
