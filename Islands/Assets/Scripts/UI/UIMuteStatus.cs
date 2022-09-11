using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMuteStatus : MonoBehaviour
{
    [SerializeField]
    private Image imgMuted;
    [SerializeField]
    private Text txtMuteKey;

    public void Init(bool state, KeyCode key)
    {
        imgMuted.enabled = state;
        txtMuteKey.text = $"{key}";
    }
    public void Toggle()
    {
        imgMuted.enabled = !imgMuted.enabled;
    }
}
