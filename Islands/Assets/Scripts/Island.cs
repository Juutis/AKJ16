using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    [SerializeField]
    private ObjectLauncher cannon;
    [SerializeField]
    private List<GameObject> hideWhenActive;
    [SerializeField]
    private Wind wind;

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
        hideWhenActive.ForEach(t => t.SetActive(false));
        GameManager.main.SetCurrentIsland(this);
        GameManager.main.ChangeCannon(cannon);
    }

    public Vector3 GetWind()
    {
        return wind.transform.forward * wind.Magnitude;
    }
}
