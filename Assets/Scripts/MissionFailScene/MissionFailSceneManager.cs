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

        //���C�t������Ȃ��悤�Ƀt���O�ݒ�
        GeneralManager.instance.SetMachineChanged();

        //�����҂��Ă�����Z���J�n
        DOVirtual.DelayedCall(
             delay: 17f, //���b��Ɏ��s���邩
             callback: () => AdDialogShow()//�x������

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
        msgDlg.GetComponent<MsgDlgManager>().DlgShow("�L��������������āA1000G���l�����܂���?", OnYesButtonDown, OnNoButtonDown);
        SoundManager.instance.PlayButtonSEOK();
    }
     void OnYesButtonDown()
    {
        //�����[�h�L����\��
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
