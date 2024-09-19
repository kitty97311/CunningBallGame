using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public enum RewardChecker { none, gems, coins, quaterCoins, doubleCoins};
public class GoogleAdsMediation : MonoBehaviour
{
    public static event Action OnNoAdsAvailable;
    public static event Action OnCoinsAwarded;
    public static event Action OnGemsAwarded;

    UITimerHandler uiTimerHandler;

    [Header("Admob Ads")]
    [SerializeField] string AdmobAppKey = "";
    [SerializeField] string AdmobBanner_Key = "";
    [SerializeField] string AdmobInterstitial_Key = "";
    [SerializeField] string AdmobRewarded_Key = "";

    private BannerView bannerView;
    private InterstitialAd interstitial;
    public RewardedAd rewardedAd;

    public static GoogleAdsMediation instance;

    [HideInInspector]
    public bool isRewardAvailable = false, isBannerShow = false;
    
    public RewardChecker whatToReward;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            MobileAds.Initialize(initStatus => { });
        }
    }
    void Start()
    {
        uiTimerHandler = FindObjectOfType<UITimerHandler>();
        MobileAds.Initialize(initStatus => { });
        RequestInterstitial();
        RequestRewarded();
    }

    //Implement code to execute when the loadCallback event triggers:


    private void RequestBanner()
    {
        AdSize adSize = new AdSize(320, 50);
        this.bannerView = new BannerView(AdmobBanner_Key, AdSize.Banner, AdPosition.Top);
        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        //Called when the user should be rewarded for interacting with the ad.
        // this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;
        AdRequest request = new AdRequest.Builder().Build();
        // Create an empty ad request.

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    public void RequestRewarded()
    {
        this.rewardedAd = new RewardedAd(AdmobRewarded_Key);
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        //Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        //Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        //Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        //  Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        //  Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }
    private void RequestInterstitial()
    {

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(AdmobInterstitial_Key);
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoadInt;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpenedInt;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosedInt;
        // Create an empty ad request.
        //this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        //Called when the user should be rewarded for interacting with the ad.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //.....................
        MonoBehaviour.print("HandleAdLoaded event received");
        isBannerShow = true;
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //...........................................Recvert................................................................

        //MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
        //                   + args.Message);
        //isBannerShow = false;

        OnNoAdsAvailable?.Invoke();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {

        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    public void HandleOnAdFailedToLoadInt(object sender, AdFailedToLoadEventArgs args)
    {

    }
    public void HandleOnAdOpenedInt(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosedInt(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        RequestInterstitial();
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
        isRewardAvailable = true;
    }
    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");

    }
    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args);
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        isRewardAvailable = false;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {

        Debug.Log("Reward Closed");
        RequestRewarded();

    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        Debug.Log("Reward Earn");
        AllRewardedAds_Success();
    }

    public void ShowAdmobBanner()
    {
        if (!isAdsPurchased())
        {
            if (this.bannerView != null)
            {
                this.bannerView.Show();
            }
            else
            {
                RequestBanner();
            }
        }
    }
    public void HideBanner()
    {
        if (this.bannerView != null)
        {
            this.bannerView.Hide();
        }
    }
    public void Show_Interstital()
    {
        if (!isAdsPurchased())
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
            else
            {
                RequestInterstitial();
            }
        }
    }
    public void ShowRewardedVideoForGems()
    {
        print("Reward function call...");
        if (!uiTimerHandler.canGetGems)
        {
            OnNoAdsAvailable?.Invoke();
            return;
        }
        if (this.rewardedAd.IsLoaded())
        {
            whatToReward = RewardChecker.gems;
            this.rewardedAd.Show();
            OnGemsAwarded?.Invoke();
        }
        else
        {
            RequestRewarded();
            OnNoAdsAvailable?.Invoke();
        }
    }
    public void ShowRewardedVideoCoins()
    {
        print("Reward function call...");
        //Debug.Log(uiTimerHandler.canGetCoins);
        if (!uiTimerHandler.canGetCoins)
        {
            OnNoAdsAvailable?.Invoke();
            return;
        }
        if (this.rewardedAd.IsLoaded())
        {
            whatToReward = RewardChecker.coins;
            this.rewardedAd.Show();
            OnCoinsAwarded?.Invoke();
        }
        else
        {
            RequestRewarded();
            OnNoAdsAvailable?.Invoke();
        }
    }
    public void ShowRewardedVideo25Coins()
    {
        print("Reward function call...");
        if (this.rewardedAd.IsLoaded())
        {
            whatToReward = RewardChecker.doubleCoins;
            this.rewardedAd.Show();
            OnCoinsAwarded?.Invoke();
        }
        else
        {
            RequestRewarded();
            OnNoAdsAvailable?.Invoke();
        }
    }
    public void AllRewardedAds_Success()
    {
        if(whatToReward == RewardChecker.coins)
        {
            PlayerInstance.playerInstance.AddCoins(50);
        }
        else if (whatToReward == RewardChecker.gems)
        {
            PlayerInstance.playerInstance.AddGems(1);
        }
        else if (whatToReward == RewardChecker.doubleCoins)
        {
            GameOverDataPublisher gameOverDataPublisher = FindObjectOfType<GameOverDataPublisher>();
            PlayerInstance.playerInstance.AddCoins(gameOverDataPublisher.GetCurrentPrizeAmount());
            Debug.Log("Coins added " + gameOverDataPublisher.GetCurrentPrizeAmount());
        }
        else if (whatToReward == RewardChecker.quaterCoins)
        {
            GameOverDataPublisher gameOverDataPublisher = FindObjectOfType<GameOverDataPublisher>();
            PlayerInstance.playerInstance.AddCoins(gameOverDataPublisher.GetCurrentLevelFees() / 4);
        }
    }

    public bool isAdsPurchased()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
