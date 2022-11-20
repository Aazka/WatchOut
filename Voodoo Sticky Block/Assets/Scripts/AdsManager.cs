//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds.Api;
//using System;

//public class AdsManager : MonoBehaviour
//{
//    private BannerView bannerView;
//    private InterstitialAd interstitial;
//    public static AdsManager instance;
//    private void Awake()
//    {
//        instance = this;
//        DontDestroyOnLoad(this.gameObject);
//    }
//    public void Start()
//    {
//        // Initialize the Google Mobile Ads SDK.
//        MobileAds.Initialize(initStatus => { });
//        this.RequestBanner();
//        RequestInterstitial();
//    }
//    private void RequestBanner()
//    {
//#if UNITY_ANDROID
//        string adUnitId = "ca-app-pub-8399038048953133/4144139811";
//#elif UNITY_IPHONE
//            string adUnitId = "";
//#else
//            string adUnitId = "unexpected_platform";
//#endif

//        // Create a 320x50 banner at the top of the screen.
//        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

//        // Create an empty ad request.
//        AdRequest request = new AdRequest.Builder().Build();

//        // Load the banner with the request.
//        this.bannerView.LoadAd(request);

//        // Called when an ad request has successfully loaded.
//        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
//        // Called when an ad request failed to load.
//        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
//        // Called when an ad is clicked.
//        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
//        // Called when the user returned from the app after an ad click.
//        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
      
//    }
//    public void HandleOnAdLoaded(object sender, EventArgs args)
//    {
//        MonoBehaviour.print("HandleAdLoaded event received");
//    }

//    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//    {
//        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
//                            + args.ToString());
//    }

//    public void HandleOnAdOpened(object sender, EventArgs args)
//    {
//        MonoBehaviour.print("HandleAdOpened event received");
//    }

//    public void HandleOnAdClosed(object sender, EventArgs args)
//    {
//        MonoBehaviour.print("HandleAdClosed event received");
//    }

//    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
//    {
//        MonoBehaviour.print("HandleAdLeavingApplication event received");
//    }
//    private void RequestInterstitial()
//    {
//#if UNITY_ANDROID
//        string adUnitId = "ca-app-pub-8399038048953133/1983227949";
//#elif UNITY_IPHONE
//        string adUnitId = "";
//#else
//        string adUnitId = "unexpected_platform";
//#endif

//        // Initialize an InterstitialAd.
//        this.interstitial = new InterstitialAd(adUnitId);
//        // Create an empty ad request.
//        AdRequest request = new AdRequest.Builder().Build();
//        // Load the interstitial with the request.
//        this.interstitial.LoadAd(request);
//    }
//    public void ShowInit()
//    {
//        if (this.interstitial.IsLoaded())
//        {
//            this.interstitial.Show();
//        }
//    }
//}
