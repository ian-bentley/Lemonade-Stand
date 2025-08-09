public class Player
{
    public decimal price; // how much lemonade costs
    public decimal cash; // your cash
    public float serve_interval; // time in seconds to serve, MIN is 1.2
    public Inventory Inventory { get; private set; } // inventory data
    public Recipe Recipe { get; set; }
    public Timer serve_timer; // timer for serve duration
    public decimal earnings; // tracks earnings for the day
    public int served; // tracks how many served today
    public bool serving; // tracks if serving
    public int servings;
    public int servings_per_batch = 12;

    public Player()
    {
        price = 1.50m; // set starting price to $1.50
        cash = 20m; // set starting cash to $20
        serve_interval = 3f; // set starting serve interval to 3s
        Inventory = new Inventory(); // initialize the inventory
        earnings = 0m; // start at 0 earnings
        served = 0; // start at 0 served
        serving = false; // start as not serving
        serve_timer = new Timer(serve_interval); // set serve timer
        servings = 0;

        Recipe = new Recipe();
        Recipe.IncreaseLemons();
        Recipe.ServingsPerBatch = servings_per_batch;

        UIManager.Instance.SetCashText(cash);
        UIManager.Instance.SetPriceText(price);
    }

    public void Reset()
    {
        served = 0; // set served back to 0
        earnings = 0; // set earnings back to 0
        serve_timer = new Timer(serve_interval); // set serve timer to serve interval
        serving = false;

        UIManager.Instance.SetServedText(served);
    }

    public void StartServing()
    {
        serving = true; // start serving
        serve_timer.Start(); // start serve timer
    }

    public void Serve()
    {
        serving = false; // stop serving

        // make more servings if out
        if (servings == 0 && Inventory.HasBatchStock(Recipe))
        {
            servings = 12;
            Inventory.UseBatchStock(Recipe);
        }

        // if you have a serving, serve
        if (servings != 0 && Inventory.HasServingStock(Recipe))
        {
            servings--; // use a serving
            Inventory.UseServingStock(Recipe);
            served++; // track they've been served
            serve_timer.Reset(); // reser serve timer
            UIManager.Instance.SetServedText(served);

            Earn(price);
        }
    }

    public bool CanServe()
    {
        return !serving && Inventory.HasServingStock(Recipe);
    }

    public void Update()
    {
        if (serving)
        {
            serve_timer.Tick();
        }
    }

    public bool CanAfford(decimal price)
    {
        return cash >= price;
    }

    public void Spend(decimal price)
    {
        cash -= price;
        UIManager.Instance.SetCashText(cash);
    }

    public void Earn(decimal price)
    {
        earnings += price; // track earnings gained
        cash += price; // add price to cash
        UIManager.Instance.SetCashText(cash);
    }
}
