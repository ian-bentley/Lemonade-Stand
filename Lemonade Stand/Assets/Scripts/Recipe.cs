using System;
using UnityEngine;

public class Recipe
{
    public static event Action<int> OnLemonsCountChanged;
    public static event Action<int> OnSugarCountChanged;
    public static event Action<int> OnIceCountChanged;

    private int _lemons_count;
    private int _sugar_count;
    private int _ice_count;

    public int LemonsCount {
        get => _lemons_count;
        private set {
            _lemons_count = value;
            OnLemonsCountChanged?.Invoke(_lemons_count);
        }
    }

    public int SugarCount {
        get => _sugar_count;
        private set {
            _sugar_count = value;
            OnSugarCountChanged?.Invoke(_sugar_count);
        }
    }

    public int IceCount {
        get => _ice_count;
        private set {
            _ice_count = value;
            OnIceCountChanged?.Invoke(_ice_count);
        }
    }

    public int ServingsPerBatch { get; set; }

    public Recipe() {
        LemonsCount = 0;
        SugarCount = 0;
        IceCount = 0;
    }

    public void IncreaseLemons() {
        if (LemonsCount + 1 <= 99) LemonsCount++;
    }

    public void DecreaseLemons() {
        if (LemonsCount > 0) LemonsCount--;
    }
}
