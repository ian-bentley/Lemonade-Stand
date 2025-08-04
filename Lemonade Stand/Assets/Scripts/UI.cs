using TMPro;
using UnityEngine;

[System.Serializable]
public class UI
{
    [SerializeField] public TextMeshProUGUI cash_text;
    [SerializeField] public TextMeshProUGUI stock_text;
    [SerializeField] public TextMeshProUGUI customers_text;
    [SerializeField] public TextMeshProUGUI day_timer_text;
    [SerializeField] public TextMeshProUGUI served_text;
    [SerializeField] public TextMeshProUGUI day_counter_text;

    public void SetCashText(decimal cash)
    {
        cash_text.text = $"{cash:C}";
    }

    public void SetStockText(int stock)
    {
        stock_text.text = $"{stock}";
    }

    public void SetCustomersText(int customers)
    {
        customers_text.text = $"Customers: {customers}";
    }

    public void SetDayTimerText(float day_timer_duration)
    {
        day_timer_text.text = $"{day_timer_duration}";
    }

    public void SetServedText(int served)
    {
        served_text.text = $"Served: {served}";
    }

    public void SetDayCounterText(int day_count)
    {
        day_counter_text.text = $"Day: {day_count}";
    }
}
