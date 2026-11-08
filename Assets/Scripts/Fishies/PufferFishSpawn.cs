using System.Collections;

using System.Runtime.CompilerServices;
using UnityEngine;


public class PufferFishSpawn : MonoBehaviour
{
    public GameObject pufferFishPrefab;
    public GameObject[] angyPuffIters;
    public Transform spawnPoint;

    public float spawnInterval = 5f;
    public float spawnChance = 0.35f;
    public static bool isSpawned;

    private float timer;
    private int currentIteration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (isSpawned)
        {
            return;
        }
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
        if(!isSpawned){
            GameObject prefab = GetCurrentPrefab();
            if(prefab == null || spawnPoint == null)
            {
                return;
            }
    
        GameObject spawnedPufferFish = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        PufferFish pufferFish = spawnedPufferFish.GetComponent<PufferFish>();
        if(pufferFish != null)
            {
                Renderer fishRenderer = spawnedPufferFish.GetComponentInChildren<Renderer>();
                Material iterationMaterial = fishRenderer != null ? fishRenderer.sharedMaterial : null;
                pufferFish.SetSpawner(this, currentIteration, iterationMaterial);
            }
        isSpawned = true;
        }
    }

    public void AdvanceIteration()
    {
        if(angyPuffIters != null && currentIteration < angyPuffIters.Length)
        {
            currentIteration++;
            Debug.Log($"Puff is getting Angy");
        }
    }

    private GameObject GetCurrentPrefab()
    {
        if(currentIteration == 0)
        {
            return pufferFishPrefab;
        }
        return angyPuffIters[currentIteration - 1];
    }
    
}
