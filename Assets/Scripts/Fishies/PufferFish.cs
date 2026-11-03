using UnityEngine;


public class PufferFish : MonoBehaviour
{
    
    [Header("Target")]
    [SerializeField] private Transform Boat;

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
    public bool OnBoat {get; private set;} = false;

    public GameObject pufferFishPost;
  
    private Rigidbody rb;
    

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
        rb = GetComponent<Rigidbody>();
        if(Boat == null){
            Boat = GameObject.FindGameObjectWithTag("Boat")?.transform;
        }
        PufferFishPost post = FindObjectOfType<PufferFishPost>();
        if(post != null)
        {
            pufferFishPost = post.gameObject;
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
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        HandleContact(other);
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
        Debug.Log("puffer on boat!");
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

}
