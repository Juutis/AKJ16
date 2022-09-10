using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLauncher : MonoBehaviour
{
    [SerializeField]
    private LaunchableObject objectToLaunchPrefab;
    [SerializeField]
    private LaunchPreview launchPreview;

    private float inputMin = 0.01f;

    [SerializeField]
    private Vector2 clampHorizontal;
    [SerializeField]
    private Vector2 clampVertical;

    [SerializeField]
    private Vector2 rotateSpeed = new Vector2(2, 3);

    [SerializeField]
    private float launchSpeed = 5f;
    private float axisX;
    private float axisY;

    private Vector3 direction;

    private bool isLaunched = false;
    private LaunchableObject launchedObject;

    private void Start()
    {
        float startHorizontal = Mathf.Lerp(clampHorizontal.x, clampHorizontal.y, 0.5f);
        float startVertical = Mathf.Lerp(clampVertical.x, clampVertical.y, 0.5f);
        direction = new Vector3(startVertical, startHorizontal, 0f);
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

        UIManager.main.DisplayLaunchDirection(direction);
        if (Input.GetKeyUp(KeyCode.R))
        { // for debugging
            Reset();
        }
        if (!isLaunched && Input.GetKeyUp(KeyCode.Space))
        {
            isLaunched = true;
            Launch();
        }

    }

    private void GetInput()
    {
        axisY = Input.GetAxis("Vertical");
        axisX = Input.GetAxis("Horizontal");
    }

    private void RotatePreview()
    {
        if (Mathf.Abs(axisX) > inputMin)
        {
            direction.y = Mathf.Clamp(direction.y + axisX * rotateSpeed.x, clampHorizontal.x, clampHorizontal.y);
        }
        if (Mathf.Abs(axisY) > inputMin)
        {
            direction.x = Mathf.Clamp(direction.x - axisY * rotateSpeed.y, clampVertical.x, clampVertical.y);
        }
        launchPreview.DisplayDirection(transform.position, direction);
        Debug.DrawLine(launchPreview.transform.position, launchPreview.transform.forward * 5f, Color.magenta, 0.1f);
    }

    private void Launch()
    {
        if (objectToLaunchPrefab != null)
        {
            launchedObject = Instantiate(objectToLaunchPrefab, transform.position, launchPreview.transform.rotation);
            launchedObject.Launch(launchSpeed, this);
            CameraManager.main.FollowFlyingObject(launchedObject.transform);
        }
    }
}
