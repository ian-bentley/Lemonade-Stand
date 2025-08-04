using TMPro;
using UnityEngine;

public class DebugTool : MonoBehaviour
{
    [SerializeField] LemonadeStand lemonade_stand;
    [SerializeField] GameObject debug_canvas;
    [SerializeField] TextMeshProUGUI debug_text;
    bool debug_on;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        debug_canvas.SetActive(false);
        debug_on = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) ToggleDebug();
        debug_text.text = "";
        debug_text.text += $"End early timer: {lemonade_stand.World.end_early_timer.current_time}";
        debug_text.text += $"\nSpawn delay timer: {lemonade_stand.CustomerManager.spawn_delay_timer.current_time}";
        debug_text.text += $"\nSpawn timer: {lemonade_stand.CustomerManager.spawn_timer.current_time}";
        debug_text.text += $"\nServe timer: {lemonade_stand.Player.serve_timer.current_time}";
        debug_text.text += $"\nServed: {lemonade_stand.Player.served}";
        debug_text.text += $"\nEarnings: {lemonade_stand.Player.earnings}";
        debug_text.text += "\n\n Customers:";
        for (int i = lemonade_stand.CustomerManager.patience_queue.Count - 1; i >= 0; i--)
        {
            debug_text.text += $"\n [Customer {i}] Patience timer: {lemonade_stand.CustomerManager.patience_queue[i].current_time}";
        }
    }

    void ToggleDebug()
    {
        debug_on = !debug_on;

        debug_canvas.SetActive(debug_on);
    }
}
