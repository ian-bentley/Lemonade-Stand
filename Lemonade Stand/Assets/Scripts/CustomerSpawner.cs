using UnityEngine;

public class CustomerSpawner : MonoBehaviour {
    [SerializeField] private const int base_popularity = 5;

    [SerializeField] private Player Player;

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
        SpawnDelayTimer = new Timer(5f);
        SpawnTimer = new Timer(SpawnInterval);
        CustomerQueue = new CustomerQueue();

        SpawnDelayTimer.OnTimerElapsed += OnSpawnDelayTimerElapsed;
        SpawnTimer.OnTimerElapsed += OnSpawnTimerElapsed;

        SpawnDelayTimer.Start();
    }

    private void Update() {
        SpawnDelayTimer.Tick();
        SpawnTimer.Tick();
        CustomerQueue.Update();
    }

    public void OnSpawnDelayTimerElapsed() => SpawnTimer.Start();
    public void OnSpawnTimerElapsed() => SpawnCustomer();
    
    private void SpawnCustomer() {
        Timer patienceTimer = new Timer(12f);
        Customer customer = new Customer(NextId, patienceTimer);

        Debug.Log($"Customer {NextId} spawned");

        CustomerQueue.Enqueue(customer);

        NextId++;
        CustomerSpawnCount++;

        if (CanSpawnMoreCustomers) SpawnTimer.Restart();
    }
}