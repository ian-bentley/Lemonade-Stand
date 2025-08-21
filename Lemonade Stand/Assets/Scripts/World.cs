using System;
using UnityEngine;

public enum DayState {
    Idle,
    Running
}

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

    private DayState DayState { get; set; }

    private void OnEnable() {
        UIButtonListener.OnStartButtonClicked += StartDay;
        CustomerSpawner.NoCustomers += EndEarly;
        Player.OutOfStock += EndEarly;
    }

    private void OnDisable() {
        UIButtonListener.OnStartButtonClicked -= StartDay;
        CustomerSpawner.NoCustomers -= EndEarly;
        Player.OutOfStock -= EndEarly;
    }

    private void Start() {
        DayCount = 1; // start at first day
        DayState = DayState.Idle;
    }

    private void Update() {
        if (DayState == DayState.Idle) return;

        DayTimer.Tick();
        EndEarlyTimer.Tick();
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
        DayState = DayState.Running;
        OnDayStart?.Invoke();
    }

    void EndDay() {
        DayTimer.OnTimerTicked -= OnDayTimerTicked;
        EndEarlyTimer.OnTimerTicked -= OnEndEarlyTimerTicked;
        DayTimer.OnTimerElapsed -= EndDay;
        EndEarlyTimer.OnTimerElapsed -= EndDay;
        RaiseDayCounter();
        DayState = DayState.Idle;
        OnDayEnd?.Invoke();
    }

    public void EndEarly() {
        EndEarlyTimer.Start();
    }

    public void RaiseDayCounter() => DayCount++;
}
