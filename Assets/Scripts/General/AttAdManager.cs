using System.Runtime.InteropServices;
using UnityEngine;
using GoogleMobileAds.Api;

public class ATTAdManager : MonoBehaviour
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void RequestTrackingAuthorization();
#endif

    void Start()
    {
#if UNITY_IOS
        RequestTrackingAuthorization(); // ATT�_�C�A���O��\��
#endif
        //Invoke(nameof(InitializeAds), 1f);
    }

    //void InitializeAds()
    //{
    //    MobileAds.Initialize(initStatus =>
    //    {
    //        // Banner / Interstitial / Rewarded�L�������[�h
    //    });
    //}
}
