using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCollider : MonoBehaviour
{
    private Island island;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsland(Island i)
    {
        island = i;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            island.ActivateIsland();
            var player = collision?.gameObject?.GetComponent<LaunchableObject>();
            if (player != null)
            {
                player.HitGround();
            }
        }
    }
}
