using UnityEngine;
using TMPro;

public class DropOff : MonoBehaviour
{
   
    [SerializeField] private float payment = 1000f;

    [SerializeField] private TMP_Text moneyText;
   
    private void OnTriggerEnter(Collider other)
    {
        TryDeposit(other);
    }
    private void OnTriggerStay(Collider other){
        TryDeposit(other);
    }
    private void TryDeposit(Collider other)
    {
        BoatInventory inventory = other.GetComponentInParent<BoatInventory>();

        if(inventory == null)
            inventory = other.GetComponentInChildren<BoatInventory>();

        if(inventory == null || inventory.storedFish.Count == 0)
            return;
        
        float value = inventory.SellFish();

        if (value <= 0f)
        {
           return;
        }
        inventory.AddMoney(value);
        UpdateMoneyUI(inventory, value);
        if(inventory.Money >= payment)
        {
            GameOver.TriggerGameOver("Payment Met!\n Demo Ended");
        }
    }
    private void UpdateMoneyUI(BoatInventory inventory, float deposited){
        if(moneyText == null)
            return;
        
        string depositMessage = "Deposited: $" + deposited.ToString("0.00");
        string balanceMessage = "Wallet: $" + inventory.Money.ToString("0.00");
        string paymentMessage = inventory.Money >= payment ? "Payment Met!" : "Goal: $" + payment.ToString("0.00");
        moneyText.text = depositMessage + "\n" + balanceMessage + "\n" + paymentMessage;
        }
}