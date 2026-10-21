using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float remainingTime;
    [SerializeField] private SharkSpawner sharkSpawner;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnDistance = 10f;
    private bool hasSpawnedShark;
    

    // Update is called once per frame
    void Update()
    {
        if(remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
        }
        remainingTime = Mathf.Max(remainingTime, 0f);
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if(remainingTime <= 0f && !hasSpawnedShark)
        {
            hasSpawnedShark = true;
            if(player == null)
            {
                GameObject boat = GameObject.FindGameObjectWithTag("Boat");
                if(boat != null)
                {
                    player = boat.transform;
                }
            }
            if(sharkSpawner != null && player != null)
            {
                sharkSpawner.JumpScare(player, spawnDistance);
            }

        }
    }
    
}
