using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blimp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.gameObject.TryGetComponent<LaunchableObject>(out LaunchableObject launchableObject))
            {
                launchableObject.Bounce(collision.collider.ClosestPoint(collision.GetContact(0).point), collision.GetContact(0).normal);
            }
        }
    }
}
