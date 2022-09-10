using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float Pitch;

    public Transform PitchBone;
    public Transform LeftWheel;
    public Transform RightWheel;

    private Quaternion initialRotation;
    private float wheelRotationScale = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = PitchBone.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, -10 * Time.deltaTime);
        RotateLeft(10 * Time.deltaTime);

        PitchBone.localRotation = initialRotation * Quaternion.AngleAxis(-Pitch, Vector3.right);
    }

    public void RotateLeft(float angle) {
        LeftWheel.Rotate(Vector3.up, angle * wheelRotationScale);
        RightWheel.Rotate(Vector3.up, angle * wheelRotationScale);

    }

    public void RotateRight(float angle) {
        LeftWheel.Rotate(Vector3.up, -angle * wheelRotationScale);
        RightWheel.Rotate(Vector3.up, -angle * wheelRotationScale);
    }


}
