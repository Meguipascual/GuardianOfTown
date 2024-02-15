using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using TMPro;

public class AdsManager : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;
    [SerializeField] private GameObject _soundSettingsManager;
    [SerializeField] private TextMeshProUGUI _debugText;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        LoadInterstitial();
    }

    IEnumerator ShowDebugText(string text)
    {
        _debugText.text = text;
        _debugText.gameObject.SetActive(true);
        yield return new WaitForSeconds (3);
        _debugText.gameObject.SetActive(false);
    }

    public void LoadInterstitial()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        StartCoroutine(ShowDebugText("Loading the interstitial ad."));
        Debug.Log("Loading the interstitial ad.");
        var adRequest = new AdRequest();
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("interstitial ad failed to load an ad " +
                               "with error : " + error);
                StartCoroutine(ShowDebugText("interstitial ad failed to load an ad " +
                               "with error : " + error));
                return;
            }

            Debug.Log("Interstitial ad loaded with response : "
                      + ad.GetResponseInfo());
            StartCoroutine(ShowDebugText("Interstitial ad loaded with response : "
                      + ad.GetResponseInfo()));

            _interstitialAd = ad;
        });
        RegisterEventHandlers(_interstitialAd);
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            StartCoroutine(ShowDebugText("Showing interstitial ad."));

            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            StartCoroutine(ShowDebugText("Interstitial ad is not ready yet."));
        }
    }

    private void DestroyInterstitial()
    {
        _interstitialAd.Destroy();
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {

            Debug.Log("Interstitial ad full screen content opened.");
            StartCoroutine(ShowDebugText("Interstitial ad full screen content opened."));
            var audioSources = _soundSettingsManager.GetComponentsInChildren<AudioSource>();
            foreach (var audioSource in audioSources)
            {
                audioSource.Pause();
            }
        };

        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            StartCoroutine(ShowDebugText("Interstitial ad full screen content closed."));
            DestroyInterstitial();
            LoadInterstitial();
            var audioSources = _soundSettingsManager.GetComponentsInChildren<AudioSource>();
            foreach (var audioSource in audioSources)
            {
                audioSource.UnPause();
            }
            //
        };

        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            StartCoroutine(ShowDebugText("Interstitial ad failed to open full screen content " +
                           "with error : " + error));
            DestroyInterstitial();
            LoadInterstitial();
            var audioSources = _soundSettingsManager.GetComponentsInChildren<AudioSource>();
            foreach (var audioSource in audioSources)
            {
                audioSource.UnPause();
            }
        };
    }
    
}
