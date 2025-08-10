using System;

public class Player {
    public static event Action<decimal> OnCashChanged;
    public static event Action<decimal> OnLemonadePriceChanged;
    public static event Action<decimal> OnEarningsChanged;
    public static event Action<int> OnServedChanged;

    private decimal _cash;
    private decimal _lemonade_price;
    private decimal _earnings;
    private int _served;

    public decimal Cash {
        get => _cash;
        set {
            _cash = value;
            OnCashChanged?.Invoke(_cash);
        }
    }

    public decimal LemonadePrice {
        get => _lemonade_price;
        set {
            _lemonade_price = value;
            OnLemonadePriceChanged?.Invoke(_lemonade_price);
        }
    }

    public decimal Earnings { 
        get => _earnings;
        private set {
            _earnings = value;
            OnEarningsChanged?.Invoke(_earnings);
        }
    }
    public int Served {
        get => _served;
        private set {
            _served = value;
            OnServedChanged?.Invoke(_served);
        }
    }

    public Inventory Inventory { get; private set; }
    public Recipe Recipe { get; set; }
    public float ServeInterval { get; private set; } // time in seconds to serve, MIN is 1.2
    public Timer ServeTimer { get; private set; }
    public bool Serving { get; private set; } // tracks if serving
    public int Servings { get; private set; }
    public int ServingsPerBatch {  get; private set; }

    public Player() {
        Cash = 20m; // set starting cash to $20
        LemonadePrice = 1.50m; // set starting price to $1.50
        ServeInterval = 3f; // set starting serve interval to 3s
        Earnings = 0m; // start at 0 earnings
        Served = 0; // start at 0 served
        Serving = false; // start as not serving
        Servings = 0;
        ServingsPerBatch = 12;
        ServeTimer = new Timer(ServeInterval); // set serve timer
        Inventory = new Inventory();
        Recipe = new Recipe();

        Recipe.IncreaseLemons();
        Recipe.ServingsPerBatch = ServingsPerBatch;
    }

    public void StartDay() {
        Served = 0; // set served back to 0

        Earnings = 0; // set earnings back to 0
        ServeTimer = new Timer(ServeInterval); // set serve timer to serve interval
        ServeTimer.Start();

        Serving = false;
    }

    public void StartServing() {
        Serving = true; // start serving
        ServeTimer.Start(); // start serve timer
    }

    public void Serve() {
        Serving = false; // stop serving

        // make more servings if out
        if (Servings == 0 && Inventory.HasBatchStock(Recipe)) {
            Servings = 12;
            Inventory.UseBatchStock(Recipe);
        }

        // if you have a serving, serve
        if (Servings != 0 && Inventory.HasServingStock(Recipe)) {
            Servings--; // use a serving
            Inventory.UseServingStock(Recipe);
            Served++; // track they've been served
            ServeTimer.Reset(); // reser serve timer

            Earn(LemonadePrice);
        }
    }

    public bool CanServe() => !Serving && Inventory.HasServingStock(Recipe);

    public void Update() {
        if (Serving) ServeTimer.Tick();
    }

    public bool CanAfford(decimal price) => Cash >= price;

    public void Spend(decimal price) => Cash -= price;

    public void Earn(decimal price) {
        Cash += price;
        Earnings += price;
    }
}
