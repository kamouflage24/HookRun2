
using UnityEngine;

public class Bobber : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 previousPosition;
    private void Awake(){
        rb = GetComponent<Rigidbody>();
        previousPosition = transform.position;
    }
    private void FixedUpdate(){
        if(rb == null){
            return;
        }
        Vector3 currentPosition = rb.position;
        Vector3 movement = currentPosition - previousPosition;
        if(movement.sqrMagnitude > 0f){
            RaycastHit[] hits = Physics.RaycastAll(
                previousPosition, movement.normalized,
                movement.magnitude, Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore);

                RaycastHit closestHit = default;
                bool foundHit = false;
                foreach (RaycastHit hit in hits)
            {
                if(hit.rigidbody == rb){
                    continue;
                }
                if(!foundHit || hit.distance < closestHit.distance)
                {
                    closestHit = hit;
                    foundHit = true;
                }
            }

            if(foundHit){
                rb.position = closestHit.point + closestHit.normal * 0.02f;
                rb.linearVelocity = Vector3.zero;
                previousPosition = rb.position;
                return;
            }

        }
        previousPosition = currentPosition;
    }
}
