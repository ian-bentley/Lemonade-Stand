public class Player
{
    public decimal price; // how much lemonade costs
    public decimal cash; // your cash
    public float serve_interval; // time in seconds to serve, MIN is 1.2
    public Inventory inventory; // inventory data
    public Timer serve_timer; // timer for serve duration
    public decimal earnings; // tracks earnings for the day
    public int served; // tracks how many served today
    public bool serving; // tracks if serving

    public Player()
    {
        price = 1.50m; // set starting price to $1.50
        cash = 20m; // set starting cash to $20
        serve_interval = 3f; // set starting serve interval to 3s
        inventory = new Inventory(); // initialize the inventory
        earnings = 0m; // start at 0 earnings
        served = 0; // start at 0 served
        serving = false; // start as not serving
        serve_timer = new Timer(serve_interval); // set serve timer
    }

    public void Reset()
    {
        served = 0; // set served back to 0
        earnings = 0; // set earnings back to 0
        serve_timer = new Timer(serve_interval); // set serve timer to serve interval
        serving = false;
    }

    public void StartServing()
    {
        serving = true; // start serving
        serve_timer.Start(); // start serve timer
    }

    public void Serve()
    {
        serving = false; // stop serving
        inventory.stock--; // serve one stock
        earnings += price; // track earnings gained
        cash += price; // add price to cash
        served++; // track they've been served
        serve_timer.Reset(); // reser serve timer
    }

    public void BuyStock()
    {
        // if you can afford to buy stock
        if (cash >= Inventory.stock_price)
        {
            cash -= Inventory.stock_price; // spend cash
            inventory.stock++; // get more stock
        }
    }

    public void Update()
    {
        if (serving)
        {
            serve_timer.Tick();
        }
    }
}
