using System.Collections;

using System.Runtime.CompilerServices;
using UnityEngine;


public class SharkSpawner : MonoBehaviour
{
    public GameObject sharkPrefab;
    public Transform spawnPoint;
    public SharkIndicator indicator;

    [SerializeField] private Transform player;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float spawnDistance = 80f;

    public float spawnInterval = 5f;
    public float spawnChance = 0.35f;

    private float timer;
    

    private void Awake()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Boat")?.transform;
        }
        if(playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
    }
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
        Vector3 forward = playerCamera.forward;
        forward.y = 0f;
        if(forward.sqrMagnitude <= 0.01f)
        {
            forward = player.right;
            forward.y = 0f;
        }
        forward.Normalize();

        Vector3 spawnPosition = player.position + forward * spawnDistance;
        spawnPosition.y = spawnPoint != null ? spawnPoint.position.y : player.position.y;
        Quaternion spawnRotation = Quaternion.LookRotation(-forward);
        GameObject spawnedShark = Instantiate(sharkPrefab, spawnPosition, spawnRotation);
        // if(indicator != null)
        // {
        //     indicator.SetTarget(spawnedShark.transform);
        // }
    }
    public void JumpScare(Transform cameraTransform, Vector3 spawnLevelPosition, float distance)
    {
        
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        if(forward.sqrMagnitude <= 0.01f)
        {
            forward = Vector3.forward;
        }
        forward.Normalize();

        Vector3 spawnPosition = cameraTransform.position + forward * distance;
        spawnPosition.y = spawnLevelPosition.y;
        Quaternion spawnRotation = Quaternion.LookRotation(-forward);
        GameObject SpawnedShark = Instantiate(sharkPrefab, spawnPosition, spawnRotation);
        Shark shark = SpawnedShark.GetComponent<Shark>();
        shark.chaseSpeed *= 1.4f;
        
    }
    
}
