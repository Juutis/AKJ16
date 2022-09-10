using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchableObject : MonoBehaviour
{
    private Rigidbody rigidBaby;
    private bool isLaunched = false;
    private ObjectLauncher launcher;

    private float axisX = 0f;
    private float axisY = 0f;

    private bool shoot = false;
    private float bounceTime = 1f;
    private float lastBounce = 0f;
    private Vector3 velocity;
    private float launchForceSpeed;

    public void Launch(float launchForceSpeed, ObjectLauncher launcher)
    {
        this.launchForceSpeed = launchForceSpeed;
        this.launcher = launcher;
        if (isLaunched)
        {
            Debug.Log($"{name} is already launched!");
        }
        if (rigidBaby == null)
        {
            rigidBaby = GetComponent<Rigidbody>();
        }
        shoot = true;
        Invoke("PlayFlyingSound", 1f);
    }

    public void PlayFlyingSound()
    {

        SoundManager.main.PlayFlyingSound(launchForceSpeed);
    }

    private void Update()
    {
        if (isLaunched)
        {
            transform.forward = rigidBaby.velocity;
            GetInput();
        }
    }

    void FixedUpdate()
    {
        if (shoot)
        {
            rigidBaby.useGravity = true;
            isLaunched = true;
            shoot = false;
            velocity = rigidBaby.transform.forward * launchForceSpeed;
        }

        if (isLaunched)
        {
            Vector3 gravity = new Vector3(0, PhysicsConstants.gravity, 0) * Time.deltaTime;
            Vector3 scaledDrag = Vector3.one * PhysicsConstants.drag * Time.deltaTime;
            Vector3 horizontalMovement = transform.right * axisX * PhysicsConstants.flyMoveAmount * Time.deltaTime;
            Vector3 verticalBreak = transform.forward * Mathf.Min(0, axisY) * PhysicsConstants.flyBreakAmount * Time.deltaTime;
            velocity = velocity - gravity - scaledDrag + horizontalMovement + verticalBreak;
            rigidBaby.velocity = velocity;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == null)
        {
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            launcher.Reset();
            SoundManager.main.StopPlayingFlyingSound();
            SoundManager.main.PlaySound(GameSoundType.Molsk);
            return;
        }
        if (collision.collider.tag != "Bounce")
        {
            var normal = collision.GetContact(0).normal;
            var reflected = Vector3.Reflect(velocity, normal);
            var coef = PhysicsConstants.smallBounceCoef;
            velocity = reflected * coef;
            SoundManager.main.StopPlayingFlyingSound();
            SoundManager.main.PlaySound(GameSoundType.Hit);
            return;
        }
    }

    public void Bounce(Vector3 point, Vector3 normal)
    {
        if (bounceTime < (Time.time - lastBounce))
        {
            velocity = Vector3.Reflect(velocity, normal) * PhysicsConstants.bigBounceCoef;
            lastBounce = Time.time;
        }
    }

    private void GetInput()
    {
        axisY = Input.GetAxis("Vertical");
        axisX = Input.GetAxis("Horizontal");
    }
}
