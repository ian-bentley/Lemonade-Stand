using System;
using UnityEngine;

public class SupplyShop : MonoBehaviour {
    const decimal lemon_price = 0.5m;

    public static event Action<PurchaseRequest> OnPurchaseRequested;

    private void Start() {
        UIButtonListener.OnSupplyShopButtonClicked += BuyLemons;
    }

    void BuyLemons() {
        PurchaseRequest purchaseRequest = new PurchaseRequest(ItemType.Lemon, lemon_price, 1);
        OnPurchaseRequested?.Invoke(purchaseRequest);
    }
}
