using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreManager : MonoBehaviour
{

    [Header("Store Screen")]
    public TextMeshProUGUI storeCoinsText;
    public TextMeshProUGUI storeGemsText;

    [Header("Purchase Screen")]
    public TextMeshProUGUI purchaseCoinsText;
    public TextMeshProUGUI purchaseGemsText;

    [Header("Home Screen")]
    public TextMeshProUGUI homeScreenCoinsText;
    public TextMeshProUGUI homeScreenGemsText;

    public readonly int kStackOfCoins = 100;
    public readonly int kBundleOfCoins = 1000000000;
    public readonly int kStackOfGems = 1;
    public readonly int kBundleOfGems = 10;

    private void OnEnable()
    {
        ShowUpdatedCoins(PlayerInstance.playerInstance.playerData.Coins);
        ShowUpdatedGems(PlayerInstance.playerInstance.playerData.Gems);
        PlayerInstance.playerInstance.GameCoinsUpdated += ShowUpdatedCoins;
        PlayerInstance.playerInstance.GameGemsUpdated += ShowUpdatedGems;
    }

    private void OnDisable()
    {
        PlayerInstance.playerInstance.GameCoinsUpdated -= ShowUpdatedCoins;
        PlayerInstance.playerInstance.GameGemsUpdated -= ShowUpdatedGems;
    }

    #region Gems Addition


    public void AddStackOfGems()
    {
        PlayerInstance.playerInstance.AddGems(kStackOfGems);
    }

    public void AddBuldeOfGems()
    {
        PlayerInstance.playerInstance.AddGems(kBundleOfGems);
    }

    public void ShowUpdatedGems(int amount)
    {
        storeGemsText.text = /*Constants.PREFIX_GEMS_LABEL +*/ amount.ToString();
        purchaseGemsText.text = /*Constants.PREFIX_GEMS_LABEL +*/ amount.ToString();
        homeScreenGemsText.text = /*Constants.PREFIX_GEMS_LABEL +*/ amount.ToString();
    }
    #endregion

    #region Coins Addition

    public void AddStackOfCoins()
    {
        PlayerInstance.playerInstance.AddCoins(kStackOfCoins);
    }

    public void AddBuldeOfCoins()
    {
        PlayerInstance.playerInstance.AddCoins(kBundleOfCoins);
    }

    public void ShowUpdatedCoins(long amount)
    {
        storeCoinsText.text = /*Constants.PREFIX_COINS_LABEL +*/ Utility.NumberToWordConverted(amount).ToString();
        purchaseCoinsText.text = /*Constants.PREFIX_COINS_LABEL +*/ Utility.NumberToWordConverted(amount).ToString();
        homeScreenCoinsText.text = /*Constants.PREFIX_COINS_LABEL +*/ Utility.NumberToWordConverted(amount).ToString();
    }
    #endregion
}
