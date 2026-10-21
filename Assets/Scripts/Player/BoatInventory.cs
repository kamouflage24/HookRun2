using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;


public class BoatInventory : MonoBehaviour
{

    public float totalWeight;
    public float totalPrice;
    [SerializeField] private float money;
    public List<Fish> storedFish = new List<Fish>();
    [SerializeField] private TMP_Text inventoryText;
    public float Money => money;
    public float FishLuck {get; private set; }

    public void AddFishLuck(float amount)
    {
        FishLuck += Mathf.Max(0f, amount);
    }

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
    public void AddMoney(float amount)
    {
        if(amount < 0f)
        {
            return;
        }
        money += amount;
        RefreshDisplay();
    }
    public bool TrySpendMoney(float amount)
    {
        if(amount < 0f)
        {
            return false;
        }
        if(money < amount)
            return false;
        money -= amount;
        RefreshDisplay();
        return true;
    }
    public float SellFish()
    {
        if(storedFish.Count == 0)
            return 0f;
        float value = totalPrice;

        foreach(Fish fish in storedFish)
        {
            if(fish != null)
            {
                Destroy(fish.gameObject);
            }
        }
        storedFish.Clear();
        totalWeight = 0f;
        totalPrice = 0f;
        RefreshDisplay();
        return value;
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
        summary += "\nWallet: $" + money.ToString("0.00");
        inventoryText.text = summary;
    }
}
