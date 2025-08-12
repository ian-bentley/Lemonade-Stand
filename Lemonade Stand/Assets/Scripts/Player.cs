using System;
using UnityEngine;

public class Player : MonoBehaviour {
    public static event Action<decimal> OnCashChanged;
    public static event Action<decimal> OnLemonadePriceChanged;
    public static event Action<decimal> OnEarningsChanged;
    public static event Action<int> OnServedChanged;
    public static event Action OnServe;
    public static event Action<float> OnServeTimerTicked;

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
    public PlayerStats PlayerStats { get; private set; }
    public Recipe Recipe { get; set; }
    public Timer ServeTimer { get; private set; }
    public float ServeInterval { get; private set; } // time in seconds to serve, MIN is 1.2
    public int Servings { get; private set; }
    public int ServingsPerBatch {  get; private set; }

    private void Start() {
        Cash = 20m; // set starting cash to $20
        LemonadePrice = 1.50m; // set starting price to $1.50
        ServeInterval = 3f; // set starting serve interval to 3s
        Earnings = 0m; // start at 0 earnings
        Served = 0; // start at 0 served
        Servings = 0;
        ServingsPerBatch = 12;
        Inventory = new Inventory();
        PlayerStats = new PlayerStats();
        Recipe = new Recipe();

        Recipe.IncreaseLemons();
        Recipe.ServingsPerBatch = ServingsPerBatch;

        CreateTimers();

        World.OnDayStart += StartDay;
        World.OnDayEnd += Inventory.ExpireStock;
        CustomerManager.OnNextCustomer += StartServing;
        SupplyShop.OnPurchaseRequested += TryPurchase;
    }

    private void Update() {
        ServeTimer.Tick();
    }

    void StopUpdating() {
        enabled = false;
    }

    void StartUpdating() {
        enabled = true;
    }

    void CreateTimers() {
        ServeTimer = new Timer(ServeInterval);
        ServeTimer.OnTimerElapsed += Serve;
        ServeTimer.OnTimerTicked += OnServeTimerTicked;
    }

    public void StartDay() {
        Served = 0;
        Earnings = 0;

        CreateTimers();
        StartUpdating();
    }

    public void EndDay() {
        StopUpdating();
    }

    public void StartServing() {
        ServeTimer.Reset();
        ServeTimer.Start();
    }

    public void Serve() {

        // make more servings if out
        if (Servings == 0 && Inventory.HasBatchStock(Recipe)) {
            Servings = ServingsPerBatch;
            Inventory.UseBatchStock(Recipe);
        }

        // if you have a serving, serve
        if (Servings != 0 && Inventory.HasServingStock(Recipe)) {
            Servings--; // use a serving
            Inventory.UseServingStock(Recipe);
            Served++; // track they've been served

            Earn(LemonadePrice);
            OnServe?.Invoke();
        }
    }

    public bool CanAfford(decimal price) => Cash >= price;

    public void Spend(decimal price) => Cash -= price;

    public void Earn(decimal price) {
        Cash += price;
        Earnings += price;
    }

    public void TryPurchase(PurchaseRequest purchaseRequest) {
        if (CanAfford(purchaseRequest.Price)) {

            Spend(purchaseRequest.Price);

            if (purchaseRequest.ItemType == ItemType.Lemon) Inventory.AddLemons(purchaseRequest.Amount);
            if (purchaseRequest.ItemType == ItemType.Sugar) Inventory.AddSugar(purchaseRequest.Amount);
            if (purchaseRequest.ItemType == ItemType.Ice) Inventory.AddIce(purchaseRequest.Amount);
            if (purchaseRequest.ItemType == ItemType.Cups) Inventory.AddCups(purchaseRequest.Amount);
        }
    }
}
