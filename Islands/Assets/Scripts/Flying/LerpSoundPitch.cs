using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpSoundPitch : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Vector2 pitchRange = new Vector2(20f, -20f);

    [SerializeField]
    private float pitchSpeed = 5f;
    private float flightSpeed;
    private float pitchT = 0f;

    bool isPitching = false;
    public void StartPitching(float flightLength)
    {
        flightSpeed = pitchSpeed / flightLength;
        audioSource.Stop();
        audioSource.pitch = pitchRange.x;
        audioSource.Play();
        pitchT = 0f;
        isPitching = true;
    }

    public void StopPitching()
    {
        pitchT += 1f;
    }

    void Update()
    {
        if (isPitching)
        {
            pitchT += flightSpeed * Time.deltaTime;
            audioSource.pitch = Mathf.Lerp(pitchRange.x, pitchRange.y, pitchT);
            if (pitchT > 1)
            {
                isPitching = false;
                audioSource.Stop();
            }
        }
    }
}
