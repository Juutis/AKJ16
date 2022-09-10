using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchableObject : MonoBehaviour
{
    private Rigidbody rigidBaby;
    private bool isLaunched = false;
    private ObjectLauncher launcher;
    public void Launch(float speed, ObjectLauncher launcher)
    {
        this.launcher = launcher;
        if (isLaunched)
        {
            Debug.Log($"{name} is already launched!");
        }
        if (rigidBaby == null)
        {
            rigidBaby = GetComponent<Rigidbody>();
        }
        rigidBaby.AddForce(transform.forward * speed);
        isLaunched = true;
    }

    private void Update()
    {
        if (isLaunched)
        {
            //transform.up = rigidBaby.velocity;
            transform.forward = rigidBaby.velocity;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null && collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            launcher.Reset();
        }
    }
}
