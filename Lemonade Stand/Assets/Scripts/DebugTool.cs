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

    private void OnEnable() {
        Player.OnServedChanged += SetServedText;
        Player.OnEarningsChanged += SetEarningsText;
    }

    private void OnDisable() {
        Player.OnServedChanged -= SetServedText;
        Player.OnEarningsChanged -= SetEarningsText;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        debug_canvas.SetActive(false);
        debug_on = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) ToggleDebug();

        debug_text.text = "";
        debug_text.text += $"End early timer: {GameManager.Instance.World.end_early_timer.current_time}";
        debug_text.text += $"\nSpawn delay timer: {GameManager.Instance.CustomerManager.spawn_delay_timer.current_time}";
        debug_text.text += $"\nSpawn timer: {GameManager.Instance.CustomerManager.spawn_timer.current_time}";
        debug_text.text += $"\nServe timer: {GameManager.Instance.Player.ServeTimer.current_time}";
        debug_text.text += served_text;
        debug_text.text += earnings_text;
        debug_text.text += "\n\n Customers:";

        for (int i = GameManager.Instance.CustomerManager.patience_queue.Count - 1; i >= 0; i--) {
            debug_text.text += $"\n [Customer {i}] Patience timer: {GameManager.Instance.CustomerManager.patience_queue[i].current_time}";
        }
    }

    void ToggleDebug() {
        debug_on = !debug_on;
        debug_canvas.SetActive(debug_on);
    }

    void SetServedText(int served) => served_text = $"\nServed: {served}";
    void SetEarningsText(decimal earnings) => earnings_text = $"\nEarnings: {earnings}";
}
