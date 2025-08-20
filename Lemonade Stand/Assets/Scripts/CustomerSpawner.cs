using System;
using UnityEngine;

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

    int NextId { get; set; }

    private void Start() {
        CustomerSpawnCount = 0;
        NextId = 1;
        CustomerQueue = new CustomerQueue();

        CreateTimers();
        World.OnDayStart += StartDay;
        World.OnDayEnd += EndDay;
    }

    private void Update() {
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

        NextId++;
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

    void StopUpdating() {
        enabled = false;
    }

    void StartUpdating() {
        enabled = true;
    }

    public void StartDay() {
        CustomerSpawnCount = 0;
        NextId = 1;
        CustomerQueue = new CustomerQueue();

        CreateTimers();
        SpawnDelayTimer.Start();
        StartUpdating();
    }

    public void EndDay() {
        CustomerQueue.UnbindCustomers();
        StopUpdating();
    }
}