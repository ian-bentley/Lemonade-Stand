using TMPro;
using UnityEngine;

public class LemonadeStand : MonoBehaviour
{
    int customers = 12;
    float price = 0.25f;
    float cash = 0f;

    [SerializeField] TextMeshProUGUI cash_text;

    public void StartDay()
    {
        float earnings = customers * price;
        cash += earnings;
        cash_text.text = $"${cash}";
        Debug.Log($"Earnings: ${earnings}");
    }
}
