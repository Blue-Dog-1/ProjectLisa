using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdMob : MonoBehaviour
{
    private const string adUnitId = "ca-app-pub-5222691241639146~5367257038";
    private const string interstitialAdId = "ca-app-pub-3940256099942544/1033173712";
    private const string BanerAdID = "ca-app-pub-3940256099942544/6300978111";

    private InterstitialAd interstitialAd;

    private BannerView bannerView;

    [SerializeField] int minThresholdsAds;
    [SerializeField] int maxThresholdsAds;

    private void Awake()
    {
        Events.Finish += Finish;
    }

    private void Start()
    {
        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

#if UNITY_ANDROID

        deviceIds.Add("9dc88a82bc0b48db8a6e45dbe96f159a");
#endif

        RequestConfiguration requestConfiguration = new RequestConfiguration
        .Builder()
        .SetTestDeviceIds(deviceIds)
        .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        MobileAds.Initialize(adUnitId); 
        RequestInterstitiaAd();

        this.RequestBanner();


        if (Events.ShowAdsInNexLevel) { 
            ShowIntegrationAd();
            Events.ShowAdsInNexLevel = false;
            PlayerPrefs.SetInt("Last Impression Counter", 1);
        }
    }
    void Finish()
    {
        #region last impression counter
        var LIC = PlayerPrefs.GetInt("Last Impression Counter", 1);
        if (LIC >= minThresholdsAds && RandomClamp(LIC, maxThresholdsAds))
        {
            Events.ShowAdsInNexLevel = true;
        }
        else
        {
            PlayerPrefs.SetInt("Last Impression Counter", LIC + 1);
        }

        #endregion
    }
    bool RandomClamp(int value, int max)
    {
        if (UnityEngine.Random.Range(value, max) == value)
            return true;
        else return false;
    }


    void RequestInterstitiaAd()
    {
        interstitialAd = new InterstitialAd(interstitialAdId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);

        interstitialAd.OnAdLoaded += this.HandlerOnAdLoaded;
        interstitialAd.OnAdOpening += this.HandlerOnAdOpening;
        interstitialAd.OnAdClosed += this.HandlerOnAdClosed;

    }
    private void RequestBanner()
    {
        this.bannerView = new BannerView(BanerAdID, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);

    }

    public void ShowIntegrationAd()
    {
        if(interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }


    }
    
    private void OnDestroy()
    {
        DestroyIntegrationAd();

        interstitialAd.OnAdLoaded -= this.HandlerOnAdLoaded;
        interstitialAd.OnAdOpening -= this.HandlerOnAdOpening;
        interstitialAd.OnAdClosed -= this.HandlerOnAdClosed;
    }

    private void HandlerOnAdClosed(object sender, EventArgs e)
    {
        interstitialAd.OnAdLoaded -= this.HandlerOnAdLoaded;
        interstitialAd.OnAdOpening -= this.HandlerOnAdOpening;
        interstitialAd.OnAdClosed -= this.HandlerOnAdClosed;

        RequestInterstitiaAd(); 
    }

    private void HandlerOnAdOpening(object sender, EventArgs e)
    {
    }

    private void HandlerOnAdLoaded(object sender, EventArgs e)
    {
    }

    public void DestroyIntegrationAd()
    {
        interstitialAd.Destroy();
    }
}
