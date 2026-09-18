using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Shark : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform Boat;

    [Header("Detection")]
    [SerializeField] private float detectionRange = 30f;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 20f;
    [SerializeField] private float chaseDuration = 10f;

    [Header("CoolDown")]
    [SerializeField] private float chaseCooldown = 5f;
    private bool isChasing = false;
    private float chaseTimer = 0f;
    private float cooldownTimer = 0f;
    private Rigidbody rb;

    private void Start(){
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

        ChasePlayer();
        chaseTimer -= Time.deltaTime;

        if(chaseTimer <= 0f)
        {
            StopChasing();
        }
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
        Debug.Log("shark be comin");
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
        if(direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();

            Vector3 newPos = transform.position + direction * chaseSpeed * Time.deltaTime;
            rb.MovePosition(newPos);
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
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
            Debug.Log("u dieded");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
