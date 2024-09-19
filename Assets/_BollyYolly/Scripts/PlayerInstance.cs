using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInstance : MonoBehaviour
{
    public string playerName;
    public int totalMatchesPlayed;
    public int totalMatchesWon;
    public int currentLevelNumber;
    public static PlayerInstance playerInstance;

    //TODO Make this as property if we can stop calling his method directly
    internal PlayerData playerData;

    #region Events

    public Action<long> GameCoinsUpdated;
    public Action<int> GameGemsUpdated;

    #endregion


    private void Awake()
    {
        playerInstance = this;
        DontDestroyOnLoad(this);
        LoadPlayer();
    }
    public void SavePlayer()
    {
        //Debug.Log("Saving");
        SaveSystem.SavePlayer(playerData);
    }

    public void LoadPlayer()
    {
        //Debug.Log("Loading");
        playerData = SaveSystem.LoadPlayer();
        if (playerData == null)
        {
            LoadDefaultPlayerValues();
            return;
        }
        playerName = playerData.playerName;
        currentLevelNumber = playerData.currentLevelNumber;
    }

    public void LoadDefaultPlayerValues()
    {
        playerData = new PlayerData(this);
        //{
        //    playerName = Constants.DEFAULT_PLAYER_NAMES[UnityEngine.Random.Range(0, Constants.DEFAULT_PLAYER_NAMES.Count)]
        //};
    }


    #region Currency Management

    public void AddCoins(long amount)
    {
        playerData.AddCoins(amount);
        UpdateSystemCoins();
    }

    public void DeductCoins(int amount)
    {
        playerData.DeductCoins(amount);
        UpdateSystemCoins();
    }

    //For Updating on UI and other parts
    private void UpdateSystemCoins()
    {
        if (GameCoinsUpdated != null) { GameCoinsUpdated.Invoke(playerData.Coins); }
        SavePlayer();
    }

    internal void AddGems(int amount)
    {
        playerData.AddGems(amount);
        UpdateSystemGems();
    }
    internal void RemoveGems(int amount)
    {
        playerData.DeductGems(amount);
        UpdateSystemGems();
    }

    //For Updating on UI and other parts
    private void UpdateSystemGems()
    {
        if (GameGemsUpdated != null) { GameGemsUpdated.Invoke(playerData.Gems); }
        SavePlayer();
    }

    #endregion


}