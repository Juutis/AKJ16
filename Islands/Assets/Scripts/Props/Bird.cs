using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bird : MonoBehaviour
{
    [SerializeField]
    private Transform center;
    private Rigidbody body;
    private float randomOffsetY;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        randomOffsetY = Random.Range(-Mathf.PI, Mathf.PI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        body.velocity = transform.forward * 60 * Time.deltaTime + transform.up * Mathf.Sin(Time.time * 2f + randomOffsetY) * 15f * Time.deltaTime;
        var pos2 = new Vector3(transform.position.x, 0, transform.position.z);
        var center2 = new Vector3(center.position.x, 0, center.position.z);
        Vector3 lookToward = Quaternion.AngleAxis(-90, Vector3.up) * (pos2 - center2);
        transform.rotation = Quaternion.LookRotation(lookToward, transform.up);
    }
}
