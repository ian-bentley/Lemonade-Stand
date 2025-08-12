public class PurchaseRequest {
    public ItemType ItemType { get; }
    public decimal Price { get; }
    public int Amount { get; }

    public PurchaseRequest(ItemType itemType, decimal price, int amount) {
        ItemType = itemType;
        Price = price;
        Amount = amount;
    }
}
