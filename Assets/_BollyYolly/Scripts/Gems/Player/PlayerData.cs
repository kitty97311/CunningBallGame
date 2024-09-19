using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    #region Variables

    public int currentLevelNumber;
    public int eligibleForLevel;

    public string playerName;

    public int totalTournamentsWin;
    public int totalMatchesPlayed;
    public int totalMatchesWon;
    public int winningStreak;
    public int totalDeaths;

    private int currnetMusicVolume;
    private int currnetSFXVolume;

    public int musicSetting;
    public int sfxSetting;

    public float sensitivityValue;
    public bool secondStart;

    private long coins = 0;
    private int gems = 0;
    #endregion

    #region Properties

    //Setting properties (optional)
    internal long Coins => coins;
    internal int Gems => gems;

    #endregion


    public PlayerData(PlayerInstance player)
    {
        //playerName = player.playerName;
        //  levelUnlocked = player.levelUnlocked;
    }
    #region Properties
    //TODO: Getters of Coins, gems, player name, gems etc
    #endregion


    #region Currency management Methods

    internal void AddCoins(long amount)
    {
        coins += amount;
       
    }
    internal void DeductCoins(long amount)
    {
        coins -= amount;
        if (coins < 0) coins = 0;
    }

    internal void AddGems(int amount)
    {
        gems += amount;
    }
    internal void DeductGems(int amount)
    {
        gems -= amount;
        if (gems < 0) gems = 0;
    }


    #endregion


}
