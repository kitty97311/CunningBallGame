using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
public class UITimerHandler : MonoBehaviour
{
    [Header("Coin/Gems Timer Text")]
    [SerializeField] private Text coinsTimerText;
    [SerializeField] private Text diamondsTimerText;

    DateTime diamondsTimeToUnlock; 
    DateTime coinsTimeToUnlock;


    public bool canGetGems;
    public bool canGetCoins;
    private void OnEnable()
    {
        GoogleAdsMediation.OnGemsAwarded += OnGemsAwarded;
        GoogleAdsMediation.OnCoinsAwarded += OnCoinsAwarded;

        if (PlayerPrefs.HasKey("TimeToCoin"))
        {
            diamondsTimeToUnlock = System.DateTime.Parse(PlayerPrefs.GetString("TimeToCoin"));
        }
        if (PlayerPrefs.HasKey("TimeToDiamonds"))
        {
            coinsTimeToUnlock = System.DateTime.Parse(PlayerPrefs.GetString("TimeToDiamonds"));
        }

        Debug.Log(diamondsTimeToUnlock);
        Debug.Log(coinsTimeToUnlock);
    }

    private void FixedUpdate()
    {
        DisplayTimers();
    }
    void DisplayTimers()
    {
        if (PlayerPrefs.HasKey("TimeToDiamonds"))
        {
            diamondsTimerText.enabled = true;
            int timeLeftD = DateTime.Compare(System.DateTime.Parse(PlayerPrefs.GetString("TimeToDiamonds")),  System.DateTime.Now);
            canGetGems = false;
            if ( timeLeftD <= 0)
            {
                canGetGems = true;
                PlayerPrefs.DeleteKey("TimeToDiamonds");
                diamondsTimerText.enabled = false;
                return;
            }
            diamondsTimerText.text = (System.DateTime.Parse(PlayerPrefs.GetString("TimeToDiamonds")) - System.DateTime.Now).ToString().Substring(0, 8);
        }
        else
        {
            canGetGems = true;
            diamondsTimerText.enabled = false;
        }
        if (PlayerPrefs.HasKey("TimeToCoin"))
        {
            coinsTimerText.enabled = true;
            int timeLeftC = DateTime.Compare(System.DateTime.Parse(PlayerPrefs.GetString("TimeToCoin")), System.DateTime.Now);
            canGetCoins = false;
            if ( timeLeftC <= 0)
            {
                canGetCoins = false;
                PlayerPrefs.DeleteKey("TimeToCoin");
                coinsTimerText.enabled = false;
                return;
            }
            coinsTimerText.text = (System.DateTime.Parse(PlayerPrefs.GetString("TimeToCoin")) - System.DateTime.Now).ToString().Substring(0, 8);
        }
        else
        {
            canGetCoins = true;
            coinsTimerText.enabled = false;
        }
    }
    public void OnCoinsAwarded()
    {
        Debug.Log("Coins Awarded");
        PlayerPrefs.SetString("TimeToCoin", System.DateTime.Now.AddMinutes(1).ToString());
        Debug.Log(PlayerPrefs.GetString("TimeToCoin") + " " + "Time Coins");
    }
    public void OnGemsAwarded()
    {
        Debug.Log("Gems Awarded");
        PlayerPrefs.SetString("TimeToDiamonds", System.DateTime.Now.AddDays(1).ToString());
        Debug.Log(PlayerPrefs.GetString("TimeToDiamonds") + " " + "Time Diamonds");
    }
    void HandleCoinsTimer()
    {

    }
    void HandleGemsTimer()
    {

    }
    void ResetCoinsTimer()
    {

    }
    void ResetGemsTimer()
    {

    }
}
