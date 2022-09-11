using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject fishPrefab;
    private Vector2 fishSpawnTimeRange = new Vector2(0.5f, 6f);
    private float secondsToNextFish = 3f;
    private float lastFishTime = 0f;

    private List<GameObject> fishPool;
    private int fishPoolSize = 5;
    private int fishIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        fishPool = new List<GameObject>();
        for (int i = 0; i < fishPoolSize; i++)
        {
            GameObject fish = Instantiate(fishPrefab);
            fish.transform.position = new Vector3(-1000, -1000, -1000);
            fishPool.Add(fish);
        }
        secondsToNextFish = Random.Range(fishSpawnTimeRange.x, fishSpawnTimeRange.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastFishTime > secondsToNextFish)
        {
            GameObject fish = fishPool[fishIndex];
            fish.transform.position = transform.position;
            fishIndex = (fishIndex + 1) % fishPool.Count;
            var fishBody = fish.GetComponent<Rigidbody>();
            fishBody.velocity = Vector3.zero;

            Vector2 dir2 = Random.insideUnitCircle.normalized;
            Vector3 dir = new Vector3(dir2.x, 0, dir2.y);
            float forceStrength = Random.Range(12f, 18f);
            fishBody.AddForce((Vector3.up + dir * 0.5f).normalized * forceStrength, ForceMode.Impulse);
            lastFishTime = Time.time;
            secondsToNextFish = Random.Range(fishSpawnTimeRange.x, fishSpawnTimeRange.y);
        }
    }
}
