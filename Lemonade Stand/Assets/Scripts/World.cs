using System;
using UnityEngine.LightTransport;

public class World {
    public static event Action<int> OnDayCountChanged;
    public static event Action<float> OnDayTimerTicked;

    private int _day_count;

    public Timer DayTimer { get; private set; }

    public int DayCount {
        get => _day_count;
        private set {
            _day_count = value;
            OnDayCountChanged?.Invoke(_day_count);
        }
    }

    public const float day_duration = 180f;// how many seconds is one day, set to 180s
    public const float end_early_timer_duration = 5f; // how long end early timer goes, set to 5s
    public bool day_running; // determines if day is running
    public Timer end_early_timer; // timer for ending day early if no customers

    public World()
    {
        DayCount = 1; // start day count at 1
        day_running = false; // start with day not running
        DayTimer = new Timer(day_duration); // set day timer to day duration
        end_early_timer = new Timer(end_early_timer_duration); // set timer to end early duration

        DayTimer.OnTimerTicked += OnDayTimerTicked;
    }

    public void StartDay()
    {
        DayTimer = new Timer(day_duration);
        DayTimer.OnTimerTicked += OnDayTimerTicked;
        DayTimer.Start();
        end_early_timer = new Timer(end_early_timer_duration);

        day_running = true;
    }

    public void Update()
    {
        DayTimer.Tick();
        end_early_timer.Tick();

        // end day if day timer or end early timer is up
        if (DayTimer.elapsed || end_early_timer.elapsed) day_running = false;
    }

    public void RaiseDayCounter() => DayCount++;
}
