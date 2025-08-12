using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class DebugTool : MonoBehaviour
{
    [SerializeField] GameObject debug_canvas;
    [SerializeField] TextMeshProUGUI debug_text;
    bool debug_on;
    string served_text;
    string earnings_text;
    string day_timer_text;
    string end_early_timer_text;
    string spawn_delay_timer_text;
    string spawn_timer_text;
    string serve_timer_text;
    Dictionary<int, float> patience_timers;

    private void OnEnable() {
        Player.OnServedChanged += SetServedText;
        Player.OnEarningsChanged += SetEarningsText;
        Player.OnServeTimerTicked += SetServeTimerText;
        World.OnDayTimerTicked += SetDayTimerText;
        World.OnEndEarlyTimerTicked += SetEndEarlyTimerText;
        CustomerManager.OnSpawnDelayTimerTicked += SetSpawnDelayTimerText;
        CustomerManager.OnSpawnTimerTicked += SetSpawnTimerText;
        CustomerManager.OnCustomerAdded += AddPatienceTimer;
        CustomerManager.OnCustomerRemoved += RemovePatienceTimer;
        Customer.OnPatienceTimerTicked += SetPatienceTimer;
    }

    private void OnDisable() {
        Player.OnServedChanged -= SetServedText;
        Player.OnEarningsChanged -= SetEarningsText;
        Player.OnServeTimerTicked -= SetServeTimerText;
        World.OnDayTimerTicked -= SetDayTimerText;
        World.OnEndEarlyTimerTicked -= SetEndEarlyTimerText;
        CustomerManager.OnSpawnDelayTimerTicked -= SetSpawnDelayTimerText;
        CustomerManager.OnSpawnTimerTicked -= SetSpawnTimerText;
        CustomerManager.OnCustomerAdded -= AddPatienceTimer;
        CustomerManager.OnCustomerRemoved -= RemovePatienceTimer;
        Customer.OnPatienceTimerTicked -= SetPatienceTimer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        debug_canvas.SetActive(false);
        debug_on = false;

        patience_timers = new Dictionary<int, float>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) ToggleDebug();

        debug_text.text = "";
        debug_text.text += day_timer_text;
        debug_text.text += end_early_timer_text;
        debug_text.text += spawn_delay_timer_text;
        debug_text.text += spawn_timer_text;
        debug_text.text += serve_timer_text;
        debug_text.text += served_text;
        debug_text.text += earnings_text;
        debug_text.text += "\nCustomers:\n";

        var patience_timers_copy = patience_timers.OrderBy(i => i.Key);

        foreach (var (id, currentTime) in patience_timers_copy) {
            debug_text.text += $"[Customer {id}] Patience timer: {currentTime}\n";
        }
    }

    void ToggleDebug() {
        debug_on = !debug_on;
        debug_canvas.SetActive(debug_on);
    }

    void SetServedText(int served) => served_text = $"Served: {served}\n";
    void SetEarningsText(decimal earnings) => earnings_text = $"Earnings: {earnings}\n";
    void SetDayTimerText(float currentTime) => day_timer_text = $"Day timer: {currentTime}\n";
    void SetEndEarlyTimerText(float currentTime) => end_early_timer_text = $"End early timer: {currentTime}\n";
    void SetSpawnDelayTimerText(float currentTime) => spawn_delay_timer_text = $"Spawn delay timer: {currentTime}\n";
    void SetSpawnTimerText(float currentTime) => spawn_timer_text = $"Spawn timer: {currentTime} \n";
    void SetServeTimerText(float currentTime) => serve_timer_text = $"Serve timer: {currentTime}\n";

    void AddPatienceTimer(int id, float currentTime) {
        patience_timers.Add(id, currentTime);
    }

    void RemovePatienceTimer(int id) {
        patience_timers.Remove(id);
    }

    void SetPatienceTimer(int id, float currentTime) {
        patience_timers[id] = currentTime;
    }
}
