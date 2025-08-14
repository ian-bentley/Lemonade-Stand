using System;
using UnityEngine;

public class Inventory
{
    public static event Action<int> OnLemonsCountChanged;
    public static event Action<int> OnSugarCountChanged;
    public static event Action<int> OnIceCountChanged;
    public static event Action<int> OnCupsCountChanged;

    private int _lemons_count;
    private int _sugar_count;
    private int _ice_count;
    private int _cups_count;

    public int LemonsCount {
        get => _lemons_count;
        set {
            _lemons_count = Mathf.Clamp(value, 0, 999);
            OnLemonsCountChanged?.Invoke(_lemons_count);
        }
    }

    public int SugarCount {
        get => _sugar_count;
        set {
            _sugar_count = Mathf.Clamp(value, 0, 999);
            OnSugarCountChanged?.Invoke(_sugar_count);
        }
    }

    public int IceCount { 
        get => _ice_count;
        set {
            _ice_count = Mathf.Clamp(value, 0, 999);
            OnIceCountChanged?.Invoke(_ice_count);
        }
    }

    public int CupsCount { 
        get => _cups_count;
        set {
            _cups_count = Mathf.Clamp(value, 0, 999);
            OnCupsCountChanged?.Invoke(_cups_count);
        }
    }

    public Inventory() {
        LemonsCount = 0;
        SugarCount = 0;
        IceCount = 0;
        CupsCount = 0;
    }

    public bool HasBatchStock(Recipe recipe) => LemonsCount >= recipe.LemonsCount && SugarCount >= recipe.SugarCount;
    public bool HasServingStock(Recipe recipe) => IceCount >= recipe.IceCount && CupsCount > 0;

    public void ExpireStock() {
        LemonsCount = 0;
        IceCount = 0;
    }

    public void UseBatchStock(Recipe recipe) {
        if (HasBatchStock(recipe)) {
            UseLemons(recipe.LemonsCount);
            UseSugar(recipe.SugarCount);
        }
    }

    public void UseServingStock(Recipe recipe) {
        if (HasServingStock(recipe)) {
            UseIce(recipe.IceCount);
            UseCup();
        }
    }

    public void AddLemons(int amount) {
        LemonsCount += amount;
    }

    public void AddSugar(int amount) {
        SugarCount += amount;
    }

    public void AddIce(int amount) {
        IceCount += amount;
    }

    public void AddCups(int amount) {
        CupsCount += amount;
    }

    public void UseLemons(int amount) {
        LemonsCount -= amount;
    }

    public void UseSugar(int amount) {
        SugarCount -= amount;
    }

    public void UseIce(int amount) {
        IceCount -= amount;
    }

    public void UseCup() {
        CupsCount--;
    }
}
