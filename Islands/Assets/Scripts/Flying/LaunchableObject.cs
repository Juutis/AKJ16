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
    private float bounceTime = 0.33f;
    private float lastBounce = 0f;

    private float lastSkip = 0f;
    private float skipTime = 0.33f;
    public int skipCount = 0;

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
        skipCount = 0;
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
            Vector3 wind = GameManager.main.GetWind();
            Vector3 gravity = new Vector3(0, PhysicsConstants.gravity, 0) * Time.deltaTime;
            Vector3 scaledDrag = Vector3.one * PhysicsConstants.drag * Time.deltaTime;
            Vector3 horizontalMovement = transform.right * axisX * PhysicsConstants.flyMoveAmount * Time.deltaTime;
            Vector3 verticalBreak = transform.forward * Mathf.Min(0, axisY) * PhysicsConstants.flyBreakAmount * Time.deltaTime;
            velocity = velocity - gravity - scaledDrag + horizontalMovement + verticalBreak + wind;
            rigidBaby.velocity = velocity;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == null)
        {
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if (Time.time - lastSkip < skipTime)
            {
                return;
            }

            Vector3 contactNormal = other.transform.up;
            float angle = 90 - (180 - Vector3.Angle(this.rigidBaby.velocity, contactNormal));
            float maxSkipAngle = 20;
            float minSkipAngle = 0;
            if (angle > minSkipAngle && angle < maxSkipAngle  && rigidBaby.velocity.magnitude > PhysicsConstants.minSkipSpeed && skipCount < PhysicsConstants.maxSkips)
            {
                Debug.Log($"Skipping {angle}, {this.rigidBaby.velocity.x}, {this.rigidBaby.velocity.y}, {this.rigidBaby.velocity.z} | {contactNormal.x}, {contactNormal.y}, {contactNormal.z}");
                var skipYVel = -velocity.y * PhysicsConstants.smallSkipBoost;
                velocity = new Vector3(velocity.x, skipYVel, velocity.z) * PhysicsConstants.smallSkipDrag;
                lastSkip = Time.time;
                skipCount++;
            }
            else
            {
                // ContactPoint pointOfContact2 = collision.GetContact(0);
                Debug.Log($"Not skipping {angle}, {this.rigidBaby.velocity.x}, {this.rigidBaby.velocity.y}, {this.rigidBaby.velocity.z} | {contactNormal.x}, {contactNormal.y}, {contactNormal.z}");
                launcher.Reset();
                SoundManager.main.StopPlayingFlyingSound();
                SoundManager.main.PlaySound(GameSoundType.Molsk);
            }
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
