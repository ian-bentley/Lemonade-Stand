using System;
using UnityEngine;

public class UIButtonListener : MonoBehaviour {
    public static UIButtonListener Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static event Action OnSupplyShopButtonClicked;
    public static event Action OnStartButtonClicked;
    public static event Action OnMenuButtonClicked;
    public static event Action OnBuyButtonClicked;
    public static event Action OnIncreaseButtonClicked;
    public static event Action OnDecreaseButtonClicked;

    public void SupplyShopButtonClicked() {
        OnSupplyShopButtonClicked?.Invoke();
        OnBuyButtonClicked?.Invoke();
    }

    public void StartButtonClicked() {
        OnStartButtonClicked?.Invoke();
        OnMenuButtonClicked?.Invoke();
    }

    public void IncreaseRecipeLemonsClicked() {
        OnIncreaseButtonClicked?.Invoke();
    }

    public void DecreaseRecipeLemonsClicked() {
        OnDecreaseButtonClicked?.Invoke();
    }
}
