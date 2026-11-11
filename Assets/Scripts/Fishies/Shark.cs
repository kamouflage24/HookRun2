using System.Collections;
using UnityEngine;
using DG.Tweening;


public class Shark : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform Boat;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 30f;

    [Header("Chase")]
     public float chaseSpeed = 20f;
    [SerializeField] private float chaseDuration = 10f;

    [Header("CoolDown")]
    [SerializeField] private float chaseCooldown = 5f;
    private bool isChasing = false;
    private float chaseTimer = 0f;
    private float cooldownTimer = 0f;

    [Header("Chase Music")]

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip SharkChaseSfx;

    [Header("Shark Rising")]
    [SerializeField] private float underwateroffset = -2f;
    [SerializeField] private float abovewaterOffset = 70f;
    [SerializeField] private float riseDistance = 40f;
    [SerializeField] private float riseDuration = 5f;

    private float waterLevel;
    private Tween riseTween;
    private float currentRiseAmount = 4f;
    private float lastTargetHeight;


    private Rigidbody rb;

    private void Start(){
        
        waterLevel = transform.position.y;
        currentRiseAmount = underwateroffset;
        lastTargetHeight = underwateroffset;
        transform.position = new Vector3(transform.position.x, 
            waterLevel + currentRiseAmount, 
            transform.position.z);
            
       StartChasing();
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (!isChasing)
        {
            DetectPlayer();
            return;
        }
        
        chaseTimer -= Time.deltaTime;

        if(chaseTimer <= 0f)
        {
            StopChasing();
        }
    }

    private void FixedUpdate()
    {
        if(isChasing)
            ChasePlayer();
    }

    private void UpdateSharkHeight()
    {
        if (Boat == null)  
            return;
            Vector3 offset = Boat.position - transform.position;
            offset.y = 0f;
        
        float distance = offset.magnitude;

        float normalizedDistance = Mathf.InverseLerp(detectionRange, riseDistance, distance);

        float targetHeight = Mathf.Lerp(underwateroffset, abovewaterOffset, normalizedDistance);

        if(Mathf.Abs(targetHeight - lastTargetHeight) < 0.1f)
            return;
        
        lastTargetHeight = targetHeight;

        riseTween?.Kill();

        riseTween = DOTween.To(() => currentRiseAmount,
            value =>
            currentRiseAmount = value, 
            targetHeight, riseDuration).SetEase(Ease.OutSine);
    }
    private void Awake(){
        rb = GetComponent<Rigidbody>();
        if(Boat == null){
            Boat = GameObject.FindGameObjectWithTag("Boat")?.transform;
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
        if(audioSource != null && SharkChaseSfx != null)
            {
                audioSource.PlayOneShot(SharkChaseSfx);
            }
        Debug.Log("shark be comin");
    }
    
    private void ChasePlayer()
    {
        if(Boat == null)
        {
            StopChasing();
            return;
        }
        
        UpdateSharkHeight();
        
        Vector3 direction = Boat.position - rb.position;
        direction.y = 0f;
        Vector3 newPos = rb.position;
        newPos.y = waterLevel + currentRiseAmount;
        if(direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();

            newPos += direction * chaseSpeed * Time.fixedDeltaTime;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 5f * Time.fixedDeltaTime));
        }

        rb.MovePosition(newPos);
    }
    private void StopChasing()
    {
        isChasing = false;
        cooldownTimer = chaseCooldown;
        Debug.Log("Shark stopped chasing!");
        StartCoroutine(DespawnAfterDelay());
    }
    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
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
    private void OnTriggerEnter(Collider other)
    {
        if(IsBoatCollision(other))
        {
            GameOver.TriggerGameOver("You Have Died");
        }
    }
}
