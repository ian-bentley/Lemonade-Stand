using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public Player Player { get; private set; }
    public World World { get; private set; }
    public CustomerManager CustomerManager { get; private set; }
    public SupplyShop SupplyShop { get; private set; }
    public Cashier Cashier { get; private set; }
    public StateMachine StateMachine { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // create objects
        Player = new Player();
        World = new World();
        CustomerManager = new CustomerManager();
        SupplyShop = new SupplyShop();
        Cashier = new Cashier(Player, SupplyShop);
        StateMachine = new StateMachine();
        StateMachine.ChangeState(new PrepDayState(this));

        UIManager.Instance.OnStartButtonClicked += StartDay;
    }

    private void Update()
    {
        StateMachine.Tick();
    }

    // resets player, world, and customer manager for the day
    public void ResetDay()
    {
        Player.Reset();
        World.Reset();
        CustomerManager.Reset();
    }

    // start all timers for the day
    public void StartDayTimers()
    {
        Player.serve_timer.Start();
        World.day_timer.Start();
        CustomerManager.spawn_delay_timer.Start();
    }

    void StartDay()
    {
        StateMachine.ChangeState(new DaytimeState(this));
    }

    public void EndDay()
    {
        World.RaiseDayCounter();
        Player.Inventory.ExpireStock();
    }
}
