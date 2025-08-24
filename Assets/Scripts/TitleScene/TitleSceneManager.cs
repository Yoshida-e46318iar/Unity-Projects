using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] int sceneIndex;
    [SerializeField] Button buttonContinue;
    [SerializeField] GameObject MsgDlg;
    [SerializeField] GameObject rewardAdManager;
    [SerializeField] PrizeManager prizeManager;
    [SerializeField] int goldRechargeCount;
    [SerializeField] string movieFileName;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GeneralManager.instance.FadeIn(2f);
        GeneralManager.lastSceneName = SceneManager.GetActiveScene().name;
        videoPlayer.SetTargetAudioSource(0, SoundManager.instance.GetAudioSource());
        SoundManager.instance.SoundMuteCheck();

        PlayOpenningMovie();

        buttonContinue.interactable = false;

        if (GeneralManager.isSavaDataExist == 1)
        {
            buttonContinue.interactable = true;
                    
        }

    }

    public void OnStartButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        if (GeneralManager.isSavaDataExist == 1)
        {
            MsgDlg.GetComponent<MsgDlgManager>().DlgShow("�ۑ�����Ă���f�[�^���������܂�", OnOK, OnCancel);
            
        }
        else
            GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(sceneIndex), 2f, 0, 1);



    }

    public void OnContinueButtonDown()
    {
        GeneralManager.instance.LoadGameData();
        SoundManager.instance.PlayButtonSEOK();

        //���C�t���Ȃ��ꍇ�̃��C�t�`���[�W
        if (!GeneralManager.instance.CheckLife())
            LifeRecharge();
        else 
            GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(sceneIndex), 2f, 0, 1);

        //Debug.Log("CurrentMissionNum=" + GeneralManager.currentMissionNum);
    }

    void OnOK()
    {
        GeneralManager.instance.InitData();
        PlayerPrefs.DeleteAll();
        buttonContinue.interactable = false;
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(sceneIndex), 2f, 0, 1);
    }

    void OnCancel()
    {
        

    }

    void LifeRecharge()
    {
        MsgDlg.SetActive(true);
        int count=GeneralManager.instance.GetLifeRewardCount();
        MsgDlg.GetComponent<MsgDlgManager>().DlgShow("����L������������" +
            "\nLIFE��"+ count+"�񕜂��܂��� ? ", OnLifeCharge, OnCancel);
        SoundManager.instance.PlayButtonSEOK();
    }

    void OnLifeCharge()
    {
        //�����[�h�L����\��
        rewardAdManager.GetComponent<AdMobManager>().ShowRewardedAd();
        SoundManager.instance.PlayButtonSEOK();

    }

    public void OnAdClose()
    {
        prizeManager.AddPrize(goldRechargeCount, GeneralManager.instance.GetLifeRewardCount());

        SoundManager.instance.PlayButtonSEOK();

    }

    public void OnRewardDlgOKButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(sceneIndex), 2f, 0, 1);

    }

    void PlayOpenningMovie()
    {

        string path = System.IO.Path.Combine(Application.streamingAssetsPath, movieFileName);

        videoPlayer.url = path;
        videoPlayer.Play();

    }


}
