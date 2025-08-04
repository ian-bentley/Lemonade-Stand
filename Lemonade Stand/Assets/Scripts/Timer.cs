using UnityEngine;

public class Timer
{
    public float duration; // how long timer will go
    public float current_time; // what time it's at
    public bool running; // if timer is running
    public bool elapsed; // if timer has completed

    public Timer(float duration)
    {
        this.duration = duration;
        running = false;
        elapsed = false;
        current_time = duration;
    }

    public void Start()
    {
        if (running) return;
        running = true; // start running timer
    }
    public void Stop() => running = false; // stop running timer
    public void Reset()
    {
        // if elapsed add back duration and set to not elapsed
        if (elapsed)
        {
            current_time += duration;
            elapsed = false;
            return;
        }

        // otherwise just set to duration
        current_time = duration;
    }
    public void Tick()
    {
        if (!running) return;
        current_time -= Time.deltaTime; // tick down timer

        if (current_time > 0f) return;

        // if time has elapsed
        elapsed = true;
        Stop();
    }
}
