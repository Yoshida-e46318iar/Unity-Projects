using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MachienSelectSceneManager : MonoBehaviour
{
    [SerializeField] TMP_Text missionText;
    [SerializeField] TMP_Text titleText;
    [SerializeField] Button startButton;
    [SerializeField] Button outroomButton;
    void Start()
    {
        GeneralManager.instance.FadeIn(1.5f);
        SoundManager.instance.SoundMuteCheck();
        SoundManager.instance.PlayBGM(3);
        SoundManager.instance.FadeInBGM();
        UpDateMissionText();


        if (!GeneralManager.instance.CheckMachineChange()&&!GeneralManager.instance.CheckLife()) { 
            startButton.gameObject.SetActive(false);
            outroomButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    public void OnGotoTitleButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(0), 2f, 0, 1);
        GeneralManager.lastSceneName = SceneManager.GetActiveScene().name;
    }

    public void OnGotoEntranceButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(6), 2f, 0, 1);
        GeneralManager.lastSceneName = SceneManager.GetActiveScene().name;
    }



    void UpDateMissionText()
    {
        if (GeneralManager.currentMissionNum != 4&&GeneralManager.currentMissionNum != 9)
            titleText.text = "Misson " + (GeneralManager.currentMissionNum + 1).ToString();
        else
            titleText.text = "Final Mission";

        missionText.text = GeneralManager.instance.GetCurrentMissionData().missionTitle;

        //Debug.Log("CurrentMissionNum=" + GeneralManager.currentMissionNum);
    }

}
