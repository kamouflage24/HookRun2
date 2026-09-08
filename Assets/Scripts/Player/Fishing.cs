

using UnityEngine;

public class Fishing : MonoBehaviour
{
    public Transform fishingArea;
    public GameObject BobberPrefab;
    public LineRenderer castIndicator;
    
    public float maxCastDistance = 20f;
    public float MaxChargeTime = 2f;

    private float chargeTimer = 0f;
    private bool isCharging = false;

    private GameObject activeBobber;
    // Update is called once per frame

    void Start(){
        if(castIndicator != null){
            castIndicator.positionCount = 2;
            castIndicator.useWorldSpace = true;
            castIndicator.enabled = false;
        }
    }
    void Update()
    {
        HandleCasting();
        if(!isCharging && activeBobber != null && castIndicator != null){
            castIndicator.SetPosition(0, transform.position);
            castIndicator.SetPosition(1, activeBobber.transform.position);
        }
    }
    void UpdateCastIndicator(float castDistance){
        if(castIndicator == null || fishingArea == null)
        {
            return;
        }
        Vector3 start = transform.position;
        Vector3 direction = (fishingArea.position - start).normalized;
        castIndicator.SetPosition(0, start);
        castIndicator.SetPosition(1, start + direction * castDistance);

    }
    void HandleCasting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            chargeTimer = 0f;
            if(castIndicator != null){
                castIndicator.enabled = true;
            }
        }
        if(isCharging && Input.GetKey(KeyCode.Space))
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0f, MaxChargeTime);
            UpdateCastIndicator(chargeTimer / MaxChargeTime * maxCastDistance);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isCharging = false;
            CastLine();
        }
    }
    void CastLine()
    {
        float chargePercent = chargeTimer / MaxChargeTime;
        float castDistance = chargePercent * maxCastDistance;
        GameObject bobber = Instantiate(BobberPrefab, transform.position, Quaternion.identity);
        activeBobber = bobber;
        if(castIndicator != null)
        {
            castIndicator.enabled = true;
        }
        Vector3 direction = (fishingArea.position - transform.position).normalized;
        Vector3 targetPos = transform.position + direction * castDistance;
        Rigidbody rb = bobber.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Vector3 force = direction * castDistance * 5f;
            rb.AddForce(force, ForceMode.Impulse);
        }
        else
        {
            bobber.transform.position = targetPos;
        }
    }
}
