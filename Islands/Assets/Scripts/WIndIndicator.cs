using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIndIndicator : MonoBehaviour
{
    [SerializeField]
    private Island island;

    [SerializeField]
    private Cloth cloth;

    private float windMultiplier = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate() {
        var wind = island.GetWind();
        cloth.externalAcceleration = wind * windMultiplier;
    }
    
}
