
using UnityEngine;

public class FishingArea : MonoBehaviour
{
    public GameObject[] fishPrefabs;
    public int fishCount = 10;
    public Vector3 areaSize = new Vector3(10, 5, 10);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnFish();
    }
    void SpawnFish()
    {
        for (int i = 0; i < fishCount; i++)
        {
            Vector3 randomPos = GetRandomPointInArea();
            GameObject prefab = GetRandomFishPrefab();
            if(prefab != null){
                Instantiate(prefab, randomPos, Quaternion.identity);
            }
        }
    }
    public GameObject GetRandomFishPrefab(){
        if(fishPrefabs == null || fishPrefabs.Length == 0){
            return null;
        }
        return fishPrefabs[Random.Range(0, fishPrefabs.Length)];
    }
    Vector3 GetRandomPointInArea()
    {
        return transform.position + new Vector3(Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
        Random.Range(-areaSize.y / 2f, areaSize.y / 2f), Random.Range(-areaSize.z / 2f, areaSize.z / 2f));
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
