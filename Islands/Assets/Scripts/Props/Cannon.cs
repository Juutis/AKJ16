using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float Pitch;

    public Transform PitchBone;

    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = PitchBone.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        PitchBone.localRotation = initialRotation * Quaternion.AngleAxis(-Pitch, transform.right);
    }
}
