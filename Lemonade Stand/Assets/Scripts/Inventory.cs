public class Inventory
{
    public const decimal stock_price = 0.5m; // how much stock costs, set to $0.50
    public int stock; // how much stock you have

    public Inventory()
    {
        stock = 0; // set initial stock to nothing
    }

    public bool HasStock() => stock > 0;

    public void ExpireStock()
    {
        stock = 0; // expire all stock
    }
}
