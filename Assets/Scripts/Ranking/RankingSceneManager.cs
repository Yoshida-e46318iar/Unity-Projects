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
    int dataMode = 0; //�g�b�v5���A�v���C���[�̎��ӂ�
    void Start()
    {
        GeneralManager.instance.FadeIn(2f);
        playFabLoginManager.GetComponent<PlayFabLogin>().Login();
        SoundManager.instance.SoundMuteCheck();

        MoviePlay();
        //���C�t������Ȃ��悤�Ƀt���O�ݒ�
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
        //PlayFab�ɖ��O��o�^
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
        //////Debug.Log("���O�C�������C�x���g���L���b�`");
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
    //���O�̓o�^///////////////////////////////////////////////////////////////////////////////////////
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
    //����̍Đ�//////////////////////////////////////////////////////////////////////
    void MoviePlay()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, movieFileName);

        videoPlayer.url = path;
        videoPlayer.Play();

    }
}



