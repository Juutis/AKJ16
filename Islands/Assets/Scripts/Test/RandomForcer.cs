using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForcer : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("Boom", Random.Range(0.5f, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Boom() {
        rb.AddForce(transform.up * Random.Range(5.0f, 50.0f), ForceMode.VelocityChange);
        Invoke("Boom", Random.Range(1.0f, 3.0f));
    }
}
