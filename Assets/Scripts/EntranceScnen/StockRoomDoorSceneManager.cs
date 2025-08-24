using UnityEngine;
using UnityEngine.SceneManagement;
public class StockRoomDoorSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GeneralManager.instance.FadeIn(2.5f);
        int bgmNumber = 12;
        SoundManager.instance.SoundMuteCheck();
        SoundManager.instance.PlayBGMNumber(bgmNumber);
        SoundManager.instance.FadeInBGM();
    }

    public void OnRightButtonDown()
    {
        //ホールへ
        GotoNextRoom(6);
    }

    public void OnGotoOfficeButtonDown()
    {
        //ホールへ
        GotoNextRoom(8);
    }


    public void OnGotoTitleButtonDown()
    {
        GotoNextRoom(0);
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
