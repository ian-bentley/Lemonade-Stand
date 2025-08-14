using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour {
    public static event Action<int, float> OnCustomerAdded;
    public static event Action<int> OnCustomerRemoved;
    public static event Action OnNoCustomers;
    public static event Action OnNextCustomer;
    public static event Action<float> OnSpawnDelayTimerTicked;
    public static event Action<float> OnSpawnTimerTicked;
    public static event Action OnCustomerServed;

    const float customer_base_patience = 5f; // set base patience to 5s

    [SerializeField] private Player Player;

    private List<Customer> CustomerQueue;
    private Timer SpawnDelayTimer { get; set; }
    private Timer SpawnTimer {  get; set; }
    private int Popularity => 5 + Player.PlayerStats.Attraction;
    private int MaxCustomers => Popularity;
    private float SpawnDuration => Popularity;
    private int CustomersSpawned { get; set; }
    private float CustomerPatience => customer_base_patience;
    private int NextId { get; set; }

    private void Start() {
        CustomerQueue = new List<Customer>();
        CustomersSpawned = 0; // sets customer spawned to 0 to start
        NextId = 1; // set ids to start at 1

        CreateTimers();

        World.OnDayStart += StartDay;
        Player.OnServe += GiveDrinkToFrontCustomer;
        Customer.OnCustomerLeft += ExitImpatientCustomer;
    }

    private void Update() {
        SpawnDelayTimer.Tick();
        SpawnTimer.Tick();

        // decrease and check customer patience, set to stop at one to avoid ticking patience of first in line
        for (int i = CustomerQueue.Count - 1; i >= 1; i--) {
            Customer customer = CustomerQueue[i];
            customer.Update();
        }
    }

    void StartSpawning() {
        SpawnTimer.Reset();
        SpawnTimer.Start();
    }

    void StopUpdating() {
        enabled = false;
    }

    void StartUpdating() {
        enabled = true;
    }

    public void StartDay() {
        CustomerQueue = new List<Customer>();
        CustomersSpawned = 0; // sets customer spawned to 0 to start
        NextId = 1;

        CreateTimers();
        SpawnDelayTimer.Start();
        StartUpdating();
    }

    public void EndDay() {
        StopUpdating();
    }

    void CreateTimers() {
        SpawnDelayTimer = new Timer(5f);

        float spawn_interval = SpawnDuration / MaxCustomers; // set spawn interval to duration over customers
        SpawnTimer = new Timer(spawn_interval); // set spawn timer to spawn interval

        SpawnDelayTimer.OnTimerTicked += OnSpawnDelayTimerTicked;
        SpawnTimer.OnTimerTicked += OnSpawnTimerTicked;
        SpawnDelayTimer.OnTimerElapsed += StartSpawning;
        SpawnTimer.OnTimerElapsed += SpawnCustomer;
    }

    void SpawnCustomer() {
        if (CanSpawnMoreCustomers()) {
            bool wasEmpty = !HasCustomerInQueue();

            // add customer to queue
            Customer newCustomer = new Customer(NextId, new Timer(CustomerPatience));
            CustomerQueue.Add(newCustomer);
            NextId++;
            OnCustomerAdded?.Invoke(newCustomer.Id, newCustomer.PatienceTimer.duration);

            // track spawn
            CustomersSpawned++;

            StartSpawning();

            if (wasEmpty) OnNextCustomer?.Invoke();
        }
    }

    public bool CanSpawnMoreCustomers() => CustomersSpawned < MaxCustomers;

    public bool HasCustomerInQueue() => CustomerQueue.Count > 0;

    public void DequeueCustomer(int index) {
        Customer removedCustomer = CustomerQueue.ElementAt(index);
        OnCustomerRemoved?.Invoke(removedCustomer.Id);
        CustomerQueue.RemoveAt(index);

        if (!HasCustomerInQueue() && !CanSpawnMoreCustomers()) OnNoCustomers?.Invoke();
    }

    public void GiveDrinkToFrontCustomer() {
        OnCustomerServed?.Invoke();
        DequeueCustomer(0);
        if (HasCustomerInQueue()) OnNextCustomer?.Invoke();
    }

    public void ExitImpatientCustomer(int id) {
        for (int i = CustomerQueue.Count - 1; i >= 1; i--) {
            Customer customer = CustomerQueue[i];
            if (customer.Id == id) DequeueCustomer(i);
        }
    }
}
