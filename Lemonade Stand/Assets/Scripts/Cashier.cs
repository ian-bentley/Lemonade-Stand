using UnityEngine;

public class Cashier {
    Player Player { get; set; }
    SupplyShop SupplyShop { get; set; }

    public Cashier(Player player, SupplyShop supplyShop) {
        Player = player;
        SupplyShop = supplyShop;

        UIButtonListener.Instance.OnSupplyShopButtonClicked += BuyLemons;
    }

    void BuyLemons() {
        int lemon_amount = 1;

        decimal price = SupplyShop.GetLemonPrice(lemon_amount);

        if (Player.CanAfford(price)) {
            Player.Spend(price);
            Player.Inventory.AddLemons(lemon_amount);
            Player.Inventory.AddCups(lemon_amount);
        }
    }
}
