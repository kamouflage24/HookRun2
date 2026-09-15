
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QTE : MonoBehaviour
{
    public Slider qTSlider;
    public TMP_Text keyToPress;
    public bool IsActive{ get; private set; }

    public bool IsFinished{ get; private set; }
    public bool Succeeded{ get; private set; }
    private bool freeze;
    public bool rapidPress;
    public int decreaseSpeed;
    public float pressIncrease = 1f;
    private KeyCode key;
    private readonly KeyCode[] availableOptions = { KeyCode.Q, KeyCode.E};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       IsActive = false;
        
    }
    public void BeginQTE(){
        IsFinished = false;
        Succeeded = false;
        IsActive = true;
        freeze = false;
        
        int rand = Random.Range(0, 2);
        key = availableOptions[Random.Range(0, availableOptions.Length)];
        keyToPress.text = key.ToString();
        qTSlider.value = rapidPress ? 5f : 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsActive)
        {
            return;
        }
    
       
            qTSlider.value = Mathf.MoveTowards(qTSlider.value, 0, decreaseSpeed * Time.deltaTime);
        

            if(Input.GetKeyDown(key))
            {
                qTSlider.value = Mathf.Clamp(qTSlider.value + pressIncrease, 0f, 10f);
                if(qTSlider.value >= 10f)
                {
                    keyToPress.text = "FISH ON!!!";
                    FinishQTE(true);
                }
            }
            
            if(qTSlider.value <= 0f)
            {
                keyToPress.text = "Tis to be a skill issue!!!";
                FinishQTE(false);
            }
    }
    private void FinishQTE(bool succeeded){
        Succeeded = succeeded;
        IsFinished = true;
        IsActive = false;
        freeze = true;
    }
}
