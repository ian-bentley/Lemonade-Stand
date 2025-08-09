using UnityEngine.LightTransport;

public class World
{
    public const float day_duration = 180f;// how many seconds is one day, set to 180s
    public const float end_early_timer_duration = 5f; // how long end early timer goes, set to 5s
    public int day_count; // tracks the number of days
    public bool day_running; // determines if day is running
    public Timer day_timer; // timer for running the day
    public Timer end_early_timer; // timer for ending day early if no customers

    public World()
    {
        day_count = 1; // start day count at 1
        day_running = false; // start with day not running
        day_timer = new Timer(day_duration); // set day timer to day duration
        end_early_timer = new Timer(end_early_timer_duration); // set timer to end early duration
    }

    public void Reset()
    {
        day_running = true;
        day_timer.Reset();
        end_early_timer.Reset();

        UIManager.Instance.SetDayTimerText(day_timer.current_time);
    }

    public void Update()
    {
        day_timer.Tick();
        end_early_timer.Tick();

        // end day if day timer or end early timer is up
        if (day_timer.elapsed || end_early_timer.elapsed) day_running = false;

        UIManager.Instance.SetDayTimerText(day_timer.current_time);
    }

    public void RaiseDayCounter()
    {
        day_count++;
        UIManager.Instance.SetDayCounterText(day_count);
    }
}
