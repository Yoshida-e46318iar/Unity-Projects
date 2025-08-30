using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class AdMobManager : MonoBehaviour
{
    [SerializeField] UnityEvent<int> rewardAdShowEvents;
    [SerializeField] UnityEvent interstirialAdClosedEvents;
    [SerializeField] float _adLoadTimeout ; // �ő�ҋ@���ԁi�b�j
    private InterstitialAd _interstitialAd;
    private bool _isInterstitialReady = false;
    private string _currentSceneName;
     RewardedAd _rewardedAd;
   // private RewardedInterstitialAd _rewardedInterstitialAd;

    void Start()
    {

        // ���C���X���b�h�Ŏ擾
        _currentSceneName = SceneManager.GetActiveScene().name;

        //Debug.Log("currentSceneName="+_currentSceneName);

        MobileAds.Initialize(initStarus => {

            if (_currentSceneName == "MissionCompleteScene"|| _currentSceneName == "MachineSelectScene")
            {
                LoadInterstitialAd();
            }
            else
            {
                LoadRewardedAd();
            }
  
        });
    }

    //�����[�h�L��//////////////////////////////////////////////////////////////////////////////////
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-1850040616476651/6481342669 ";// "; ca-app-pub-3940256099942544/5224354917
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
  private string _adUnitId = "unused";
#endif


    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }


        //Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    //Debug.LogError("Rewarded ad failed to load an ad " +
                    //               "with error : " + error);
                    return;
                }

                //Debug.Log("Rewarded ad loaded with response : "
                //          + ad.GetResponseInfo());

                _rewardedAd = ad;

                _rewardedAd.OnAdFullScreenContentClosed += () =>
                {
                    //Debug.Log("Rewarded ad full screen content closed.");
                    HandleRewardedAdClosed();

                };
            });
    }
    public void ShowRewardedAd()
    {
        //const string rewardMsg =
        //    "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            if (!Application.isFocused)
            {
                //Debug.LogWarning("�A�v�����o�b�N�O���E���h���Ȃ̂ŁA�L����\�����܂���B");

                // ���i�s�����邽�߁A�����ł�Invoke����
                MainThreadDispatcher.Enqueue(() =>
                {
                    rewardAdShowEvents?.Invoke(1000);
                });

                return;
            }

            _rewardedAd.Show((Reward reward) =>
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    rewardAdShowEvents?.Invoke((int)reward.Amount);
                });

                //Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        else
        {
            //Debug.LogWarning("Rewarded Ad is not ready yet.");

            // ���L���������ł��ĂȂ��Ƃ����A�i�s������
            MainThreadDispatcher.Enqueue(() =>
            {
                rewardAdShowEvents?.Invoke(1000);
            });
        }
    }

    private void HandleRewardedAdClosed()
    {
        //Debug.Log("Rewarded ad was closed.");



    }





    //�C���^�[�X�e�B�V�����L��////////////////////////////////////////////////////////////////////////////////
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId_Intst = "ca-app-pub-1850040616476651/4415298553";// "; ca-app-pub-3940256099942544/1033173712
#elif UNITY_IPHONE
  private string _adUnitId_Intst = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId_Intst = "unused";
#endif



    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
            _isInterstitialReady = false;
        }

        var adRequest = new AdRequest();

        InterstitialAd.Load(_adUnitId_Intst, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    //Debug.LogError("Interstitial ad failed to load: " + error);
                    return;
                }

               // Debug.Log("Interstitial ad loaded: " + ad.GetResponseInfo());
                _interstitialAd = ad;
                _isInterstitialReady = true;

                _interstitialAd.OnAdFullScreenContentClosed += HandleAdClosed;
                _interstitialAd.OnAdFullScreenContentFailed += HandleAdFailed;
            });
    }

    //public void ShowInterstitialAd()
    //{
    //    if (_interstitialAd != null && _interstitialAd.CanShowAd())
    //    {
    //        //Debug.Log("Showing interstitial ad.");
    //        _interstitialAd.Show();
    //    }
    //    else
    //    {
    //        //Debug.LogError("Interstitial ad is not ready yet.");
    //    }
    //}

    public void ShowInterstitialAdWithTimeout()
    {
        StartCoroutine(WaitAndShowInterstitial());
    }
    IEnumerator WaitAndShowInterstitial()
    {
        float elapsed = 0f;
        while (!_isInterstitialReady && elapsed < _adLoadTimeout)
        {
            yield return null;
            elapsed += Time.deltaTime;
        }

        if (_isInterstitialReady && _interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
            _isInterstitialReady = false; // �\����̓t���O��������
        }
        else
        {
            Debug.Log("Interstitial not ready after waiting " + _adLoadTimeout + "s");
        }
    }
    private void HandleAdClosed()
    {
        //Debug.Log("�L���������܂���");

        interstirialAdClosedEvents.Invoke();


        // �ēǂݍ��݁i����ɔ�����j
        LoadInterstitialAd();
    }
    private void HandleAdFailed(AdError error)
    {
        //Debug.LogWarning("�L���\�����s: " + error.GetMessage());

        interstirialAdClosedEvents.Invoke();
        // �K�v�Ȃ�ēǂݍ���
        LoadInterstitialAd();
    }


}
