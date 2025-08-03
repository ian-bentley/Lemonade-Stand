using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LemonadeStand : MonoBehaviour
{
    // how much lemonade costs
    decimal price = 1.50m;

    // your cash
    decimal cash = 20m;
    
    // how much stock you have
    int stock = 0;

    // how much stock costs
    decimal stock_price = 0.5m;

    // time in seconds to serve, MIN is 1.2f
    float serve_interval = 3f;

    // how many seconds is one day
    float day_duration = 180f;

    // tracks the number of days
    int day_count = 1;

    [SerializeField] TextMeshProUGUI cash_text;
    [SerializeField] TextMeshProUGUI stock_text;
    [SerializeField] TextMeshProUGUI customers_text;
    [SerializeField] TextMeshProUGUI day_timer_text;
    [SerializeField] TextMeshProUGUI served_text;
    [SerializeField] TextMeshProUGUI day_counter_text;

    private void Start()
    {
        cash_text.text = $"{cash:C}";
        stock_text.text = $"{stock}";
        customers_text.text = "Customers: 0";
        served_text.text = "Served: 0";
        day_counter_text.text = $"Day: {day_count}";
    }

    public void StartDay()
    {
        StartCoroutine(SellLemonade());
    }

    IEnumerator SellLemonade()
    {
        // set end day to false
        bool end_day = false;

        // set day timer
        float day_timer = day_duration;

        // set serve timer
        float serve_timer = serve_interval;

        // set spawn delay timer
        float spawn_delay_timer = 5f;

        // set max customers 
        int max_customers = 5;

        // set spawn duration for spawn interval
        float spawn_duration = 5f;

        // set spawn interval
        float spawn_interval = spawn_duration / max_customers;

        // set spawn timer
        float spawn_timer = spawn_interval;

        // set end early timer
        float end_early_timer = 5f;

        // tracks earnings for the day
        decimal earnings = 0m;

        // tracks how many served today
        int served = 0;

        // tracks how many customers are waiting
        int customer_queue = 0;

        // tracks customers spawned
        int customers_spawned = 0;

        float customer_patience = 5f;

        List<float> patience_queue = new List<float>();

        // reset customer queue and served display
        customers_text.text = $"Queue: {customer_queue}";
        served_text.text = $"Served: {served}";

        while (!end_day)
        {
            // end day if day timer or end early timer is up
            if (day_timer <= 0 || end_early_timer <= 0) end_day = true;

            // decrease day and spawn timers
            day_timer -= Time.deltaTime;

            // update day timer text
            day_timer_text.text = $"{day_timer}";
            
            // decrease spawn delay timer until it's complete
            if (spawn_delay_timer > 0f)
            {
                spawn_delay_timer -= Time.deltaTime;
            }

            // if spawn delay is done and not at max customers
            if (spawn_delay_timer <= 0f && customers_spawned < max_customers)
            {
                // decrease spawn timer
                spawn_timer -= Time.deltaTime;

                // spawn a customer if timer is up and reset timer
                if (spawn_timer <= 0f)
                {
                    // add customer to queue
                    customer_queue++;

                    // track their patience
                    patience_queue.Add(customer_patience);

                    // track spawn
                    customers_spawned++;

                    // update customer display
                    customers_text.text = $"Queue: {customer_queue}";

                    // reset spawn timer
                    spawn_timer += spawn_interval;
                }
            }

            // decrease and check customer patience
            for (int i = patience_queue.Count - 1; i >= 0; i--)
            {
                // front no longer loses patience
                if (i == 0)
                    continue;

                // decrease this patience timer
                patience_queue[i] -= Time.deltaTime;

                // if patience timer is up
                if (patience_queue[i] <= 0f)
                {
                    // stop tracking their patience
                    patience_queue.RemoveAt(i);

                    // remove them from queue
                    customer_queue--;
                    
                    // update customer queue text
                    customers_text.text = $"Queue: {customer_queue}";
                }
            }

            // if there are no more customers, run end early timer
            if (customer_queue == 0 && customers_spawned == max_customers)
            {
                end_early_timer -= Time.deltaTime;
            }

            // if someone is in queue and you have stock, start serving
            if (customer_queue > 0 && stock > 0)
            {
                serve_timer -= Time.deltaTime;
            }

            // if serving is done
            if (serve_timer <= 0 && customer_queue > 0)
            {
                // reset serve timer
                serve_timer += serve_interval;

                // remove them from the queue
                customer_queue--;

                // stop tracking their patience
                patience_queue.RemoveAt(0);

                // serve one stock
                stock--;

                // track earnings gained
                earnings += price;

                // add to cash
                cash += price;

                // track they've been served
                served++;

                // update cash, customer, and stock display
                cash_text.text = $"{cash:C}";
                customers_text.text = $"Queue: {customer_queue}";
                stock_text.text = $"{stock}";
                served_text.text = $"Served: {served}";
            }

            yield return null; // pause loop till next frame so timers work
        }

        // End of Day

        // Raise day count
        day_count++;

        // change day counter display
        day_counter_text.text = $"Day: {day_count}";

        // expire stock
        stock = 0;

        // update stock display
        stock_text.text = $"{stock}";
    }

    public void BuyStock()
    {
        // if you can afford to buy stock
        if (cash >= stock_price)
        {
            // spend cash
            cash -= stock_price;

            // get more stock
            stock++;

            // update stock and cash display
            stock_text.text = $"{stock}";
            cash_text.text = $"{cash:C}";
        }
    }
}
