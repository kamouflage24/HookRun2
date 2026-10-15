using UnityEngine;
using UnityEngine.UI;

public class SharkIndicator : MonoBehaviour
{
    public Transform shark;
    public Camera Camera;
    public RectTransform arrowUI;
    public float edgePadding = 40f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(arrowUI != null){
        arrowUI.gameObject.SetActive(false);
        }
    }
    public void OnSharkSpawned(Transform spawnedShark)
    {
        shark = spawnedShark;
        if(arrowUI != null){
        arrowUI.gameObject.SetActive(shark != null);
        }    
    }
        public void OnSharkDespawned()
    {
        shark = null;
        if(arrowUI != null){
        arrowUI.gameObject.SetActive(false);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if(arrowUI == null)return;

        if(shark == null)
        {
            arrowUI.gameObject.SetActive(false);
            return;
        }

        if(Camera == null)
        {
            return;
        }

        RectTransform parentRect = arrowUI.parent as RectTransform;
        if (parentRect == null)
        {
            return;
        }
        arrowUI.gameObject.SetActive(true);
        Vector2 screenCenter = Camera.pixelRect.center;
        Vector3 sharkScreenPos = Camera.WorldToScreenPoint(shark.position);
        Vector2 localCenter;
        Vector2 localSharkPos;
        Camera uiCamera = null;
        Canvas canvas = arrowUI.GetComponentInParent<Canvas>();
        if(canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = canvas.worldCamera;
        }

        if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenCenter, uiCamera, out localCenter) || !RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, new Vector2(sharkScreenPos.x, sharkScreenPos.y), uiCamera, out localSharkPos))
        {
            return;
        }

        Vector2 direction = localSharkPos - localCenter;
        if(sharkScreenPos.z > 0f)
        {
            direction = -direction;
        }

        if(direction.sqrMagnitude < 0.001f)
        {
            direction = Vector2.up;
        }

        direction.Normalize();
        Rect rect = parentRect.rect;
        float availableX = direction.x >= 0f ? rect.xMax - localCenter.x - edgePadding : localCenter.x - rect.xMin - edgePadding;
        float availableY = direction.y >= 0f ? rect.yMax - localCenter.y - edgePadding : localCenter.y - rect.yMin - edgePadding;
        float xDistance = Mathf.Abs(direction.x) > 0.001f ? Mathf.Max(0f, availableX) / Mathf.Abs(direction.x) : float.PositiveInfinity;
        float yDistance = Mathf.Abs(direction.y) > 0.001f ? Mathf.Max(0f, availableY) / Mathf.Abs(direction.y) : float.PositiveInfinity;
        float edgeDistance = Mathf.Min(xDistance, yDistance);

        Vector2 arrowPos = localCenter + direction * edgeDistance;
        arrowUI.position = parentRect.TransformPoint(arrowPos);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        arrowUI.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
