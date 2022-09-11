using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlock : MonoBehaviour
{
    [SerializeField]
    float speed = 90;

    // Start is called before the first frame update
    void Start()
    {
        var anims = GetComponentsInChildren<Animator>();
        foreach (var anim in anims) {
            anim.Play(0, -1, Random.value);
        }
        var birds = GetComponentsInChildren<Bird>();
        foreach (var bird in birds) {
            bird.enabled = false;
            bird.GetComponent<Rigidbody>().velocity = Vector3.zero;
            bird.GetComponent<Rigidbody>().isKinematic = true;
            var dir = bird.transform.position - transform.position;
            dir = Vector3.Cross(dir, Vector3.up);
            bird.transform.forward = dir;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.down, speed * Time.deltaTime);
    }
}
