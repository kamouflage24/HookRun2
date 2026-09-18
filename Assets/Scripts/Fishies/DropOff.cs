using UnityEngine;
using TMPro;

public class DropOff : MonoBehaviour
{
    [SerializeField] private float money = 0f;
    [SerializeField] private float payment = 1000f;

    [SerializeField] private TMP_Text moneyText;
    private bool met = false;
    private void OnTriggerEnter(Collider other)
    {
        TryDeposit(other);
    }
    private void OnTriggerStay(Collider other){
        TryDeposit(other);
    }
    private void TryDeposit(Collider other)
    {
        BoatInventory inventory = other.GetComponentInChildren<BoatInventory>();

        if(inventory == null || inventory.storedFish.Count == 0)
            return;
        
        float value = inventory.SellFish();

        if (value > 0f)
        {
            money += value;
            UpdateMoneyUI();
        }
        if (money >= payment){
            met = true;
        }
    }
    private void UpdateMoneyUI(){
        if(moneyText != null){
            moneyText.text = "Payments: $" + money.ToString("0.00") + "/ $" + payment.ToString("1000.00");
        }
        if(met == true){
            moneyText.text = "Payment Met!!!";
        }
    }
}
