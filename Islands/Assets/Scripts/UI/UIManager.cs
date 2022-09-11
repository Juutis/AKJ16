using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private UIMainMenu uiMainMenu;
    [SerializeField]
    private UITheEnd theEndMenu;
    [SerializeField]
    private UIPowerMeter uiPowerMeter;
    [SerializeField]
    private UIMuteStatus musicMuted;
    [SerializeField]
    private UIMuteStatus sfxMuted;
    [SerializeField]
    private KeyCode muteSfxKey;
    [SerializeField]
    private KeyCode muteMusicKey;

    void Start()
    {
        sfxMuted.Init(SoundManager.main.SfxMuted, muteSfxKey);
        musicMuted.Init(SoundManager.main.MusicMuted, muteMusicKey);
#if !UNITY_EDITOR
        uiMainMenu.gameObject.SetActive(true);
#else
        CameraManager.main.Init();
#endif
    }

    public void ShowTheEnd()
    {
        theEndMenu.gameObject.SetActive(true);
        theEndMenu.Show();
    }

    public void InitPowerMeter(KeyCode key)
    {
        uiPowerMeter.Init(key);
    }

    public void UpdatePowerMeter(float amount)
    {
        uiPowerMeter.UpdateMeter(amount);
    }

    public void ClearPowerMeter()
    {
        uiPowerMeter.ClearMeter();
    }

    public bool CanUsePower()
    {
        return uiPowerMeter.CanUsePower;
    }

    void Update()
    {
        if (Input.GetKeyDown(muteSfxKey))
        {
            SoundManager.main.ToggleSfx();
            sfxMuted.Toggle();
        }
        if (Input.GetKeyDown(muteMusicKey))
        {
            SoundManager.main.ToggleMusic();
            musicMuted.Toggle();
        }
    }
}
