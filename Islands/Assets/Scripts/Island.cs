using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    [SerializeField]
    private GameObject cannon;
    [SerializeField]
    private List<GameObject> hideWhenActive;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in transform)
        {
            if (t.TryGetComponent<IslandCollider>(out IslandCollider collider))
            {
                collider.SetIsland(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateIsland()
    {
        cannon.SetActive(true);
        hideWhenActive.ForEach(t => t.SetActive(false));
        GameManager.main.SetCurrentIsland(this);
        GameManager.main.ChangeCannon(cannon);
    }
}
