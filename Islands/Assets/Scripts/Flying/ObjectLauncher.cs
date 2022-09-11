using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLauncher : MonoBehaviour
{
    [SerializeField]
    private LaunchableObject objectToLaunchPrefab;
    [SerializeField]
    private LaunchPreview launchPreview;
    [SerializeField]
    private Cannon cannon;
    [SerializeField]
    private ParticleSystem muzzleFlash;

    private float inputMin = 0.01f;

    [SerializeField]
    private Vector2 clampHorizontal;
    [SerializeField]
    private Vector2 clampVertical;

    [SerializeField]
    private Vector2 rotateSpeed = new Vector2(2, 3);

    [SerializeField]
    private Vector2 launchSpeed = new Vector2(12, 24);
    [SerializeField]
    private KeyCode launchKey = KeyCode.Space;
    [SerializeField]
    private LauncherPowerMeter powerMeter;
    private float axisX;
    private float axisY;
    private float mouseX;
    private float mouseY;

    private Vector3 direction;

    private bool isLaunched = false;
    private bool isLaunchKeyDown = false;
    private LaunchableObject launchedObject;


    private void Start()
    {
        float startHorizontal = Mathf.Lerp(clampHorizontal.x, clampHorizontal.y, 0.5f);
        float startVertical = Mathf.Lerp(clampVertical.x, clampVertical.y, 0.1f);
        direction = new Vector3(startVertical, startHorizontal, 0f);
        UIManager.main.InitPowerMeter(launchKey);
    }

    public void Reset()
    {
        isLaunched = false;
        if (launchedObject != null)
        {
            Destroy(launchedObject.gameObject);
            launchedObject = null;
        }
        CameraManager.main.LookAtLauncher();
    }

    private void Update()
    {
        GetInput();
        RotatePreview();
        if (Input.GetKeyUp(KeyCode.R))
        { // for debugging
            Reset();
        }
        if (UIManager.main.CanUsePower() && !isLaunchKeyDown && !isLaunched && Input.GetKey(launchKey))
        {
            isLaunchKeyDown = true;
        }
        if (!isLaunched && isLaunchKeyDown && Input.GetKeyUp(launchKey))
        {
            Launch();
            isLaunched = true;
            isLaunchKeyDown = false;
            powerMeter.StopMoving();
        }

    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.Mouse1)) {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        } else {
            mouseX = 0;
            mouseY = 0;
        }

        if (!isLaunched)
        {
            axisY = Input.GetAxis("Vertical") + mouseY;
            axisX = Input.GetAxis("Horizontal") + mouseX;
        }
    }

    private void RotatePreview()
    {
        if (Mathf.Abs(axisX) > inputMin)
        {
            bool canRotate = (axisX > 0 && direction.y < clampHorizontal.y) || (axisX < 0 && direction.y > clampHorizontal.x);
            if (canRotate)
            {
                SoundManager.main.PlaySoundLoop(GameSoundType.TurnHorizontal);
            }
            var dirDiff = axisX * rotateSpeed.x * Time.deltaTime;
            var oldDir = direction.y;
            var newDir = direction.y + dirDiff;
            direction.y = Mathf.Clamp(newDir, clampHorizontal.x, clampHorizontal.y);
            var realDiff = direction.y - oldDir;
            cannon.RotateRight(realDiff);
        }
        if (Mathf.Abs(axisY) > inputMin)
        {
            bool canRotate = (axisY < 0 && direction.x < clampVertical.y) || (axisY > 0 && direction.x > clampVertical.x);
            if (canRotate)
            {
                SoundManager.main.PlaySoundLoop(GameSoundType.TurnVertical);
            }
            var dirDiff = axisY * rotateSpeed.y * Time.deltaTime;
            direction.x = Mathf.Clamp(direction.x - dirDiff, clampVertical.x, clampVertical.y);
        }
        launchPreview.DisplayDirection(transform.position, new Vector2(-direction.x, direction.y));
        cannon.transform.localEulerAngles = new Vector2(0, direction.y);
        cannon.Pitch = direction.x;
        if (isLaunchKeyDown)
        {
            powerMeter.StartMoving();
        }
    }

    private void Launch()
    {
        if (objectToLaunchPrefab != null)
        {
            launchedObject = Instantiate(objectToLaunchPrefab, transform.position, launchPreview.transform.rotation);
            launchedObject.Launch(Mathf.Lerp(launchSpeed.x, launchSpeed.y, powerMeter.GetPower()), this);
            CameraManager.main.FollowFlyingObject(launchedObject.transform);
            muzzleFlash.Play();
            GameManager.main.SetFlyingObject(launchedObject);
            SoundManager.main.PlaySound(GameSoundType.Boom);
        }
    }
}
