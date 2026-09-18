using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SharkSpawner : MonoBehaviour
{
    public GameObject sharkPrefab;
    public Transform spawnPoint;

    public float spawnInterval = 5f;
    public float spawnChance = 0.35f;

    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnInterval)
        {
            timer = 0f;
            PreSpawnShark();
        }
    }
    void PreSpawnShark()
    {
        float roll = Random.value;
        if(roll <= spawnChance)
        {
            SpawnShark();
        }
    }
    void SpawnShark()
    {
        Instantiate(sharkPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    
}
