using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    private bool outOfCannon = false;
    private bool shoot = false;
    private Rigidbody body;
    private Vector3 startPos;
    private Quaternion startRotation;
    private float force = 12;
    [SerializeField]
    private GameObject cannon;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        startPos = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shoot = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            body.useGravity = false;
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            transform.position = startPos;
            transform.rotation = startRotation;
            outOfCannon = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            force += 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            force -= 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            cannon.transform.Rotate(Vector3.up, -50 * Time.deltaTime);
            if (!outOfCannon)
            {
                startPos = transform.position;
                startRotation = transform.rotation;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            cannon.transform.Rotate(Vector3.up, 50 * Time.deltaTime);
            if (!outOfCannon)
            {
                startPos = transform.position;
                startRotation = transform.rotation;
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            cannon.transform.Rotate(Vector3.right, -50 * Time.deltaTime);
            if (!outOfCannon)
            {
                startPos = transform.position;
                startRotation = transform.rotation;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            cannon.transform.Rotate(Vector3.right, 50 * Time.deltaTime);
            if (!outOfCannon)
            {
                startPos = transform.position;
                startRotation = transform.rotation;
            }
        }
    }

    void FixedUpdate()
    {
        if (shoot)
        {
            body.useGravity = true;
            outOfCannon = true;
            shoot = false;
            body.AddForce(body.transform.up * force, ForceMode.Impulse);
        }
    }
}
