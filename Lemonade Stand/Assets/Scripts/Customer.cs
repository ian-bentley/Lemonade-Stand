using System;
using UnityEngine;

public class Customer
{
    public static event Action<float> OnPatienceTimerTickedWrapper;
    public static event Action<int, float> OnPatienceTimerTicked;
    public static event Action OnPatienceTimerElapsed;
    public static event Action<int> OnCustomerLeft;

    public int Id { get; }
    public Timer PatienceTimer { get; }

    public Customer(int id, Timer patienceTimer) {
        Id = id;
        PatienceTimer = patienceTimer;
        PatienceTimer.Start();

        PatienceTimer.OnTimerTicked += currentTime => OnPatienceTimerTicked?.Invoke(Id, currentTime);
        PatienceTimer.OnTimerElapsed += OnPatienceTimerElapsed;
        PatienceTimer.OnTimerElapsed += Leave;
    }

    public void Update() {
        PatienceTimer.Tick();
    }

    private void Leave() {
        OnCustomerLeft?.Invoke(Id);
    }
}
