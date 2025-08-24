using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OfficeDoorSceneManager : MonoBehaviour
{
    [SerializeField] Camera SubCamera;
    [SerializeField] GameObject doorLockSencer;
    [SerializeField] GameObject doorLockManager;
    [SerializeField] Canvas numberButtonsCanvas;
    [SerializeField] GameObject numberButtonsDomy;
    [SerializeField] Button button_Return;
    void Start()
    {
        GeneralManager.instance.FadeIn(2.5f);
        int bgmNumber = 12;
        SoundManager.instance.SoundMuteCheck();
        SoundManager.instance.PlayBGMNumber(bgmNumber);
        SoundManager.instance.FadeInBGM();


        doorLockManager.GetComponent<DoorLockManager>().ButtonEnable(false);

        numberButtonsCanvas.gameObject.SetActive(false);
    }

    public void OnLeftButtonDown()
    {
        //ÇŸÅ[ÇÈÇ÷
        GotoNextRoom(6);
    }
    public void OnGotoStockRoomButtonDown()
    {
        //ëqå…Ç÷
        GotoNextRoom(7);
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
    public void OnDoorLockClicked()
    {
        doorLockSencer.SetActive(false);
        SubCamera.gameObject.SetActive(true);
        button_Return.gameObject.SetActive(true);
        numberButtonsCanvas.gameObject.SetActive(true);
        numberButtonsDomy.SetActive(false);

        doorLockManager.GetComponent<DoorLockManager>().ButtonEnable(true);

    }

    public void OnReturnButtonDown()
    {
        doorLockSencer.SetActive(true);
        SubCamera.gameObject.SetActive(false);
        button_Return.gameObject.SetActive(false);
        numberButtonsCanvas.gameObject.SetActive(false);
        numberButtonsDomy.SetActive(true);

        doorLockManager.GetComponent<DoorLockManager>().ButtonEnable(false);
        SoundManager.instance.PlayButtonSEOK();
    }
}
