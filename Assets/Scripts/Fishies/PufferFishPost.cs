using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PufferFishPost : MonoBehaviour
{
    
    public List<Fish> storedFish = new List<Fish>();
    [SerializeField] private TMP_Text puffTalk;
    public TMP_Text PuffTalk
    {
        get
        {
            if(puffTalk == null)
            {
                TMP_Text[] childTexts = GetComponentsInChildren<TMP_Text>(true);
                foreach(TMP_Text childText in childTexts)
                {
                    if(childText.name == "PuffTalk")
                    {
                        puffTalk = childText;
                        break;
                    }
                }
            }
            return puffTalk;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
