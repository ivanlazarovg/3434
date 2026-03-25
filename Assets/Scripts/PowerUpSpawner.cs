using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public static PowerUpSpawner Instance;

    [SerializeField] List<GameObject> possiblePowerUps;

    int numberOfPickups;
    [SerializeField] int maxNumberOfPickups;

    [SerializeField] float timeBetweenPickupSpawns;
    float currentTimeTillNextPickup;

    Dictionary<Transform, GameObject> possibleSpawnLocations;

    void Start()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;
        

        numberOfPickups = 0;
        List<Transform> transforms = transform.GetComponentsInChildren<Transform>().ToList();
        transforms.Remove(transform);
        possibleSpawnLocations = new Dictionary<Transform, GameObject>();
        foreach (var t in transforms)
        {
            possibleSpawnLocations.Add(t, null);
        }

        currentTimeTillNextPickup = timeBetweenPickupSpawns;
    }

    private void Update()
    {
        currentTimeTillNextPickup -= Time.deltaTime;

        if (currentTimeTillNextPickup <= 0 && numberOfPickups < maxNumberOfPickups)
        {
            SpawnPickup();
            currentTimeTillNextPickup = timeBetweenPickupSpawns;
        }
    }

    void SpawnPickup() 
    {
        numberOfPickups++;
        KeyValuePair<Transform, GameObject>[] kvps = possibleSpawnLocations.Where(x => x.Value == null).ToArray();
        Transform currentPos = kvps[Random.Range(0, kvps.Length)].Key;
        possibleSpawnLocations[currentPos] = Instantiate(possiblePowerUps[Random.Range(0, possiblePowerUps.Count)], currentPos.position, Quaternion.identity);
    }

    public void RemovePickup(GameObject toBeRemoved) 
    {
        numberOfPickups--;
        Debug.Log(numberOfPickups);
        Debug.Log(toBeRemoved.name);
        Transform trans = possibleSpawnLocations.Where(x => x.Value == toBeRemoved).First().Key;
        possibleSpawnLocations[trans] = null;
    }
}
