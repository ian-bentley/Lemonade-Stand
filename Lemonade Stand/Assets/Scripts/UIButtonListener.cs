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

    public event Action OnSupplyShopButtonClicked;
    public event Action OnStartButtonClicked;

    public void SupplyShopButtonClicked() {
        OnSupplyShopButtonClicked?.Invoke();
    }

    public void StartButtonClicked() {
        OnStartButtonClicked?.Invoke();
    }
}
