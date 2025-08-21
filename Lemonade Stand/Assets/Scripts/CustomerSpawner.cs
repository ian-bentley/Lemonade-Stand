using System;
using UnityEngine;

public enum CustomerSpawnerState {
    Idle,
    Running
}

public class CustomerSpawner : MonoBehaviour {
    [SerializeField] private const int base_popularity = 5;

    [SerializeField] private Player Player;

    public static event Action<float> SpawnDelayTimerTicked;
    public static event Action<float> SpawnTimerTicked;
    public static event Action NoCustomers;

    private int Popularity => base_popularity + Player.PlayerStats.Attraction;
    private int MaxCustomers => Popularity;
    private float SpawnDuration => MaxCustomers;
    private float SpawnInterval => 1; // TODO make it SpawnDuration / MaxCustomers but fix null reference to Player/PlayerStats
    private bool CanSpawnMoreCustomers => CustomerSpawnCount < MaxCustomers;

    private int CustomerSpawnCount { get; set; }
    private Timer SpawnDelayTimer { get; set; }
    private Timer SpawnTimer { get; set; }
    private CustomerQueue CustomerQueue { get; set; }
    private CustomerSpawnerState CustomerSpawnerState { get; set; }

    private void OnEnable() {
        World.OnDayStart += StartDay;
        World.OnDayEnd += EndDay;
    }

    private void OnDisable() {
        World.OnDayStart -= StartDay;
        World.OnDayEnd -= EndDay;
    }

    private void Start() {
        CustomerSpawnCount = 0;
        CustomerSpawnerState = CustomerSpawnerState.Idle;
        CustomerQueue = new CustomerQueue();
    }

    private void Update() {
        if (CustomerSpawnerState == CustomerSpawnerState.Idle) return;

        SpawnDelayTimer.Tick();
        SpawnTimer.Tick();
        CustomerQueue.Update();

        if (!CustomerQueue.HasCustomerInQueue && !CanSpawnMoreCustomers) NoCustomers?.Invoke();
    }

    public void OnSpawnDelayTimerElapsed() => SpawnTimer.Start();
    public void OnSpawnTimerElapsed() => SpawnCustomer();
    
    private void SpawnCustomer() {
        Timer patienceTimer = new Timer(12f);
        Customer customer = new Customer(patienceTimer);

        CustomerQueue.Enqueue(customer);
        CustomerSpawnCount++;

        if (CanSpawnMoreCustomers) SpawnTimer.Restart();
    }

    void CreateTimers() {
        SpawnDelayTimer = new Timer(5f);
        SpawnTimer = new Timer(SpawnInterval);

        SpawnDelayTimer.OnTimerElapsed += OnSpawnDelayTimerElapsed;
        SpawnTimer.OnTimerElapsed += OnSpawnTimerElapsed;

        SpawnDelayTimer.OnTimerTicked += SpawnDelayTimerTicked;
        SpawnTimer.OnTimerTicked += SpawnTimerTicked;
    }

    public void StartDay() {
        CustomerSpawnCount = 0;
        CustomerQueue = new CustomerQueue();

        CreateTimers();
        SpawnDelayTimer.Start();
        CustomerSpawnerState = CustomerSpawnerState.Running;
    }

    public void EndDay() {
        SpawnDelayTimer.OnTimerElapsed -= OnSpawnDelayTimerElapsed;
        SpawnTimer.OnTimerElapsed -= OnSpawnTimerElapsed;
        SpawnDelayTimer.OnTimerTicked -= SpawnDelayTimerTicked;
        SpawnTimer.OnTimerTicked -= SpawnTimerTicked;
        CustomerQueue.UnbindCustomers();
        CustomerSpawnerState = CustomerSpawnerState.Idle;
    }
}