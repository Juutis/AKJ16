using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    private bool outOfCannon = false;
    private bool shoot = false;
    private Rigidbody body;
    private Vector3 startPos;
    private Quaternion startRotation;
    private float forceAmount = 12;
    [SerializeField]
    private GameObject cannon;
    private float bounceTime = 1f;
    private float lastBounce = 0f;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        startPos = transform.position;
        startRotation = transform.rotation;
        velocity = Vector3.zero;
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
            forceAmount += 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            forceAmount -= 1;
        }

        if (Input.GetKey(KeyCode.A) && !outOfCannon)
        {
            cannon.transform.Rotate(Vector3.up, -50 * Time.deltaTime);
            startPos = transform.position;
            startRotation = transform.rotation;
        }
        if (Input.GetKey(KeyCode.D) && !outOfCannon)
        {
            cannon.transform.Rotate(Vector3.up, 50 * Time.deltaTime);
            startPos = transform.position;
            startRotation = transform.rotation;
        }
        if (Input.GetKey(KeyCode.W) && !outOfCannon)
        {
            cannon.transform.Rotate(Vector3.right, -50 * Time.deltaTime);
            startPos = transform.position;
            startRotation = transform.rotation;
        }
        if (Input.GetKey(KeyCode.S) && !outOfCannon)
        {
            cannon.transform.Rotate(Vector3.right, 50 * Time.deltaTime);
            startPos = transform.position;
            startRotation = transform.rotation;
        }
    }

    void FixedUpdate()
    {
        if (shoot)
        {
            body.useGravity = true;
            outOfCannon = true;
            shoot = false;
            //body.AddForce(body.transform.up * force, ForceMode.Impulse);
            velocity = body.transform.up * forceAmount;
        }

        if (outOfCannon)
        {
            velocity = velocity + new Vector3(0, -PhysicsConstants.gravity, 0) * Time.deltaTime;
            float scaledDrag = PhysicsConstants.drag * Time.deltaTime;
            velocity = new Vector3(velocity.x - scaledDrag, velocity.y - scaledDrag, velocity.z - scaledDrag);
            body.velocity = velocity;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Bounce")
        {
            var normal = collision.GetContact(0).normal;
            var reflected = Vector3.Reflect(velocity, normal);
            var coef = 0.1f;// PhysicsConstants.smallBounceCoef;
            velocity = new Vector3(reflected.x * coef, reflected.y * coef, reflected.z * coef);
        }
    }
}
