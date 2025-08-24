using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpecSheetManager : MonoBehaviour
{
    [SerializeField] GameObject specSheetParent;
    [SerializeField] TMP_Text[] specSheetTexts;
    [SerializeField] float moveStep;
    [SerializeField] Button[] sheetButton;
    [SerializeField] Button missionStartButton;
    [SerializeField] GameObject adManager;


    int currentSheetNum = 0;

    void Start()
    {
        UpdateFirstPos();
        UpdateSpecSheetColor();
        MissionStartButtonEnable();
        LRButtonCheckEnable();

        SetupSpecText();
    }

    void LRButtonCheckEnable()
    {
        if (currentSheetNum == 0)
        {
            sheetButton[0].interactable = false;
            sheetButton[1].interactable = true;
        }
        else if (currentSheetNum == 9)
        {
            sheetButton[0].interactable = true;
            sheetButton[1].interactable = false;

        }
        else
        {
            sheetButton[0].interactable = true;
            sheetButton[1].interactable = true;
        }

    }

    public void OnLRButtonDown(int direction)
    {
        if (currentSheetNum >= 0 && currentSheetNum <= 9)
        {
            currentSheetNum += -direction;
            SoundManager.instance.PlayButtonSEOK();
        }

            float currentX = specSheetParent.transform.localPosition.x;
        specSheetParent.transform.DOLocalMoveX(currentX + moveStep * direction, 0.1f);

        //Debug.Log("currentSpecNum= " + currentSheetNum);

        if (currentSheetNum == 0)
            sheetButton[0].interactable = false;

        else if (currentSheetNum == 9)
            sheetButton[1].interactable = false;

        else
        {
            sheetButton[0].interactable = true;
            sheetButton[1].interactable = true;

        }
        MissionStartButtonEnable();

    }

    public void OnGotoMissionSceneButtonDown()
    {
        if (GeneralManager.instance.CheckShowInterAd())
            ShowInterstitialAd();
        else
            GoNextScene();

    }

    void ShowInterstitialAd()
    {

        adManager.GetComponent<AdMobManager>().ShowInterstitialAdWithTimeout();
    }

    public void OnInterStitialAdClose()
    {
        GoNextScene();

    }

    void GoNextScene()
    {
        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();

        GeneralManager.currentMachineNum = currentSheetNum;
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(2), 2f, 0, 1);
        SoundManager.instance.PlayButtonSEOK();
    }


    public void SpecSheetEnable(int index,int enable)
    {
               
        Transform childTransform=specSheetParent.transform.GetChild(index);

        Color color = childTransform.GetComponent<Image>().color;

        if (enable==0)
            color.a = 1.0f;
        else
            color.a = 0.1f;

        childTransform.GetComponent<Image>().color = color;
    }

    public void UpdateSpecSheetColor()
    {
        for (int i = 0; i < 10; i++)
        {
            SpecSheetEnable(i, GeneralManager.machineEnable[i]);

        }

    }

    void MissionStartButtonEnable()
    {
        if (GeneralManager.machineEnable[currentSheetNum]==0)
            missionStartButton.interactable = true;
        else
            missionStartButton.interactable = false;

    }

    void UpdateFirstPos()
    {

        int count = GeneralManager.machineEnable.Length;

        for (int i = 0; i < count; i++)
        {
            if (GeneralManager.machineEnable[i] == 0)
            {
                currentSheetNum = i;
                break;
            }
            
        }

        float currentX = specSheetParent.transform.localPosition.x;
        specSheetParent.transform.DOLocalMoveX(currentX + moveStep * -currentSheetNum, 0f);

    }

    void SetupSpecText()
    {

        for (int i = 0;i < specSheetTexts.Length; i++)
        {

            if (specSheetTexts[i] != null)
                specSheetTexts[i].GetComponent<TMP_Text>().text = GetSpecText(i);


        }


    }

    string GetSpecText(int index)
    {

        SpecDataObj currentSpecData = GeneralManager.instance.GetSpecData(index);
        string payout="";

        payout = $"{currentSpecData.payouts[0]}&{currentSpecData.payouts[2]}&{currentSpecData.payouts[3]}" +
            $"&{currentSpecData.payouts[7]}";

        string round = $"{currentSpecData.rounddatas[0]}&{currentSpecData.rounddatas[1]}&" +
            $"{ currentSpecData.rounddatas[2]}&{currentSpecData.rounddatas[3]}&"+
            $"{currentSpecData.rounddatas[4]}";

        string roundWeight = $"{currentSpecData.picWeight[0]}:{currentSpecData.picWeight[1]}:"+
            $"{currentSpecData.picWeight[2]}:{currentSpecData.picWeight[3]}:"+
            $"{currentSpecData.picWeight[4]}";

        string ct1 = $"{currentSpecData.jitandataL[0]},{currentSpecData.jitandataL[1]}," +
            $"{currentSpecData.jitandataL[2]},{currentSpecData.jitandataL[3]},"+
            $"{currentSpecData.jitandataL[4]}";

        string ct2 = $"{currentSpecData.jitandataH[0]},{currentSpecData.jitandataH[1]}," +
            $"{currentSpecData.jitandataH[2]},{currentSpecData.jitandataH[3]}," +
            $"{currentSpecData.jitandataH[4]}";

        string type = currentSpecData.machineType.ToString("D2");

        string str = $"Machine No.{currentSpecData.number} , Type : {type}\r" +
            $"\n●賞球:{payout}\r\n●ラウンド:{round}\r\n             ={roundWeight}" +
            $"\r\n●チャンスタイム回数\r\n　初回\r\n　　{ct1}={roundWeight}" +
            $"\r\n　チャンスタイム中\r\n　　{ct2}={roundWeight}";

        return str;
        
    }

    public void EnableMachine()
    {
        GeneralManager.machineEnable[currentSheetNum] = 0;
        GeneralManager.instance.SaveGameData();
        UpdateSpecSheetColor();
        MissionStartButtonEnable();
    }

}
