using System;
using UnityEngine;

public class Customer
{
    public static event Action<string, float> OnPatienceTimerTicked;
    public event Action<Customer> Leave;
    public event Action<Customer> ReceiveDrink;
    public static event Action<Customer> Order;
    public static event Action CustomerLeft;
    public static event Action CustomerServed;

    public string Id { get; } = System.Guid.NewGuid().ToString();
    public Timer PatienceTimer { get; }

    public Customer(Timer patienceTimer) {
        PatienceTimer = patienceTimer;
        PatienceTimer.Start();

        PatienceTimer.OnTimerTicked += currentTime => OnPatienceTimerTicked?.Invoke(Id, currentTime);
        PatienceTimer.OnTimerElapsed += OnPatienceTimerElapsed;
    }

    public void Update() {
        PatienceTimer.Tick();
    }

    public void OnPatienceTimerElapsed() {
        CustomerLeft?.Invoke();
        Leave?.Invoke(this);
    }

    public void OnPlayerServe() {
        CustomerServed?.Invoke();
        ReceiveDrink?.Invoke(this);
    }
    public void OnReachedStand() => Order?.Invoke(this);
}
