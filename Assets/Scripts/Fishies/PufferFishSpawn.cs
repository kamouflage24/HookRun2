using System.Collections;

using System.Runtime.CompilerServices;
using UnityEngine;


public class PufferFishSpawn : MonoBehaviour
{
    public GameObject pufferFishPrefab;
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
            PreSpawnPufferFish();
        }
    }
    void PreSpawnPufferFish()
    {
        float roll = Random.value;
        if(roll <= spawnChance)
        {
            SpawnPufferFish();
        }
    }
    void SpawnPufferFish()
    {
        
        GameObject spawnedPufferFish = Instantiate(pufferFishPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    public void JumpScare(Transform target, float distance)
    {
        
        Vector3 forward = target.forward;
        forward.y = 0f;
        if(forward.sqrMagnitude <= 0.01f)
        {
            forward = Vector3.forward;
        }
        forward.Normalize();

        Vector3 spawnPosition = target.position + forward * distance;
        Quaternion spawnRotation = Quaternion.LookRotation(-forward);
        GameObject spawnedPufferFish = Instantiate(pufferFishPrefab, spawnPosition, spawnRotation);
        
    }
    
}
