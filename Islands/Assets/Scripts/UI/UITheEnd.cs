using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITheEnd : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private bool isShown = false;
    [SerializeField]
    private Text txtScore;
    public void Show()
    {
        if (!isShown)
        {
            animator.SetTrigger("Show");
            txtScore.text = $"Cannons launched: {UIManager.main.GetScore()}";
            isShown = true;
        }
    }
}
