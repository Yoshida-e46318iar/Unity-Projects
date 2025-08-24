using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class HoleDoorSceneManager : MonoBehaviour
{

    [SerializeField] GameObject startMsgManager;
    [SerializeField] Canvas startMsgCanvas;
    int bgmNumber = 12;
    private void Start()
    {


        GeneralManager.instance.FadeIn(2.5f);
        if (GeneralManager.isStartMsgShowed == 0)
        {
            startMsgCanvas.gameObject.SetActive(true);
            startMsgManager.GetComponent<StartMsgManager>().ShowMsg();
            GeneralManager.instance.ShowDefCanvas(false);
            GeneralManager.isStartMsgShowed = 1;
            GeneralManager.instance.SaveGameData();
        }
        else
        {
            SoundManager.instance.SoundMuteCheck();
            SoundManager.instance.PlayBGMNumber(bgmNumber);
            SoundManager.instance.FadeInBGM();

        }
    }
    public void OnRightButtonDown()
    {
        //倉庫へ
        GotoNextRoom(7);



    }
    public void OnLeftButtonDown()
    {
       //オフィスへ
        GotoNextRoom(8);

    }

    public void OnGotoTitleButtonDown()
    {
        GotoNextRoom(0);
    }
    public void OnMsgSkipButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        startMsgManager.GetComponent<StartMsgManager>().StopMsg();
        SoundManager.instance.PlayBGMNumber(bgmNumber);
    }

    void GotoNextRoom(int index)
    {
        SoundManager.instance.PlayButtonSEOK();
        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();
        GeneralManager.instance.HideItemDlg();
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(index), 2f, 0, 1);
        GeneralManager.lastSceneName = SceneManager.GetActiveScene().name;
    }



}



