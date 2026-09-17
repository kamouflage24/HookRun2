
using System.Net;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data.Common;





public class Fishing : MonoBehaviour
{
    [Header("fishing elements")]
    public Transform fishingArea;
    public Transform caughtFishPoint;
    public GameObject BobberPrefab;
    public LineRenderer castIndicator;
    public Camera playerCamera;
    public BoatInventory boatInventory;
    public Transform boatDropPoint;
    public Transform oceanDropPoint;
   
    
    public float maxCastDistance = 20f;
    public float MaxChargeTime = 2f;
    public float minBiteDelay = 2f;
    public float maxBiteDelay = 5f;
    public float interactionDistance = 5f;

    private float chargeTimer = 0f;
    private bool isCharging = false;
    private bool canCast = true;
    private bool isReeling = false;
    private bool catchResolved = false;
    private bool bobberLanded = false;
    private bool biteReady = false;

    [Header("UI Elements")]
    public GameObject fishingUI;
    public RectTransform Needle;
    public RectTransform Bar;
    public RectTransform Target;
    [SerializeField] private TMP_Text text;

    [Header ("Choice")]
    [SerializeField] private GameObject catchChoiceUI;
    [SerializeField] private Button storeFishButton;
    [SerializeField] private Button releaseFishButton;


    [Header("Settings")]
    public float speed = 300f;

    private float barWidth;
    private float direction = 1f;
    private bool isActive = true;



    private GameObject activeBobber;
    private GameObject caughtFish;
    // Update is called once per frame

