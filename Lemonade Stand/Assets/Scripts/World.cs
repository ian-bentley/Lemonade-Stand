using System;
using UnityEngine;

public class World : MonoBehaviour {
    public static event Action<int> OnDayCountChanged;
    public static event Action<float> OnDayTimerTicked;
    public static event Action<float> OnEndEarlyTimerTicked;
    public static event Action OnDayStart;
    public static event Action OnDayEnd;

    private int _day_count;

    public Timer DayTimer { get; private set; }
    public Timer EndEarlyTimer { get; private set; }
    public int DayCount {
        get => _day_count;
        private set {
            _day_count = value;
            OnDayCountChanged?.Invoke(_day_count);
        }
    }

    public const float day_duration = 180f;// how many seconds is one day, set to 180s
    public const float end_early_timer_duration = 5f; // how long end early timer goes, set to 5s

    private void Start() {
        DayCount = 1; // start at first day
        CreateTimers();

        UIButtonListener.OnStartButtonClicked += StartDay;
        CustomerManager.OnNoCustomers += EndEarly;
    }

    private void Update() {
        DayTimer.Tick();
        EndEarlyTimer.Tick();
    }

    void StopUpdating() {
        enabled = false;
    }

    void StartUpdating() {
        enabled = true;
    }

    public void CreateTimers() {
        DayTimer = new Timer(day_duration);
        EndEarlyTimer = new Timer(end_early_timer_duration);

        DayTimer.OnTimerTicked += OnDayTimerTicked;
        EndEarlyTimer.OnTimerTicked += OnEndEarlyTimerTicked;
        DayTimer.OnTimerElapsed += EndDay;
        EndEarlyTimer.OnTimerElapsed += EndDay;
    }

    public void StartDay()
    {
        CreateTimers();
        DayTimer.Start();
        StartUpdating();
        OnDayStart?.Invoke();
    }

    void EndDay() {
        RaiseDayCounter();
        StopUpdating();
        OnDayEnd?.Invoke();
    }

    public void EndEarly() {
        EndEarlyTimer.Start();
    }

    public void RaiseDayCounter() => DayCount++;
}
