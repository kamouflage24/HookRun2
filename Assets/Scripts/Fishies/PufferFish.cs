using System.Collections;
using UnityEngine;
using TMPro;




public class PufferFish : MonoBehaviour
{
    
    [Header("Target")]
    [SerializeField] private Transform Boat;
    [SerializeField] private FishingArea fishingArea;
    private Fish needFish;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 30f;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 20f;
    [SerializeField] private float chaseDuration = 10f;

    [SerializeField] private float spinSpeed = 360f;
    private Quaternion chaseFacingRotation;
    private float spinAngle;

    [Header("CoolDown")]
    [SerializeField] private float chaseCooldown = 5f;
    private bool isChasing = false;
    private float chaseTimer = 0f;
    private float cooldownTimer = 0f;

    [Header("Interaction")]

    [SerializeField] private Renderer fishRenderer;
    [SerializeField] private int maxReels = 3;
    [SerializeField] private int currentReels = 0;
    [SerializeField] private int maxAngy = 3;
    [SerializeField] private int currentAngy = 0;

    [SerializeField] private float growthAmount = 0.5f;
    [SerializeField] private float redAmount = 0.15f;

    private Vector3 OGScale;
    private Color ogColor;

    private bool requestActive = false;
    private bool requestSuccess = false;
    public bool OnBoat {get; private set;} = false;

    private TMP_Text puffTalk;

    public GameObject pufferFishPost;
  
    private Rigidbody rb;
    private PufferFishSpawn pufferSpawn;
    