    void Start(){
        if(boatInventory == null && boatDropPoint != null){
            boatInventory = boatDropPoint.GetComponentInParent<BoatInventory>();

        }
        if(boatInventory == null)
        {
            boatInventory = FindObjectOfType<BoatInventory>();
        }

        if(castIndicator != null){
            castIndicator.positionCount = 2;
            castIndicator.useWorldSpace = true;
            castIndicator.enabled = false;
        }
        if (fishingUI != null){
            fishingUI.SetActive(false);
        }
        if (catchChoiceUI != null)
        {
            catchChoiceUI.SetActive(false);
        }
        if(storeFishButton != null){
            storeFishButton.onClick.AddListener(DropCaughtFish);
        }
        if(releaseFishButton != null){
            releaseFishButton.onClick.AddListener(ReleaseCaughtFish);
        }
        if(Bar != null){
            barWidth = Bar.rect.width;
        }
        text.color = Color.green;
        text.fontSize = 36f;
        text.fontStyle = FontStyles.Bold;
       
    }
    void Update()
    {
        HandleCasting();
        if(!isCharging && activeBobber != null && castIndicator != null){
            castIndicator.SetPosition(0, transform.position);
            Vector3 offset = activeBobber.transform.position - transform.position;

            castIndicator.SetPosition(1, activeBobber.transform.position);
        }
        if(biteReady){
   
            MoveIndicator();
            
            if (Input.GetKeyDown(KeyCode.R)){
                bool caught = EvaluateSkillCheck();
                if(caught){
                    ResolveCatch();
                    Reeling();
                    ShowCatchChoices();
                }
                else{
                Reeling();
                canCast = true;
                }
            
            }
        } if(caughtFish != null && Input.GetKeyDown(KeyCode.Q))
        {
            HandleFish();
        }
        
    }
    void UpdateCastIndicator(float castDistance){
        if(castIndicator == null || fishingArea == null)
        {
            return;
        }
        
        Vector3 start = transform.position;
        Vector3 direction = playerCamera.transform.forward;
        castIndicator.SetPosition(0, start);
        castIndicator.SetPosition(1, start + direction * castDistance);

    }
    void HandleCasting()
    {
        if(!canCast) return;
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
            canCast = false;
        }
    }
    void CastLine()
    {
        catchResolved = false;
        bobberLanded = false;
        biteReady = false;

        float chargePercent = chargeTimer / MaxChargeTime;
        float castDistance = chargePercent * maxCastDistance;
        GameObject bobber = Instantiate(BobberPrefab, transform.position, Quaternion.identity);
        activeBobber = bobber;
        Bobber bobberScript = bobber.GetComponent<Bobber>();
        if(bobberScript != null){
            bobberScript.Landed += HandleBobberLanded;
        }
        if(castIndicator != null)
        {
            castIndicator.enabled = true;
        }
        Vector3 direction = playerCamera.transform.forward;
        Vector3 targetPos = transform.position + direction * castDistance;
        Rigidbody rb = bobber.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Vector3 force = direction * castDistance * chargeTimer;
            rb.AddForce(force, ForceMode.Impulse);
        }
        else
        {
            bobber.transform.position = targetPos;
        }
    }
    public void DropCaughtFish()
    {
        if(caughtFish == null) return;

        Fish fish = caughtFish.GetComponent<Fish>();
        if(fish != null && boatInventory != null)
            boatInventory.AddFish(fish);
        if(boatDropPoint != null){
        caughtFish.transform.SetParent(boatDropPoint, false);
        caughtFish.transform.localPosition = Vector3.zero;
        caughtFish.transform.localRotation = Quaternion.identity;
        }

        caughtFish = null;
        FinishCatchChoice();
    }
    public void ReleaseCaughtFish()
    {
        if(caughtFish != null)
        {
            Transform releasePoint = oceanDropPoint != null ? oceanDropPoint : fishingArea;
            caughtFish.transform.SetParent(null);
            if(releasePoint != null)
            {
                caughtFish.transform.position = releasePoint.position;
            }
            caughtFish = null;
            FinishCatchChoice();
        }
    }
    private void HandleFish()
    {
        if(playerCamera == null || caughtFish == null) 
            return;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if(!Physics.Raycast(ray, out RaycastHit hit, interactionDistance)) 
            return;
        
        
            if (hit.collider != null && hit.collider.CompareTag("Boat"))
            {
                DropCaughtFish();
                Debug.Log("boat!!");
                return;
            }
            if (hit.collider != null && hit.collider.CompareTag("Water"))
            {
                ReleaseCaughtFish();
                Debug.Log("water!!");
            }
        
    }
    private void ShowCatchChoices()
    {
        if(catchChoiceUI != null)
        {
            catchChoiceUI.SetActive(false);
        }
    }
    private void FinishCatchChoice()
    {
        if(catchChoiceUI != null)
        {
            catchChoiceUI.SetActive(true);
        }
        canCast = true;
    }

    private void HandleBobberLanded(Collider hitCollider){
        if(bobberLanded || hitCollider == null || hitCollider.gameObject.name != "Water"){
            return;
        }
        bobberLanded = true;
        isActive = true;

        
        StartCoroutine(StartBite());
        Debug.Log("Hit da water!!");
    }
    private IEnumerator StartBite(){
        float delay = Random.Range(minBiteDelay, maxBiteDelay);
        yield return new WaitForSeconds(delay);
        if(activeBobber == null){
            yield break;
        }
        biteReady = true;
        isActive = true;
         if(fishingUI != null){
            fishingUI.SetActive(true);
         }
    }
    void Reeling(){
        Bobber bobberScript = activeBobber != null ? activeBobber.GetComponent<Bobber>() : null;
        if(bobberScript != null){
            bobberScript.Landed -= HandleBobberLanded;
        }
        if(activeBobber != null)
            Destroy(activeBobber);

        activeBobber = null;
        canCast = false;
        isReeling = false;
        bobberLanded = false;
        biteReady = false;

        if(castIndicator != null)
            castIndicator.enabled = false;
        if(fishingUI != null)
            fishingUI.SetActive(false);
    }

    private void ResolveCatch()
    {
        if (catchResolved)
        {
            return;
        }
        catchResolved = true;
        text.text = "GID GUD!!!";

        FishingArea area = fishingArea != null ? fishingArea.GetComponent<FishingArea>() : null;
        GameObject fishPrefab = area != null ? area.GetRandomFishPrefab() : null;
        if(fishPrefab == null || caughtFishPoint == null){
            return;
        }
        if(caughtFish != null)
        {
            Destroy(caughtFish);
        }
        caughtFish = Instantiate(fishPrefab, caughtFishPoint.position, caughtFishPoint.rotation);
        caughtFish.transform.SetParent(caughtFishPoint, false);
        caughtFish.transform.localPosition = Vector3.zero;
        caughtFish.transform.localRotation = Quaternion.identity;
        
    }
    void MoveIndicator()
    {
        Needle.anchoredPosition += new Vector2(direction * speed * Time.deltaTime, 0);

        float halfWidth = barWidth / 6f;
        if(Needle.anchoredPosition.x >= halfWidth)
        {
            direction = -1f;
        }
        else if (Needle.anchoredPosition.x <= -halfWidth)
        {
            direction = 1f;
        }
    }
    bool EvaluateSkillCheck()
    {
        isActive = false;
        float targetMin = Target.anchoredPosition.x - (Target.rect.width / 2f);
        float targetMax = Target.anchoredPosition.x + (Target.rect.width / 2f);
        float indicatorPos = Needle.anchoredPosition.x;

        if(indicatorPos >= targetMin && indicatorPos <= targetMax)
        {
            text.text = "FISH ON!!!";
            return true;
        }
        else
        {
            Debug.Log("Fail!!");
            return false;
        }
    }
}


