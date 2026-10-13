using UnityEngine;
using UnityEngine.UI;

public class SharkIndicator : MonoBehaviour
{
    public Transform shark;
    public Transform player;
    public RectTransform ArrowUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {
        if(shark != null && player != null)return;
        Vector3 direction = shark.position - player.position;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        ArrowUI.rotation = Quaternion.Euler(0f, 0f, -angle);
    }
}
