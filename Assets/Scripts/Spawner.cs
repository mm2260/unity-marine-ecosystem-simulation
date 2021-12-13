using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public Vector3 center;
    public Vector3 size;

    public GameObject PlantPrefab;
    public GameObject FishPrefab;

    [SerializeField] private float FoodSpawnInterval = 15;
    [SerializeField] private float FishSpawnInterval = 30;
    [SerializeField] private float FoodSpawnIterator;
    [SerializeField] private float FishSpawnIterator;

    [SerializeField] private Plant[] plants;

    private void Start()
    {
        SpawnFood();
        SpawnFish();
    }

    private void Update()
    {
        plants = FindObjectsOfType<Plant>();


        if(FoodSpawnIterator >= FoodSpawnInterval && plants.Length < 200 )
        {
            SpawnFood();
            FoodSpawnIterator = 0;
        }
        if(FishSpawnIterator >= FishSpawnInterval)
        {
            SpawnFish();
            FishSpawnIterator = 0;
        }

        FishSpawnIterator += Time.deltaTime;
        FoodSpawnIterator += Time.deltaTime;
    }


    //----------------------------------------------------------------


    public void SpawnFood()
    {
        int i = Random.Range(1, 15);
        Vector3 pos;

        for (int j = 0; j < i; j++)
        {
            pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), (-size.y / 2), Random.Range(-size.z / 2, size.z / 2));

            Instantiate(PlantPrefab, pos, Quaternion.identity);
        }

        Debug.Log(i + " Plants Spawned");

    }

    public void SpawnFish()
    {
        int i = Random.Range(1, 10);
        Vector3 pos;

        for (int j = 0; j < i; j++)
        {
            pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));

            Instantiate(FishPrefab, pos, Quaternion.identity);
        }

        Debug.Log(i + " Fishes Spawned");
        
    }

    //----------------------------------------------------------------


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(center, size);
    }
}
