using UnityEngine;

public class Recipe
{
    public int LemonsCount {  get; private set; }
    public int SugarCount { get; private set; }
    public int IceCount { get; private set; }
    public int ServingsPerBatch { get; set; }

    public Recipe()
    {
        LemonsCount = 0;
        SugarCount = 0;
        IceCount = 0;

        UIManager.Instance.SetRecipeLemonsCountText(LemonsCount);
        UIManager.Instance.SetRecipeSugarCountText(SugarCount);
        UIManager.Instance.SetRecipeIceCountText(IceCount);
    }

    public void IncreaseLemons()
    {
        if (LemonsCount + 1 <= 99)
        {
            LemonsCount++;
            UIManager.Instance.SetRecipeLemonsCountText(LemonsCount);
        }
    }

    public void DecreaseLemons()
    {
        if (LemonsCount > 0)
        {
            LemonsCount--;
            UIManager.Instance.SetRecipeLemonsCountText(LemonsCount);
        }
    }
}
