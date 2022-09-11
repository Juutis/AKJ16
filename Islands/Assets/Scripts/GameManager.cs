using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    [SerializeField]
    private List<Island> islands;
    [SerializeField]
    private ObjectLauncher currentCannon;
    private Island currentIsland;
    private LaunchableObject flyingObject;

    private void Awake()
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentIsland = islands[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCannon(ObjectLauncher cannon)
    {
        currentCannon.gameObject.SetActive(false);
        currentCannon = cannon;
        cannon.gameObject.SetActive(true);
        // DELETE Zarguuf
        // Call current ObjectLauncher reset
        GameObject.Destroy(this.flyingObject.gameObject);
        currentCannon.Reset();
    }

    public void SetCurrentIsland(Island island)
    {
        currentIsland = island;
        if (currentIsland == islands.Last())
        {
            // Game over
            Debug.Log("GAME OVER!");
        }
    }

    public void SetFlyingObject(LaunchableObject flyingObject)
    {
        this.flyingObject = flyingObject;
    }

    public Vector3 GetWind()
    {
        return this.currentIsland.GetWind();
    }
}
