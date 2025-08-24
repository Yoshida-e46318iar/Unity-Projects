using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Events;

public class MissionFailSceneManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] string movieFileName;
    [SerializeField] GameObject gotoTitleDlg;
    [SerializeField] GameObject msgDlg;
    [SerializeField] GameObject rewardAdManager;
    [SerializeField] PrizeManager prizeManager;
    [SerializeField] int goldRechargeCount;
    [SerializeField] int lifeRewardCount;

    void Start()
    {
        GeneralManager.instance.FadeIn(2f);
        SoundManager.instance.SoundMuteCheck();
        GeneralManager.lastSceneName = SceneManager.GetActiveScene().name;

        MoviePlay();
        gotoTitleDlg.SetActive(false);
        msgDlg.SetActive(false);

        //ライフが減らないようにフラグ設定
        GeneralManager.instance.SetMachineChanged();

        //少し待ってから加算を開始
        DOVirtual.DelayedCall(
             delay: 17f, //何秒後に実行するか
             callback: () => AdDialogShow()//遅延処理

        );

    }


    public void OnGotoTitleButtonDown()
    {
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(0), 2f, 0, 1);
        SoundManager.instance.PlayButtonSEOK();
    }

    void AdDialogShow()
    {

        msgDlg.SetActive(true);
        msgDlg.GetComponent<MsgDlgManager>().DlgShow("広告動画を視聴して、1000Gを獲得しますか?", OnYesButtonDown, OnNoButtonDown);
        SoundManager.instance.PlayButtonSEOK();
    }
     void OnYesButtonDown()
    {
        //リワード広告を表示
        rewardAdManager.GetComponent<AdMobManager>().ShowRewardedAd();
        SoundManager.instance.PlayButtonSEOK();


    }
    void OnNoButtonDown()
    {
        gotoTitleDlg.SetActive(true);
        SoundManager.instance.PlayButtonSECancel();
    }

    public void OnAdClose()
    {
        prizeManager.AddPrize(goldRechargeCount,lifeRewardCount);

        SoundManager.instance.PlayButtonSEOK();

    }

    void MoviePlay()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, movieFileName);

        videoPlayer.url = path;
        videoPlayer.Play();

    }


}
