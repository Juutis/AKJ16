using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchableObject : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem sandParticles;

    [SerializeField]
    private ParticleSystem waterParticles;

    private Rigidbody rigidBaby;
    private bool isLaunched = false;
    private ObjectLauncher launcher;
    public bool crashed = false;

    private float axisX = 0f;
    private float axisY = 0f;

    private bool shoot = false;
    private float bounceTime = 0.33f;
    private float lastBounce = 0f;

    private float lastSkip = 0f;
    private float skipTime = 0.33f;
    public int skipCount = 0;

    private float crashStarted = 0f;
    private const float timeUntilCrashRestarts = 10f;

    private Vector3 velocity;
    private float launchForceSpeed;

    private float previousYPos;
    private float minYPosForFallingSound = 5f;
    private bool flyingSoundIsPlaying = false;
    private bool spaceReady = true;
    private float spacePressed = 0;
    private float waterWasHit = 0;
    private float skipToleranceBefore = 0.5f;
    private float skipToleranceAfter = 0.25f;

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
        rigidBaby.useGravity = false;
        crashStarted = 0f;
        crashed = false;
        previousYPos = transform.position.y;
        UIManager.main.IncreaseScore();
    }

    public void PlayFlyingSound()
    {
        SoundManager.main.PlayFlyingSound(launchForceSpeed);
        flyingSoundIsPlaying = true;
    }
    public void StopPlayingFlyingSound()
    {
        SoundManager.main.StopPlayingFlyingSound();
        flyingSoundIsPlaying = false;
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
            // rigidBaby.useGravity = true;
            isLaunched = true;
            shoot = false;
            velocity = rigidBaby.transform.forward * launchForceSpeed;
        }

        if (isLaunched && !crashed)
        {
            if (!flyingSoundIsPlaying && previousYPos > minYPosForFallingSound && transform.position.y < previousYPos)
            {
                PlayFlyingSound();
            }
            Vector3 wind = GameManager.main.GetWind() * Time.deltaTime;
            Vector3 gravity = new Vector3(0, PhysicsConstants.gravity, 0) * Time.deltaTime;
            Vector3 scaledDrag = Vector3.one * PhysicsConstants.drag * Time.deltaTime;
            Vector3 horizontalMovement = transform.right * axisX * PhysicsConstants.flyMoveAmount * Time.deltaTime;
            Vector3 verticalBreak = transform.forward * Mathf.Min(0, axisY) * PhysicsConstants.flyBreakAmount * Time.deltaTime;
            velocity = velocity - gravity - scaledDrag + horizontalMovement + verticalBreak + wind;
            rigidBaby.velocity = velocity;
            previousYPos = transform.position.y;
        }

        if (crashed && Time.time - crashStarted > timeUntilCrashRestarts)
        {
            launcher.Reset();
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
            SoundManager.main.StopPlayingFlyingSound();
            SoundManager.main.PlaySound(GameSoundType.Hit);
            rigidBaby.useGravity = true;
            rigidBaby.AddForce(normal * PhysicsConstants.crashForceAmount, ForceMode.Impulse);
            if (!crashed)
            {
                crashStarted = Time.time;
                crashed = true;
            }
        }

        if (collision.collider.tag == "DEATH")
        {
            launcher.Reset();
            StopPlayingFlyingSound();
            SoundManager.main.PlaySound(GameSoundType.Molsk);
            var wp = Instantiate(waterParticles);
            wp.transform.parent = null;
            wp.transform.rotation = Quaternion.Euler(-90, 0, 0);
            var oldPos = transform.position;
            wp.transform.position = new Vector3(oldPos.x, -8.3f, oldPos.z);
            wp.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            waterWasHit = Time.time;
            HandleSkipping();
        }
    }

    private void HandleSkipping()
    {
        if (Time.time - lastSkip < skipTime)
        {
            return;
        }
        if (spacePressed < waterWasHit - skipToleranceBefore)
        {
            return;
        }
        if (spacePressed > waterWasHit + skipToleranceAfter)
        {
            return;
        }

        float skipPrecision = 0;
        if (spacePressed < waterWasHit)
        {
            skipPrecision = 1.0f - (waterWasHit - spacePressed) / skipToleranceBefore;
        }
        else
        {
            skipPrecision = 1.0f - (spacePressed - waterWasHit) / skipToleranceAfter;
        }

        Vector3 contactNormal = Vector3.up;
        float angle = 90 - (180 - Vector3.Angle(this.rigidBaby.velocity, contactNormal));
        skipPrecision *= (1.0f - angle / 80.0f);
        float maxSkipAngle = 80;
        float minSkipAngle = 0;
        Debug.Log(skipPrecision);
        if (angle > minSkipAngle && angle < maxSkipAngle && rigidBaby.velocity.magnitude > PhysicsConstants.minSkipSpeed && skipCount < PhysicsConstants.maxSkips)
        {
            var skipYVel = -velocity.y * PhysicsConstants.smallSkipBoost * skipPrecision;
            var dragFactor = Mathf.Lerp(PhysicsConstants.smallSkipDragMin, PhysicsConstants.smallSkipDragMax, skipPrecision);
            velocity = new Vector3(velocity.x, skipYVel, velocity.z) * dragFactor;
            lastSkip = Time.time;
            skipCount++;
            StopPlayingFlyingSound();
            SoundManager.main.PlaySound(GameSoundType.Skip);
            var wp = Instantiate(waterParticles);
            wp.transform.parent = null;
            wp.transform.rotation = Quaternion.Euler(-90, 0, 0);
            var oldPos = transform.position;
            wp.transform.position = new Vector3(oldPos.x, -8.3f, oldPos.z);
            wp.Play();
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (spaceReady)
            {
                spaceReady = false;
                spacePressed = Time.time;
                Invoke("ReadySpace", 1.0f);
                HandleSkipping();
            }
        }
    }

    private void ReadySpace()
    {
        spaceReady = true;
    }

    public void HitGround()
    {
        sandParticles.transform.parent = null;
        sandParticles.transform.rotation = Quaternion.Euler(-90, 0, 0);
        sandParticles.Play();
    }
}
