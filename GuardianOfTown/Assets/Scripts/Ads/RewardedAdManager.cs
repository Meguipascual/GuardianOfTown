using GoogleMobileAds.Api;
using System;
using TMPro;
using UnityEngine;

public class RewardedAdManager : MonoBehaviour
{
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    private RewardedAd _rewardedAd;
    [SerializeField] private GameObject _soundSettingsManager;
    [SerializeField] private TextMeshProUGUI _ShowErrorText;
    private bool _adClosed;
    private bool _adOpened;
    private bool _adFailed;
    private AdError _error;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        LoadRewardedAd();
    }

    private void Update()
    {
        if (_adClosed)
        {
            _adClosed = false;
            AdClosed();
        }

        if (_adOpened)
        {
            _adOpened = false;
            AdOpened();
        }

        if (_adFailed)
        {
            _adFailed = false;
            AdFailed();
        }
    }

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
            });
        RegisterEventHandlers(_rewardedAd);
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    private void DestroyRewarded()
    {
        _rewardedAd.Destroy();
    }

    private void AdOpened()
    {
        var audioSources = _soundSettingsManager.GetComponentsInChildren<AudioSource>();
        foreach (var audioSource in audioSources)
        {
            audioSource.Pause();
        }
    }

    private void AdClosed()
    {
        //Reward archieved I suppose.
        DestroyRewarded();
        LoadRewardedAd();
        var audioSources = _soundSettingsManager.GetComponentsInChildren<AudioSource>();
        foreach (var audioSource in audioSources)
        {
            audioSource.UnPause();
        }
        GameManager.Instance.RevivePlayerReward();
    }

    private void AdFailed()
    {
        DestroyRewarded();
        LoadRewardedAd();
        var audioSources = _soundSettingsManager.GetComponentsInChildren<AudioSource>();
        foreach (var audioSource in audioSources)
        {
            audioSource.UnPause();
        }
        GameManager.Instance.ShowRewardedAdPanel();
        _ShowErrorText.text = "Ad failed";
        _ShowErrorText.color = Color.red;
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            _adOpened = true;
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            _adClosed = true;
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            _adFailed = true;
            _error = error;
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

}
