public class Inventory
{
    public int LemonsCount { get; private set; }
    public int SugarCount { get; private set; }
    public int IceCount { get; private set; }
    public int CupsCount { get; private set; }

    public Inventory()
    {
        LemonsCount = 0;
        SugarCount = 0;
        IceCount = 0;
        CupsCount = 0;

        UIManager.Instance.SetInventoryLemonsCountText(LemonsCount);
        UIManager.Instance.SetInventorySugarCountText(LemonsCount);
        UIManager.Instance.SetInventoryIceCountText(LemonsCount);
        UIManager.Instance.SetInventoryCupsCountText(LemonsCount);
    }

    public bool HasBatchStock(Recipe recipe)
    {
        return LemonsCount >= recipe.LemonsCount && SugarCount >= recipe.SugarCount;
    }
    public bool HasServingStock(Recipe recipe)
    {
        return IceCount >= recipe.IceCount && CupsCount > 0;
    }

    public void ExpireStock()
    {
        LemonsCount = 0;
        IceCount = 0;

        UIManager.Instance.SetInventoryLemonsCountText(LemonsCount);
        UIManager.Instance.SetInventoryIceCountText(LemonsCount);
    }

    public void UseBatchStock(Recipe recipe)
    {
        if (HasBatchStock(recipe))
        {
            UseLemons(recipe.LemonsCount);
            UseSugar(recipe.SugarCount);
        }
    }

    public void UseServingStock(Recipe recipe)
    {
        if (HasServingStock(recipe))
        {
            UseIce(recipe.IceCount);
            UseCup();
        }
    }

    public void AddLemons(int amount)
    {
        if (LemonsCount + amount <= 999) LemonsCount += amount;
        UIManager.Instance.SetInventoryLemonsCountText(LemonsCount);
    }

    public void AddSugar(int amount)
    {
        if (SugarCount + amount <= 999) SugarCount += amount;
        UIManager.Instance.SetInventorySugarCountText(SugarCount);
    }

    public void AddIce(int amount)
    {
        if (IceCount + amount <= 999) IceCount += amount;
        UIManager.Instance.SetInventoryIceCountText(IceCount);
    }

    public void AddCups(int amount)
    {
        if (CupsCount + amount <= 999) CupsCount += amount;
        UIManager.Instance.SetInventoryCupsCountText(CupsCount);
    }

    public void UseLemons(int amount)
    {
        if (LemonsCount - amount >= 0) LemonsCount -= amount;
        UIManager.Instance.SetInventoryLemonsCountText(LemonsCount);
    }

    public void UseSugar(int amount)
    {
        if (SugarCount - amount >= 0) SugarCount -= amount;
        UIManager.Instance.SetInventorySugarCountText(SugarCount);
    }

    public void UseIce(int amount)
    {
        if (IceCount - amount >= 0) IceCount -= amount;
        UIManager.Instance.SetInventoryIceCountText(IceCount);
    }

    public void UseCup()
    {
        if (CupsCount > 0) CupsCount--;
        UIManager.Instance.SetInventoryCupsCountText(CupsCount);
    }
}
