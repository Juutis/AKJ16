using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Text txtStart;
    [SerializeField]
    private KeyCode startKey;
    private bool started = false;

    void Start()
    {
        txtStart.text = $"- Press {startKey} to start -";
    }

    void Update()
    {
        if (!started && Input.GetKeyDown(startKey))
        {
            started = true;
            animator.SetTrigger("Start");
        }
    }

    public void StartFinished()
    {
        CameraManager.main.Init();
        Destroy(gameObject);
    }
}
