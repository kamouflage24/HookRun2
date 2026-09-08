using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BoatMovement : MonoBehaviour
{
   [Header("Movement")]
   public float moveSpeed;
   public Transform orientation;
   public float turnSpeed = 8f;

   float horizontalInput;
   float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = 2f;
        rb.angularDamping = 4f;
    }
    private void Update(){
        MyInput();
        SpeedControl();
    }
    private void FixedUpdate(){
        MovePlayer();
    }
    private void MyInput(){
        horizontalInput = -Input.GetAxisRaw("Horizontal");
        verticalInput = -Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer(){
        moveDirection = transform.right * verticalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        float turnDirection = horizontalInput * verticalInput;
        Quaternion turnRotation = Quaternion.Euler(0f, turnDirection * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
       


    }
    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        //limit velocity
        if(flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            Vector3 cForce = -flatVel * 0.5f;
            rb.AddForce(cForce, ForceMode.Force);
        }
    }
}
