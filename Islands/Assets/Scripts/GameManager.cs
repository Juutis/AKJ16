using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    [SerializeField]
    private List<Island> islands;
    [SerializeField]
    private GameObject currentCannon;
    private Island currentIsland;

    private void Awake()
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCannon(GameObject cannon)
    {
        currentCannon.SetActive(false);
        currentCannon = cannon;
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
}