    private void Start()
    {
    }
    // Update is called once per frame
    private void Update()
    {
        if (OnBoat)
        {
            return;
        }
        if(cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (!isChasing)
        {
            DetectPlayer();
            return;
        }

        ChasePlayer();
        chaseTimer -= Time.deltaTime;

        if(chaseTimer <= 0f)
        {
            StopChasing();
        }
        if (OnBoat)
        {
            return;
        }
    }

        private void Awake(){

            OGScale = transform.localScale;
            if(fishRenderer == null)
        {
            fishRenderer = GetComponentInChildren<Renderer>();
        
        }
        if(fishRenderer != null && fishRenderer.sharedMaterial != null)
        {
            ogColor = fishRenderer.sharedMaterial.color;
        }
        
        rb = GetComponent<Rigidbody>();
        if(Boat == null){
            Boat = GameObject.FindGameObjectWithTag("Boat")?.transform;
        }
        PufferFishPost post = FindObjectOfType<PufferFishPost>();
        if(post != null)
        {
            pufferFishPost = post.gameObject;
            puffTalk = post.PuffTalk;
            if(puffTalk == null)
            {
                Debug.LogError("PufferFIshPost has no child");
            }
        }
        else if(pufferFishPost == null || !pufferFishPost.scene.IsValid())
        {
            pufferFishPost = null;
            Debug.LogError("pufferFishPost was not found)");
        }
        
    }
    private void DetectPlayer()
    {
        Boat = GameObject.FindGameObjectWithTag("Boat")?.transform;
        if(Boat == null)
        {
            return;
        }

        if(cooldownTimer > 0f){return;}

        float distance = Vector3.Distance(transform.position, Boat.position);

        if(distance <= detectionRange)
        {
            StartChasing();
        }
    }
    private void StartChasing()
    {
        isChasing = true;
        chaseTimer = chaseDuration;
        chaseFacingRotation = transform.rotation;
        spinAngle = 0f;
        Debug.Log("puffer be comin");
        
    }
       private void ChasePlayer()
    {
        if(Boat == null)
        {
            StopChasing();
            return;
        }
        Vector3 direction = Boat.position - transform.position;
        direction.y = 0f;
        if(direction.sqrMagnitude < 0.01f)
        {
            return;
        }
        direction.Normalize();
        Vector3 newPos = transform.position + direction * chaseSpeed * Time.deltaTime;
        if(rb != null)
        {
            rb.MovePosition(newPos);
        }
        else
        {
            transform.position = newPos;
        }
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        chaseFacingRotation = Quaternion.Slerp(chaseFacingRotation, targetRotation, 5f * Time.deltaTime);
        spinAngle = Mathf.Repeat(spinAngle + spinSpeed * Time.deltaTime, 360f);
        transform.rotation = chaseFacingRotation * Quaternion.AngleAxis(spinAngle, Vector3.forward);
    }
    private void StopChasing()
    {
        isChasing = false;
        cooldownTimer = chaseCooldown;
        Debug.Log("puffer stopped chasing!");
        PufferFishSpawn.isSpawned = false;
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        HandleContact(other);
        if (currentAngy >= maxAngy)
        {
            if(IsBoatCollision(other))
            {
            GameOver.TriggerGameOver("You Have Died");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleContact(collision.collider);
    }

    private void HandleContact(Collider other)
    {
        if(OnBoat || !IsBoatCollision(other))
        {
            return;
        }
        OnBoat = true;
        isChasing = false;
        Transform postTransform = pufferFishPost.transform;
        if(rb != null)
        {
            rb.position = postTransform.position;
            rb.rotation = postTransform.rotation;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        
        transform.SetPositionAndRotation(postTransform.position, postTransform.rotation);
        transform.SetParent(postTransform, true);
        Debug.Log("puffer on boat!");
        NeedFish();
    }
    

    private bool IsBoatCollision(Collider other){
        if(other == null)
        {
            return false;
        }
        if(Boat != null)
        {
            Transform hitTransform = other.transform;
            if(hitTransform == Boat || hitTransform.IsChildOf(Boat))
            {
                return true;
            }
        }
        return other.CompareTag("Boat");
    }
    public void NeedFish()
    {
        if(fishingArea == null)
        {
            fishingArea = FindObjectOfType<FishingArea>();
            
        }
        if(fishingArea == null)
        {
            Debug.LogError("No FishArea");
            return;
        }

        GameObject fishPrefab = fishingArea.GetRandomFishPrefab();
        if(fishPrefab == null)
        {
            return;
        }
        needFish = fishPrefab.GetComponent<Fish>();
        if(needFish == null)
        {
            return;
        }
        currentReels = 0;
        requestActive = true;
        requestSuccess = false;
        if(puffTalk != null)
        {
            string fishName = needFish.gameObject.name.Replace("(Clone)", "").Trim();
            puffTalk.text = $"eh, I need a {needFish.name}! and I need it fresh!";
        }

    }

    public void CorrectCaughtFish(GameObject caughtFish)
    {
        if(!requestActive || requestSuccess)
        {
            return;
        }
        currentReels++;

        if(currentReels >= maxReels)
        {
            currentReels = 0;
            if(currentAngy < maxAngy)
            {
                currentAngy++;
                Angy();
                pufferSpawn?.AdvanceIteration();
            }
            
        }
    }

    private void Angy()
    {
        transform.localScale *= 1f + Mathf.Max(0f, growthAmount);;
        if(fishRenderer != null)
        {
            Color angyColor = fishRenderer.material.color;
            angyColor.r = Mathf.Clamp01(angyColor.r + Mathf.Max(0f, redAmount));
            fishRenderer.material.color = angyColor;
        }
        if(puffTalk != null)
        {
            puffTalk.text = "DO NOT FAIL ME AGAIN!";
        }
        if(currentAngy >= maxAngy)
        {
            if(puffTalk != null)
            {
                puffTalk.text = "Thats It!! Say your finally words!!";
            }
            requestActive = false;
            StartCoroutine(Despawn());
        }
    }

    public void SetSpawner(PufferFishSpawn spawner, int iteration, Material iterationMaterial)
    {
        pufferSpawn = spawner;
        transform.localScale = OGScale * (1f + growthAmount * iteration);
        if(fishRenderer != null && iterationMaterial != null)
        {
            fishRenderer.sharedMaterial = iterationMaterial;
            ogColor = iterationMaterial.color;
        }
    }

    public bool CorrectCatch(GameObject caughtFish)
    {
        if(!OnBoat||!requestActive || requestSuccess)
        {
            return false;
        }
        Fish caughtFishInfo = caughtFish.GetComponent<Fish>();
        if(caughtFishInfo == null)
        {
            return false;
        }
        string requestedFishName = needFish.gameObject.name.Replace("(Clone)", "").Trim();
        string caughtFishName = caughtFishInfo.gameObject.name.Replace("(Clone)", "").Trim();

        if(requestedFishName == caughtFishName)
        {
                CompleteReq();
                return true;
            }
        
            CorrectCaughtFish(caughtFish);
            if(puffTalk != null)
            {
                puffTalk.text = $" NO! I wanted {requestedFishName} not {caughtFishName}!";
            }
            return false;
    }
    

    private void CompleteReq()
    {
        requestSuccess = true;
        requestActive = false;

        if(puffTalk != null)
        {
            puffTalk.text = "thanks for the fish bub!";
        }
        
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(1f);
        PufferFishSpawn.isSpawned = false;
        Destroy(gameObject);

    }
    

    private void puffTalking()
    {
        if(puffTalk == null)
        {
            return;
        }

        OnBoat = !OnBoat;
        if (OnBoat)
        {
            NeedFish();
           
        }
    }
}
