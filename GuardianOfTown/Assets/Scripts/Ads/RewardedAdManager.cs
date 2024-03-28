using GoogleMobileAds.Api;
using System;
using TMPro;
using UnityEngine;

public class RewardedAdManager : MonoBehaviour
{
    private string _adUnitId = "ca-app-pub-5808337888205967/6015102178";  //  Testing ad "ca-app-pub-3940256099942544/5224354917"
    private RewardedAd _rewardedAd;
    [SerializeField] private GameObject _soundSettingsManager;
    [SerializeField] private TextMeshProUGUI _ShowErrorText;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        LoadRewardedAd();
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
                SubscribeEvents();
            });
        //RegisterEventHandlers(_rewardedAd);
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
        UnsubscribeEvents();
        _rewardedAd.Destroy();
    }

    private void AdOpened()
    {
        Debug.Log("Rewarded ad full screen content opened.");
        var audioSources = _soundSettingsManager.GetComponentsInChildren<AudioSource>();
        foreach (var audioSource in audioSources)
        {
            audioSource.Pause();
        }
    }

    private void AdClosed()
    {
        Debug.Log("Rewarded ad full screen content closed.");
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

    private void AdFailed(AdError error)
    {
        Debug.LogError("Rewarded ad failed to open full screen content " + "with error : " + error);
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

    private void SubscribeEvents()
    {
        _rewardedAd.OnAdFullScreenContentFailed += AdFailed;
        _rewardedAd.OnAdFullScreenContentOpened += AdOpened;
        _rewardedAd.OnAdFullScreenContentClosed += AdClosed;
    }

    private void UnsubscribeEvents()
    {
        _rewardedAd.OnAdFullScreenContentFailed -= AdFailed;
        _rewardedAd.OnAdFullScreenContentOpened -= AdOpened;
        _rewardedAd.OnAdFullScreenContentClosed -= AdClosed;
    }

}
