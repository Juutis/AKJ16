using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITheEnd : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private bool isShown = false;
    public void Show()
    {
        if (!isShown)
        {
            animator.SetTrigger("Show");
            isShown = true;
        }
    }
}
