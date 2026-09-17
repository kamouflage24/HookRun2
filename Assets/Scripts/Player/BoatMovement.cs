using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BoatMovement : MonoBehaviour
{
   [Header("Movement")]
   public float moveSpeed;
   public Transform orientation;
   public BoatInventory inventory;
   public float turnSpeed = 8f;

   [Header("Stamina")]
   public float maxStamina = 5f;
   public float staminaRegen = 1f;
   public float staminaDrain = 2f;
   public float sprintMult = 1.8f;
   private float stamina;
   private bool isSprint;
   float horizontalInput;
   float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = 2f;
        rb.angularDamping = 4f;
        
        stamina = maxStamina;
    }
    private void Update(){
        MyInput();
        SpeedControl();
        HandleStamina();
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
        float speed = moveSpeed;
        if(isSprint && stamina > 0f)
        {
            speed *= sprintMult;
        }
        moveDirection = transform.right * verticalInput;
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        moveDirection = transform.right * verticalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        float turnDirection = horizontalInput * verticalInput;
        Quaternion turnRotation = Quaternion.Euler(0f, turnDirection * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
       
    }
    void ApplyWeightSlowdown()
    {
        if(inventory == null) return;
        float slowFactor = Mathf.Clamp(inventory.totalWeight * 0.1f, 0f, 0.7f);
        float effectiveSpeed = moveSpeed * (1f - slowFactor);

        moveDirection = transform.right * verticalInput;
        rb.AddForce(moveDirection.normalized * effectiveSpeed * 10f, ForceMode.Force);
    }
    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float maxSpeed = isSprint && stamina > 0f ? moveSpeed * sprintMult : moveSpeed;
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
        if(isSprint && stamina > 0f && verticalInput != 0f)
        {
            stamina -= staminaDrain * Time.deltaTime;
            if(stamina > maxStamina) 
                stamina = maxStamina;
        }
        else
        {
            stamina += staminaRegen * Time.deltaTime;
            if(stamina > maxStamina) 
                stamina = maxStamina;
        }
    }
}
