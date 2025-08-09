using UnityEngine;

public class SupplyShop
{
    const decimal lemon_price = 0.5m;

    public decimal GetLemonPrice(int amount)
    {
        return lemon_price * amount;
    }
}
