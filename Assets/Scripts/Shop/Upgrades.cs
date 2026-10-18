
using UnityEngine;
using UnityEngine.Events;

public class Upgrades : MonoBehaviour
{

        [SerializeField] private string upgradeName = "Upgrade";
        [SerializeField, Min(0f)] private float price = 1000f;
        [SerializeField] private UnityEvent onPurchased;

        public string DisplayName => upgradeName;
        public float Price => price;
        public bool IsPurchased { get; private set;}

        public bool TryPurchase(BoatInventory inventory){
            if(inventory == null)
                return false;
            BoatMovement boat = inventory.GetComponentInParent<BoatMovement>();
            if(boat == null)
        {
            return false;
        }
            
            if(IsPurchased || !inventory.TrySpendMoney(price))
                return false;
            
            IsPurchased = true;
            boat.ApplySpeedUpgrade();
            onPurchased?.Invoke();
            return true;
        }

}
