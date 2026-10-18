


using UnityEngine;
using UnityEngine.UI;

public class SharkIndicator : MonoBehaviour
{
    [Header("References")]
    public Transform shark;
    public Camera camera;
    public RectTransform ArrowUI;
    [Header("Settings")]
    private float radius = Screen.height * 0.4f;
    private bool hideWhenVisible = true;
    
    public void SetTarget(Transform target)
    {
        shark = target;
    }
    public void ClearTarget()
    {
        shark = null;
        ArrowUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(shark == null)
        {
            ArrowUI.gameObject.SetActive(false);
            return;
        }

        Vector3 screenPos =
            camera.WorldToScreenPoint(shark.position);
        
        bool visible = screenPos.z > 0 &&
            screenPos.x > 0 && screenPos.x < Screen.width &&
            screenPos.y > 0 && screenPos.y < Screen.height;
        
        if(hideWhenVisible && visible)
        {
            ArrowUI.gameObject.SetActive(false);
            return;
        }
        
        ArrowUI.gameObject.SetActive(true);

        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Vector2 direction = ((Vector2)screenPos - screenCenter).normalized;

        if(screenPos.z < 0)
            direction = -direction;
        ArrowUI.anchoredPosition = direction * radius;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        ArrowUI.localRotation = Quaternion.Euler(0,0, angle - 90f);
    }
    
}