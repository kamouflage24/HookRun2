using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BoatInventory : MonoBehaviour
{

    public float totalWeight;
    public float totalPrice;
    public List<Fish> storedFish = new List<Fish>();
    [SerializeField] private TMP_Text inventoryText;

    private void Start(){
        if(inventoryText == null)
        {
            inventoryText = GetComponentInChildren<TMP_Text>();
            
        }
        RefreshDisplay();
    }

    public void AddFish(Fish fish)
    {
        if(fish == null)
            return;
        storedFish.Add(fish);
        totalWeight += fish.weight;
        totalPrice += fish.price;
        fish.transform.SetParent(transform, false);
        fish.transform.localPosition = Vector3.zero;
        RefreshDisplay();
    }
    public float GetWeight()
    {
        return totalWeight;
    }
    public float GetPrice()
    {
        return totalPrice;
    }
    public void RefreshDisplay(){
        if(inventoryText == null)
            return;
        if(storedFish.Count == 0){
            inventoryText.text = "Boat Inventory\nEmpty";
            return;
        }

        string summary = "Boat Inventory\n";
        for(int i = 0; i < storedFish.Count; i++){
            Fish fish = storedFish[i];
            string name = fish != null ? fish.name : "Unknown fish";
            summary += string.Format("- {0} ({1:0.0} kg)\n", name, fish != null ? fish.weight : 0f);
        }
        summary += "Total: " + totalWeight.ToString("0.0") + "kg\n $" + totalPrice.ToString("0.0");
        inventoryText.text = summary;
    }
}
