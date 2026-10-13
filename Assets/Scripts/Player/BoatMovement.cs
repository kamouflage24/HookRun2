using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;


public class BoatMovement : MonoBehaviour
{
   [Header("Movement")]
   public float moveSpeed;
   public Transform orientation;
   public BoatInventory inventory;
   public float turnSpeed = 8f;

   [Header("Stamina")]
   public float CurrentStamina;
   public float maxStamina = 5f;
   public float staminaRegen = 1f;
   public float staminaDrain = 2f;
   public float sprintMult = 1.8f;
   [SerializeField] private Image StaminaBar;
   private float originalBarWidth;
   private float stamina;
   private bool isSprint;
   float horizontalInput;
   float verticalInput;
   [Header("Weight down")]
   public float CurrentWeight;
   public float maxWeight = 10f;
   public float weightSlowdown = 0.1f;
   public float weightMaxSlowdown = 1f;
   public float weightThreshold = 1f;
   [SerializeField] private Image WeightBar;
   private float originalWeightBarWidth;

    Vector3 moveDirection;

    Rigidbody rb;
    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = 2f;
        rb.angularDamping = 4f;
        if(StaminaBar != null)
        {
            originalBarWidth = StaminaBar.rectTransform.sizeDelta.x;
        }
        
        stamina = Mathf.Max(0f, maxStamina);
        if(StaminaBar == null)
        {
            GameObject staminaBarObject = GameObject.Find("StaminaBar");
            if(staminaBarObject != null)
            {
                StaminaBar = staminaBarObject.GetComponent<Image>();
            }

        }
        if(StaminaBar == null)
        {
            Debug.LogError("StaminaBar is not assigned");
        }
        UpdateStaminaBar();

                if(WeightBar != null)
        {
            originalWeightBarWidth = WeightBar.rectTransform.sizeDelta.x;
        }
        
        stamina = Mathf.Max(0f, maxStamina);
        if(WeightBar == null)
        {
            GameObject WeightBarObject = GameObject.Find("WeightBar");
            if(WeightBarObject != null)
            {
                WeightBar = WeightBarObject.GetComponent<Image>();
            }

        }
        if(WeightBar == null)
        {
            Debug.LogError("WeightBar is not assigned");
        }
        else
        {
            originalWeightBarWidth = WeightBar.rectTransform.sizeDelta.x;
        }
        UpdateWeightBar();
    }
    private void Update(){
        MyInput();
        SpeedControl();
        HandleStamina();
        UpdateWeightBar();
    }
    private void FixedUpdate(){
        MovePlayer();
    }
    private void MyInput(){
        horizontalInput = -Input.GetAxisRaw("Horizontal");
        verticalInput = -Input.GetAxisRaw("Vertical");
        isSprint = Input.GetKey(KeyCode.LeftShift);
    }

    private void MovePlayer(){ 
        float slow = GetWeightSlowdown();
        float speed = moveSpeed;
        if(isSprint && stamina > 0f)
        {
            speed *= sprintMult;
        }
        if (inventory != null && inventory.totalWeight > weightThreshold)
        {
            //ApplyWeightSlowdown();
            speed *= 1f - slow;
        }
        moveDirection = transform.right * verticalInput;
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        float turnDirection = horizontalInput * verticalInput;
        Quaternion turnRotation = Quaternion.Euler(0f, turnDirection * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
       
    }
    float GetWeightSlowdown()
    {
       if(inventory == null) return 0f;
        
        if(inventory.totalWeight < weightThreshold)
        {
            return 0f;
        }
        if(inventory.totalWeight == weightThreshold)
        {
            return 0.7f;
        }

        float extraWeight = inventory.totalWeight - weightThreshold;
        float slow = extraWeight * weightSlowdown;
        return Mathf.Clamp(slow, 0.7f, weightMaxSlowdown);
        }
    void ApplyWeightSlowdown()
    {
        float slowFactor = Mathf.Clamp(inventory.totalWeight * 0.1f, 0f, 0.7f);
        float effectiveSpeed = moveSpeed * (1f - slowFactor);

        if(isSprint && stamina > 0f)
            effectiveSpeed *= sprintMult;

        moveDirection = transform.right * verticalInput;
        rb.AddForce(moveDirection.normalized * effectiveSpeed * 10f, ForceMode.Force);
    }
    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float maxSpeed = isSprint && stamina > 0f ? moveSpeed * sprintMult : moveSpeed;
        maxSpeed *= 1f - GetWeightSlowdown();
        //limit velocity
        if(flatVel.magnitude > maxSpeed){
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            Vector3 cForce = -flatVel * 0.5f;
            rb.AddForce(cForce, ForceMode.Force);
        }
    }
    void HandleStamina()
    {
        float slow = GetWeightSlowdown();
        float effectiveDrain = staminaDrain * (1f + slow);
        float effectiveRegen = staminaRegen * (1f - slow);
        if(isSprint && stamina > 0f)
        {
            stamina -= effectiveDrain * Time.deltaTime;

        }
        else
        {
            stamina += effectiveRegen * Time.deltaTime;

        }
        stamina = Mathf.Clamp(stamina, 0f, Mathf.Max(0f, maxStamina));
        UpdateStaminaBar();
    }
    private void UpdateStaminaBar()
    {
        CurrentStamina = stamina;
        if(StaminaBar != null)
        {
            float normalized = Mathf.Clamp01(CurrentStamina / maxStamina);
            RectTransform rt = StaminaBar.rectTransform;
            rt.sizeDelta = new Vector2(originalBarWidth * normalized, rt.sizeDelta.y);
        }
    }
        private void UpdateWeightBar()
    {
        
        CurrentWeight = inventory != null ? inventory.totalWeight : 0f;
        if(WeightBar != null)
        {
            float normalized = maxWeight > 0f ? Mathf.Clamp01(CurrentWeight / maxWeight) : 0f;
            RectTransform rt = WeightBar.rectTransform;
            rt.sizeDelta = new Vector2(originalWeightBarWidth * normalized, rt.sizeDelta.y);
        }
    }
}
