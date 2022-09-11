using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;
    [SerializeField]
    private Animator animator;
    private bool isShown = false;
    public void SetScore(int score)
    {
        txtScore.text = $"{score}";
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
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
