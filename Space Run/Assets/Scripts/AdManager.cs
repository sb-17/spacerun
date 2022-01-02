using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private BannerView bannerAd;
    private RewardedAd rewardedAd;
    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;

    public GameObject rewardAdButtonDoubleCoins;
    public GameObject rewardAdButtonExtraLife;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            rewardAdButtonDoubleCoins.SetActive(false);
            rewardAdButtonExtraLife.SetActive(false);
        }

        MobileAds.Initialize(InitializationStatus => { });
        this.RequestAndLoadRewardedAd();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            ShowRewardAdButtons();
    }

    public void RequestBanner()
    {
        if (PlayerPrefs.GetInt("a") == 0)
        {
            string adUnitId = "ca-app-pub-1217351089486176/2812066875";
            this.bannerAd = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
            AdRequest request = new AdRequest.Builder().Build();
            this.bannerAd.LoadAd(request);
        }
    }

    public void DestroyBanner()
    {
        if (PlayerPrefs.GetInt("a") == 0)
            this.bannerAd.Destroy();
    }

    public void RequestAndLoadRewardedAd()
    {
        string adUnitId = "ca-app-pub-1217351089486176/6559740199";

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent.Invoke();
        rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();
        rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent.Invoke();

        AdRequest request = new AdRequest.Builder().AddKeyword("unity-admob-sample").Build();
        rewardedAd.LoadAd(request);
    }

    public void ShowRewardAdButtons()
    {
        if (rewardedAd != null)
        {
            rewardAdButtonDoubleCoins.SetActive(true);
            rewardAdButtonExtraLife.SetActive(true);
        }
    }

    public void ShowRewardedAd(string rewardType)
    {
        if (rewardedAd != null)
        {
            PlayerPrefs.SetString("RewardType", rewardType);
            rewardedAd.Show();
        }
    }

    public void Reward()
    {
        if (PlayerPrefs.GetString("RewardType") == "DoubleCoins")
        {
            PlayerPrefs.SetInt("DoubleCoins", PlayerPrefs.GetInt("DoubleCoins") + 1);
        }
        else if (PlayerPrefs.GetString("RewardType") == "ExtraLife")
        {
            PlayerPrefs.SetInt("ExtraLife", PlayerPrefs.GetInt("ExtraLife") + 1);
        }
    }
}
