using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class RankingSceneManager : MonoBehaviour
{
    [SerializeField] Button buttonPageChange;
    [SerializeField] TMP_Text titleText;
    [SerializeField] GameObject nameDlg;
    [SerializeField] Button nameEntyOKButton;
    [SerializeField] TMP_InputField InputFieldName;
    [SerializeField] string[] titleStrings;
    [SerializeField] GameObject playFabLoginManager;
    [SerializeField] GameObject playFabRankingManager;
    [SerializeField] UnityEvent updateReaderBoradEvent;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] string movieFileName;
    int currentPage = 0;
    int dataMode = 0; //トップ5か、プレイヤーの周辺か
    void Start()
    {
        GeneralManager.instance.FadeIn(2f);
        playFabLoginManager.GetComponent<PlayFabLogin>().Login();
        SoundManager.instance.SoundMuteCheck();

        MoviePlay();
        //ライフが減らないようにフラグ設定
        GeneralManager.instance.SetMachineChanged();
    }

    public void OnRankingCloseButtonDown()
    {
        GeneralManager.instance.ChageScene(GeneralManager.lastSceneName, 2f, 0, 1);


    }

    public void OnPageChangeButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();

        if (currentPage == 0)
        {
            buttonPageChange.transform.localScale = -Vector3.one;
            titleText.text = titleStrings[1];
            currentPage = 1;
        }
        else
        {
            buttonPageChange.transform.localScale = Vector3.one;
            titleText.text = titleStrings[0];
            currentPage = 0;

        }
        GetRankingData();

    }

    public void OnNameInputButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        nameDlg.SetActive(true);
        nameEntyOKButton.interactable = false;
    }

    public void OnNameInputOKButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        //PlayFabに名前を登録
        UpdataUserName();

        nameDlg.SetActive(false);
    }

    public void OnNameInputCancelButtonDown()
    {
        SoundManager.instance.PlayButtonSECancel();
        nameDlg.SetActive(false);
    }

    public void OnPrayFabLoginSuccess()
    {
        //////Debug.Log("ログイン成功イベントをキャッチ");
        updateReaderBoradEvent.Invoke();
    }

    public void OnTop5ButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();

        dataMode = 0;
        GetRankingData();

    }

    public void OnMyRankingButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();

        dataMode = 1;
        GetRankingData();
    }

    void GetRankingData()
    {
        if (dataMode == 0)
            playFabRankingManager.GetComponent<PlayFabRainkingManager>().GetLeaderboard(currentPage);
        else
            playFabRankingManager.GetComponent<PlayFabRainkingManager>().GetLeaderboardAroundPlayer(currentPage);

    }
    //名前の登録///////////////////////////////////////////////////////////////////////////////////////
    void UpdataUserName()
    {
        playFabRankingManager.GetComponent<PlayFabRainkingManager>().SetUserName(InputFieldName.text);
    }

    public void NameInputFieldValueChane()
    {
        if (InputFieldName.text.Length >= 3)
        {
            nameEntyOKButton.interactable = true;
        }      

    }
    //動画の再生//////////////////////////////////////////////////////////////////////
    void MoviePlay()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, movieFileName);

        videoPlayer.url = path;
        videoPlayer.Play();

    }
}



