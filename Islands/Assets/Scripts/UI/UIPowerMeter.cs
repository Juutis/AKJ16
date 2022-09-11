using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerMeter : MonoBehaviour
{
    [SerializeField]
    private Image imgMeter;
    [SerializeField]
    private Image oldImgMeter;
    [SerializeField]
    private Text txtKey;

    [SerializeField]
    private Animator animator;

    private bool canUsePower;
    public bool CanUsePower { get { return canUsePower; } }

    private bool isShown = false;

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        canUsePower = false;
        animator.SetTrigger("Hide");
    }

    public void ShowFinished()
    {
        canUsePower = true;
    }

    public void Init(KeyCode key)
    {
        txtKey.text = $"{key}";
    }

    public void UpdateMeter(float amount)
    {
        imgMeter.fillAmount = amount;
    }

    public void ClearMeter()
    {
        oldImgMeter.fillAmount = imgMeter.fillAmount;
        imgMeter.fillAmount = 0;
    }

    private void Update()
    {
        if (!isShown && CameraManager.main.CameraIsAtLaunchPosition)
        {
            isShown = true;
            Show();
        }
        if (isShown && !CameraManager.main.CameraIsAtLaunchPosition)
        {
            isShown = false;
            Hide();
        }
    }
}
