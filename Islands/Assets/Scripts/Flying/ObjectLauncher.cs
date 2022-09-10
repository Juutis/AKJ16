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

    private Vector3 direction = new Vector3(45, 0, 0);

    private bool isLaunched = false;
    private LaunchableObject launchedObject;

    public void Reset()
    {
        isLaunched = false;
        if (launchedObject != null)
        {
            Destroy(launchedObject.gameObject);
            launchedObject = null;
        }
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
