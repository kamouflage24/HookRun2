
using UnityEngine;
using UnityEngine.UI;
public class QTE : MonoBehaviour
{
    public Slider qTSlider;
    public Text keyToPress;

    public bool IsFinished{ get; private set; }
    public bool Succeeded{ get; private set; }
    private bool freeze;
    public bool rapidPress;
    public int decreaseSpeed;
    private KeyCode key;
    private readonly KeyCode[] availableOptions = { KeyCode.Alpha1, KeyCode.Alpha2};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       BeginQTE();
        
    }
    public void BeginQTE(){
        IsFinished = false;
        Succeeded = false;
        freeze = false;
        
        int rand = Random.Range(0, 2);
        key = availableOptions[rand];
        keyToPress.text = availableOptions[rand].ToString();
        if (rapidPress)
        {
            qTSlider.value = 5;
        }
        else
        {
            qTSlider.value = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze)
        {
            qTSlider.value = Mathf.MoveTowards(qTSlider.value, 0, decreaseSpeed * Time.deltaTime);
        }

        if (rapidPress)
        {
            if(Input.GetKeyDown(key) && qTSlider.value > 0)
            {
                qTSlider.value += 1;
                if(qTSlider.value == 10)
                {
                    keyToPress.text = "FISH ON!!!";
                    FinishQTE(true);
                }
            }
            
            if(qTSlider.value == 0)
            {
                keyToPress.text = "Tis to be a skill issue!!!";
                FinishQTE(true);
            }
        }
    }
    private void FinishQTE(bool succeeded){
        Succeeded = succeeded;
        IsFinished = true;
        freeze = true;
    }
}
