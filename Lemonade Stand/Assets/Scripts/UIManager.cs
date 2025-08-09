using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] public TextMeshProUGUI cash_text;
    [SerializeField] public TextMeshProUGUI price_text;
    [SerializeField] public TextMeshProUGUI customers_text;
    [SerializeField] public TextMeshProUGUI day_timer_text;
    [SerializeField] public TextMeshProUGUI served_text;
    [SerializeField] public TextMeshProUGUI day_counter_text;
    [SerializeField] public TextMeshProUGUI recipe_lemons_count_text;
    [SerializeField] public TextMeshProUGUI recipe_sugar_count_text;
    [SerializeField] public TextMeshProUGUI recipe_ice_count_text;
    [SerializeField] public TextMeshProUGUI inventory_lemons_count_text;
    [SerializeField] public TextMeshProUGUI inventory_sugar_count_text;
    [SerializeField] public TextMeshProUGUI inventory_ice_count_text;
    [SerializeField] public TextMeshProUGUI inventory_cups_count_text;

    public event Action OnSupplyShopButtonClicked;
    public event Action OnStartButtonClicked;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCashText(decimal cash)
    {
        cash_text.text = $"{cash:C}";
    }

    public void SetPriceText(decimal price)
    {
        price_text.text = $"{price:C}";
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

    public void SetRecipeLemonsCountText(int lemons_count)
    {
        recipe_lemons_count_text.text = $"{lemons_count}";
    }

    public void SetRecipeSugarCountText(int sugar_count)
    {
        recipe_sugar_count_text.text = $"{sugar_count}";
    }

    public void SetRecipeIceCountText(int ice_count)
    {
        recipe_ice_count_text.text = $"{ice_count}";
    }

    public void SetInventoryLemonsCountText(int lemons_count)
    {
        inventory_lemons_count_text.text = $"{lemons_count}";
    }

    public void SetInventorySugarCountText(int sugar_count)
    {
        inventory_sugar_count_text.text = $"{sugar_count}";
    }

    public void SetInventoryIceCountText(int ice_count)
    {
        inventory_ice_count_text.text = $"{ice_count}";
    }

    public void SetInventoryCupsCountText(int cups_count)
    {
        inventory_cups_count_text.text = $"{cups_count}";
    }

    public void SupplyShopButtonClicked()
    {
        OnSupplyShopButtonClicked?.Invoke();
    }

    public void StartButtonClicked()
    {
        OnStartButtonClicked?.Invoke();
    }
}
