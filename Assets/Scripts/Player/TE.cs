
using UnityEngine;
using UnityEngine.UI;

public class TE : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform Needle;
    public RectTransform Bar;
    public RectTransform Target;
    [Header("Settings")]
    public float speed = 300f;

    private float barWidth;
    private float direction = 1f;
    private bool isActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barWidth = Bar.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive) return;

        MoveIndicator();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EvaluateSkillCheck();
        }
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
    void EvaluateSkillCheck()
    {
        isActive = false;
        float targetMin = Target.anchoredPosition.x - (Target.rect.width / 2f);
        float targetMax = Target.anchoredPosition.x + (Target.rect.width / 2f);
        float indicatorPos = Needle.anchoredPosition.x;

        if(indicatorPos >= targetMin && indicatorPos <= targetMax)
        {
            Debug.Log("Great!!");
        }
        else
        {
            Debug.Log("Fail!!");
        }
    }
}
