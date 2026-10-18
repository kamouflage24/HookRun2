
using UnityEngine;

using TMPro;

public class Shop : MonoBehaviour
{   
    [SerializeField] private TMP_Text shopText;
    [SerializeField] private Upgrades upgrades;

    private void OnTriggerEnter(Collider other)
    {
        UpdateShopText(GetInventory(other));
    }

    private void OnTriggerStay(Collider other)
    {
        BoatInventory inventory = GetInventory(other);
        if(inventory == null)
            return;
        
        if(Input.GetKeyDown(KeyCode.E))
            TryBuyUpgrade(inventory);
        else
            UpdateShopText(inventory);
    }

    private BoatInventory GetInventory(Collider other)
    {
        BoatInventory inventory = other.GetComponentInParent<BoatInventory>();
        return inventory != null ? inventory : other.GetComponentInChildren<BoatInventory>();
    }

    private void TryBuyUpgrade(BoatInventory inventory)
    {
        if(upgrades == null)
        {
            return;
        }

        if (upgrades.TryPurchase(inventory))
        {
            UpdateShopText(inventory, upgrades.DisplayName + "purchased!");
        }
        else if (inventory.Money < upgrades.Price){
            UpdateShopText(inventory, "Not enough money.");
        }
        else
        {
            UpdateShopText(inventory);
        }
    }

    private void UpdateShopText(BoatInventory inventory, string message = null)
    {
        if(shopText == null || inventory == null)
            return;

        if (!string.IsNullOrEmpty(message))
        {
            shopText.text = message + "\nWallet: $" + inventory.Money.ToString("0.00");
            return;
        }

        shopText.text = upgrades.IsPurchased ? upgrades.DisplayName + "already purchased. \n Wallet: $" + inventory.Money.ToString("0.00") :
            upgrades.DisplayName + ": $" + upgrades.Price.ToString("0.00") + "\nPress E to buy. \nWallet: $" + inventory.Money.ToString("0.00");
            
        
    }
        

}