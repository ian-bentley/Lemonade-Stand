using System;
using UnityEngine;

public class Timer
{
    public event Action<float> OnTimerTicked;
    public event Action OnTimerElapsed;

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

    public void Start() => running = true;
    public void Stop() => running = false;
    public void Reset()
    {
        current_time = duration;
        elapsed = false;
    }
    public void Tick()
    {
        if (!running) return;

        current_time -= Time.deltaTime; // tick down timer
        OnTimerTicked?.Invoke(current_time);

        if (current_time > 0f) return;

        // if time has elapsed
        Stop();
        elapsed = true;
        OnTimerElapsed?.Invoke();
    }
}
