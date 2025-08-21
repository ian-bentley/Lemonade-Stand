using System;
using UnityEngine;

public enum PlayerState {
    Idle,
    Running,
}

public class Player : MonoBehaviour {
    public static event Action<decimal> OnCashChanged;
    public static event Action<decimal> OnLemonadePriceChanged;
    public static event Action<decimal> OnEarningsChanged;
    public static event Action<int> OnServedChanged;
    public static event Action OnServe;
    public static event Action<float> OnServeTimerTicked;
    public static event Action OnPaid;
    public static event Action OutOfStock;

    private decimal _cash;
    private decimal _lemonade_price;
    private decimal _earnings;
    private int _served;

    public decimal Cash {
        get => _cash;
        set {
            _cash = value;
            if (value < 0m) _cash = 0;
            if (value > 999999999999.99m) _cash = 999999999999.99m;
            OnCashChanged?.Invoke(_cash);
        }
    }

    public decimal LemonadePrice {
        get => _lemonade_price;
        set {
            _lemonade_price = value;
            if (value < 0m) _lemonade_price = 0;
            if (value > 9.99m) _lemonade_price = 9.99m;
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
    private Customer ServingCustomer { get; set; }
    private PlayerState PlayerState { get; set; }

    private void OnEnable() {
        World.OnDayStart += StartDay;
        World.OnDayEnd += EndDay;
        SupplyShop.OnPurchaseRequested += TryPurchase;
        Customer.Order += StartServing;
    }

    private void OnDisable() {
        World.OnDayStart -= StartDay;
        World.OnDayEnd -= EndDay;
        SupplyShop.OnPurchaseRequested -= TryPurchase;
        Customer.Order -= StartServing;
    }

    private void Start() {
        Cash = 20m; // set starting cash to $20
        LemonadePrice = 1.50m; // set starting price to $1.50
        ServeInterval = 1.5f; // set starting serve interval to 3s
        Earnings = 0m; // start at 0 earnings
        Served = 0; // start at 0 served
        Servings = 0;
        ServingsPerBatch = 12;
        Inventory = new Inventory();
        PlayerStats = new PlayerStats();
        Recipe = new Recipe();

        Recipe.ServingsPerBatch = ServingsPerBatch;

        PlayerState = PlayerState.Idle;
    }

    private void Update() {
        if (PlayerState == PlayerState.Idle) return;

        ServeTimer.Tick();
    }

    void CreateTimers() {
        ServeTimer = new Timer(ServeInterval);
        ServeTimer.OnTimerElapsed += Serve;
        ServeTimer.OnTimerTicked += OnServeTimerTicked;
    }

    public void StartDay() {
        Served = 0;
        Earnings = 0;
        Servings = 0;
        Inventory.CupsCount = 999;

        CreateTimers();
        PlayerState = PlayerState.Running;
    }

    public void EndDay() {
        if (ServingCustomer  != null) {
            OnServe -= ServingCustomer.OnPlayerServe;
            ServingCustomer = null;
        }

        ServeTimer.OnTimerElapsed -= Serve;
        ServeTimer.OnTimerTicked -= OnServeTimerTicked;
        PlayerState = PlayerState.Idle;
    }

    public void StartServing(Customer customer) {
        ServingCustomer = customer;
        OnServe += ServingCustomer.OnPlayerServe;
        ServeTimer.Reset();
        ServeTimer.Start();
        Earn(LemonadePrice);
    }

    public void Serve() {

        // make more servings if out
        if (Servings == 0 && Inventory.HasBatchStock(Recipe)) {
            Servings = ServingsPerBatch;
            Inventory.UseBatchStock(Recipe);
        }

        if (Servings == 0 && !Inventory.HasBatchStock(Recipe)) OutOfStock?.Invoke();

        // if you have a serving, serve
        if (Servings != 0 && Inventory.HasServingStock(Recipe)) {
            Servings--; // use a serving
            Inventory.UseServingStock(Recipe);
            Served++; // track they've been served
            OnServe?.Invoke();
            OnServe -= ServingCustomer.OnPlayerServe;
            ServingCustomer = null;
        }
    }

    public bool CanAfford(decimal price) => Cash >= price;

    public void Spend(decimal price) => Cash -= price;

    public void Earn(decimal price) {
        Cash += price;
        Earnings += price;
        OnPaid?.Invoke();
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
