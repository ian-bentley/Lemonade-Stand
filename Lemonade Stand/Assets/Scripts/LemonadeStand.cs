using System.Collections;
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

    // tracks popularity
    float popularity = 0f;

    // factor at which popularity grows
    float popularity_factor = 0.1f;

    // how many seconds is one day
    float day_duration = 60f;

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
        // set day timer
        float day_timer = day_duration;

        // set serve timer
        float serve_timer = serve_interval;

        // calculate spawn interval based on popularity

        float base_spawn_rate = 0.2f; // TODO make based off attraction

        float growth_spawn_rate = popularity_factor * (Mathf.Pow(popularity, 2.0f) / 100);

        float spawn_rate = base_spawn_rate + growth_spawn_rate;

        float spawn_interval = 1 / spawn_rate;

        // set spawn timer
        float spawn_timer = spawn_interval;

        // tracks earnings for the day
        decimal earnings = 0m;

        // tracks how many served today
        int served = 0;

        // tracks how many customers are waiting
        int customer_queue = 0;

        // reset customer queue and served display
        customers_text.text = $"Queue: {customer_queue}";
        served_text.text = $"Served: {served}";

        while (day_timer > 0f)
        {
            // decrease day and spawn timers
            day_timer -= Time.deltaTime;
            spawn_timer -= Time.deltaTime;

            // update day timer text
            day_timer_text.text = $"{day_timer}";

            // spawn a customer if timer is up and reset timer
            if (spawn_timer <= 0)
            {
                // add customer to queue
                customer_queue++;

                // update customer display
                customers_text.text = $"Queue: {customer_queue}";

                // reset spawn timer
                spawn_timer += spawn_interval;
            }

            // if someone is in queue and you have stock, start serving
            if (customer_queue > 0 && stock > 0)
            {
                serve_timer -= Time.deltaTime;
            }

            // if serving is done
            if (serve_timer <= 0)
            {
                // reset serve timer
                serve_timer += serve_interval;

                // remove them from the queue
                customer_queue--;

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

        // change day counter display
        day_counter_text.text = $"Day: {day_count}";

        // expire stock
        stock = 0;

        // update stock display
        stock_text.text = $"{stock}";

        // calculate popularity change and update popularity

        float normalization_cap = 5; // caps popularity change to +/- 5
        float sum_scores = 0.8f * served; // TODO make based on sum of scores

        // change is based on the scores normalized by number served up to a cap, is 0 if none served
        float popularity_change = served > 0 ? (sum_scores / served) * normalization_cap : 0;

        popularity += popularity_change;
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
