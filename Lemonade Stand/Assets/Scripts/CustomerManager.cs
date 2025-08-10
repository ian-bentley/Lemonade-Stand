using System;
using System.Collections.Generic;

public class CustomerManager
{
    public static event Action<int> OnQueueChanged;

    private int _customer_queue;

    public int CustomerQueue {
        get => _customer_queue;
        set {
            _customer_queue = value;
            OnQueueChanged?.Invoke(_customer_queue);
        }
    }

    const float customer_base_patience = 5f; // set base patience to 5s

    public Timer spawn_delay_timer; // set spawn delay timer
    public int max_customers; // max customers that can be spawned for day 
    public float spawn_duration; // how long spawning will occur for
    public Timer spawn_timer; // cooldown between spawns
    public int customers_spawned; // tracks customers spawned
    public float customer_patience; // customer patience limit
    public List<Timer> patience_queue; // tracks patience of customers

    public void StartDay() {
        max_customers = 5; // set max customers to 5
        CustomerQueue = 0; // sets queue to 0 to start
        customers_spawned = 0; // sets customer spawned to 0 to start
        customer_patience = customer_base_patience; // set patience to base patience
        spawn_duration = 5f; // set spawn duration to 5s
        patience_queue = new List<Timer>(); // initialize patience queue

        spawn_delay_timer = new Timer(5f);
        spawn_delay_timer.Start();

        float spawn_interval = spawn_duration / max_customers; // set spawn interval to duration over customers
        spawn_timer = new Timer(spawn_interval); // set spawn timer to spawn interval
    }

    void SpawnCustomer() {
        // add customer to queue
        CustomerQueue++;

        // track their patience
        Timer patience_timer = new Timer(customer_patience);
        patience_timer.Start();
        patience_queue.Add(patience_timer);

        // track spawn
        customers_spawned++;

        // reset spawn timer
        spawn_timer.Reset();
        spawn_timer.Start();
    }

    public bool CanSpawnMoreCustomers() => customers_spawned < max_customers;

    public bool HasCustomerInQueue() => CustomerQueue > 0;

    public bool QueueIsEmpty() => CustomerQueue == 0;

    public void DequeueCustomer(int index) {
        CustomerQueue--; // remove them from the queue
        patience_queue.RemoveAt(index); // stop tracking their patience
    }

    public void DequeueFrontCustomer() => DequeueCustomer(0);

    public void Update() {
        // tick spawn delay timer
        spawn_delay_timer.Tick();

        // once delay is up, start spawn timer
        if (spawn_delay_timer.elapsed && !spawn_timer.running) spawn_timer.Start();

        // if spawn delay is done and not at max customers
        if (spawn_delay_timer.elapsed && CanSpawnMoreCustomers()){
            // tick spawn timer
            spawn_timer.Tick();

            // spawn a customer if timer is up and reset timer
            if (spawn_timer.elapsed) SpawnCustomer();
        }

        // decrease and check customer patience
        for (int i = patience_queue.Count - 1; i >= 0; i--) {
            Timer patience_timer = patience_queue[i];
            // front no longer loses patience
            if (i == 0) continue;

            // tick this patience timer
            patience_timer.Tick();

            // if patience timer is up
            if (patience_timer.elapsed) DequeueCustomer(i);
        }
    }
}
