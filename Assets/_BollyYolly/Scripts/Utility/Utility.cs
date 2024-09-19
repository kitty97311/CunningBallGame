using UnityEngine;

public class Utility
{
    /// <summary>
    /// Converts the large amount 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static string NumberToWordConverted(long amount)
    {
        string amountConverted;
        float converted;
        if (amount >= 1000000 && amount <= 999999999)
        {
            converted = amount / 1000000;
            converted = Mathf.Round(converted * 100f) / 100f;
            amountConverted = converted.ToString() + Constants.MILLIONS_SHORT_FORM;
        }
        else
        if (amount >= 1000000000 && amount <= 999999999999)
        {
            converted = amount / 1000000000;
            converted = Mathf.Round(converted * 100f) / 100f;
            amountConverted = converted.ToString() + Constants.BILLIONS_SHORT_FORM;
        }
        else
        if (amount >= 1000000000000 && amount <= 999999999999999)
        {
            converted = amount / 1000000000000;
            converted = Mathf.Round(converted * 100f) / 100f;
            amountConverted = converted.ToString() + Constants.TRILLIONS_SHORT_FORM;
        }
        else
        if (amount >= 1000000000000000 && amount <= 999999999999999999)
        {
            converted = amount / 1000000000000000;
            converted = Mathf.Round(converted * 100f) / 100f;
            amountConverted = converted.ToString() + Constants.QUADRILLIONS_SHORT_FORM;
        }
        else
        if (amount >= 999999999999999999 + 1)
        {
            converted = amount / 1000000000000000000;
            converted = Mathf.Round(converted * 100f) / 100f;
            amountConverted = converted.ToString() + Constants.QUADRILLIONS_SHORT_FORM;
        }
        else return amount.ToString();

        return amountConverted;
    }
}
