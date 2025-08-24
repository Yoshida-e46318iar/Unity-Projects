using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;
using System.Runtime.ConstrainedExecution;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.Windows;
using System.Security.Cryptography;
using static GeneralManager;
using UnityEngine.Rendering.Universal;
using System.Drawing;
using Unity.Mathematics;

public class GeneralManager : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] string[] sceneNames;
    [SerializeField] MissionDataObj[] missionDataObjs;

    [SerializeField] GameObject rankingButton;
    [SerializeField] Image itemImage; //�\���I�u�W�F�N�g
    [SerializeField] Sprite[] itemSprites;//�\������摜
    [SerializeField] GameObject itemMsg;//�\������e�L�X�g�I�u�W�F�N�g
    [SerializeField] string[] itemMsgText;//�\������e�L�X�g
    [SerializeField] GameObject itemDlg;
    [SerializeField] Canvas defCanvas;

    [SerializeField] TMP_Text toalOut_text;
    [SerializeField] TMP_Text amountGold_text;
    [SerializeField] SpecDataObj[] specDataObjs;
    [SerializeField] int defaultGoldAmount;
    [SerializeField] Button itemDlgButton;
    [SerializeField] int currentMission;
    [SerializeField] GameObject itemDlgCtrl;
    [SerializeField] TMP_Text itemDlgButtonText;

    [SerializeField] TMP_Text lifeText;
    [SerializeField] TMP_Text lifeRemainText;
    [SerializeField] int lifeRewardCount;
    GameObject springEffectTimer;
    GameObject medicineEffectTimer;

    public static int currentMissionNum ;
    public static GeneralManager instance;
    public static int currentMachineNum = 0;
    public static int totalOut;
    public static int amountGold;
    public static int clearedMissionNum=0;
    public static int mochidama=0;
    public static int[] machineEnable=new int[10];
    public static string lastSceneName = "";
    public static string currentSceneName = "";
    public static int gameStatus = 0;//0:�ʏ�,1:���Z,2:�哖��

    public static int isSavaDataExist = 0;
    public static int entranceSceneDiriction = 1;

    public static int isStartMsgShowed=0;

    public static int[,] itemAcquireds=new int[16,5]; //�A�C�e���̋L���� 16��ށ~3
    public static int[] questConditions=new int[16];
    //0:�z�[���̃h�A,1:�I�t�B�X�̃h�A,2:�q�ɂ̃h�A,

    public static int isShowInterAdCount = 0;

    //�A�C�e���̕ۑ��p
    string saveStr = "";
    string loadStr = "";

    //Life�Ǘ�
    [SerializeField] int lifeCountDefault;
    int lifeRemainCount;

    bool isMachineChange = false;

    public enum ItemName
    {
        FlashLight,
        HoleKey,
        Oil,
        Hummer,
        ToolBoxPicture,
        Spring,
        OfficeKeyNumber,
        Medicine,
        OfficeLockkerKey,
        Magnet,
        BankNumber,
        USBMemory,
        MachineEnableTicket,
    } 


    Dictionary<string, bool> itemDlgButtonEnable = new Dictionary<string, bool>();
    Dictionary<string, bool> rankingButtonEnable = new Dictionary<string, bool>();
    Dictionary<string, List<string>> usableLocationsByItem = new Dictionary<string, List<string>>
    {
        { "FlashLight", new List<string> { "StockRoomScene" } },
        { "HoleKey", new List<string> { "HoleDoorScene" } },
        { "Oil", new List<string> { "StockRoomScene" } },
        { "Hummer", new List<string> { "GameScene" } },
        { "ToolBoxPicture", new List<string> { "StockRoomScene" } },
        { "Spring", new List<string> { "GameScene" } },
        { "OfficeKeyNumber", new List<string> { "OfficeScene" } },
        { "Medicine", new List<string> { "GameScene" } },
        { "OfficeLockkerKey", new List<string> { "OfficeScene" } },
        { "Magnet", new List<string> { "GameScene" } },
        { "BankNumber", new List<string> { "OfficeScene" } },
        { "USBMemory", new List<string> { "GameScene" } },
        { "MachineEnableTicket", new List<string> { "MachineSelectScene" } },
    };

    Dictionary<string, bool> isConditionClear = new Dictionary<string, bool>
    {
        {"HoleDoorLock",false },
        {"StockRoomDoorLock",true },
        {"OilUsed",true },
        {"OfficeRoomDoorLock",false},
        {"OfficeRoomLockkerLock",false},
        {"HummerUsed",false },
        {"SpringUsed",false},
        {"MedicineUsed",false },
        {"HikidashiLock",false },
        {"OfficeLockkerKey",false },
        {"MagnetUsed",false },
        {"KinkoLock",false },
        {"SpringItemShow",false },

    };

    Dictionary<string, int> itemMaxCount = new Dictionary<string, int>
    {
        {"FlashLight",1 },
        {"HoleKey",1 },
        {"Oil",1 },
        {"Hummer",3},
        {"ToolBoxPicture",1},
        {"Spring",2 },
        {"OfficeKeyNumber",1},
        {"Medicine",2 },
        {"DeskKey",1 },
        {"Magnet",3 },
        {"BankNumber",1 },
        {"USBMemory",1 },
        {"MachineEnableTicket",3 }

    };




    void Awake()
    {
        CheckInstance();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        RankingButtonEanabelDataSetup();
        ItemDlgButtonEanabelDataSetup();

        InitData();

        isSavaDataExist = PlayerPrefs.GetInt("isSavaDataExist");

        lastSceneName = SceneManager.GetActiveScene().name;

        itemImage.gameObject.SetActive(false);
        itemMsg.SetActive(false);

        lifeRemainText.text = "-";
        ResetMachineChanged();

        


    }

    void ItemDlgButtonEanabelDataSetup()
    {
        itemDlgButtonEnable.Add("TitleScene", false);
        itemDlgButtonEnable.Add("MachineSelectScene", true);
        itemDlgButtonEnable.Add("GameScene", true);
        itemDlgButtonEnable.Add("MissionCompleteScene", false);
        itemDlgButtonEnable.Add("MissionFailScene", false);
        itemDlgButtonEnable.Add("RankingScene", false);
        itemDlgButtonEnable.Add("HoleDoorScene", true);
        itemDlgButtonEnable.Add("StockRoomDoorScene", true);
        itemDlgButtonEnable.Add("OfficeDoorScene", true);
    }
    void RankingButtonEanabelDataSetup()
    {
        rankingButtonEnable.Add("TitleScene", true);
        rankingButtonEnable.Add("MachineSelectScene", false);
        rankingButtonEnable.Add("GameScene", true);
        rankingButtonEnable.Add("MissionCompleteScene", false);
        rankingButtonEnable.Add("MissionFailScene", false);
        rankingButtonEnable.Add("RankingScene", false);
        rankingButtonEnable.Add("HoleDoorScene", false);
        rankingButtonEnable.Add("OfficeDoorScene", false);
        rankingButtonEnable.Add("StockRoomDoorScene", false);
    }

    void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //�Q�[���f�[�^�̏�����/////////////////////////////////////
   public void InitData()
    {
        for (int i = 0; i < machineEnable.Length; i++)
        {
            machineEnable[i] = 0;
        }

        amountGold = defaultGoldAmount;
        totalOut= 0;
        currentMissionNum = 0;
        currentMachineNum = 0;
        isSavaDataExist = 0;
        for (int i = 0; i < itemAcquireds.GetLength(0); i++)
        {
            for(int j=0;j<itemAcquireds.GetLength(1);j++)
               itemAcquireds[i,j] = 0;

            questConditions[i]= 0;
        }


        //�����̏�����
        var keys = new List<string>(isConditionClear.Keys);
        foreach (var key in keys)
        {
            isConditionClear[key] = false;
        }
        isConditionClear["StockRoomDoorLock"] = true;

        isStartMsgShowed = 0;
        UpdateToalOut();
        UpdateAmountGold();
            
        itemDlgCtrl.GetComponent<ItemDlgCtrl>().Init();

        lifeRemainCount = lifeCountDefault;
        ResetMachineChanged();
        UpdateLifeRemainCount();



    }
    //�Q�[���f�[�^�̕ۑ�//////////////////////////////////////////
    public void SaveGameData()
    {
        isSavaDataExist = 1;
        PlayerPrefs.SetInt("isSavaDataExist", 1);

        PlayerPrefs.SetInt("TotalOut", totalOut);
        PlayerPrefs.SetInt("CurrentMissionNum", currentMissionNum);
        PlayerPrefs.SetInt("ClearedMissionNum", clearedMissionNum);
        PlayerPrefs.SetInt("Gold", amountGold);
        PlayerPrefs.SetInt("isStartMsgShowed", 1);
        PlayerPrefs.SetInt("lifeRemainCount", lifeRemainCount);
        PlayerPrefs.SetInt("isStartMsgShowed ", isStartMsgShowed);


        for (int i = 0; i < machineEnable.Length; i++)
        {
            PlayerPrefs.SetInt("Machine"+i.ToString(), machineEnable[i]);
        }

        for (int i = 0; i < questConditions.Length; i++)
        {
            //PlayerPrefs.SetInt("itemAcquireds" + i.ToString(), itemAcquireds[i]);
            PlayerPrefs.SetInt("questConditions" + i.ToString(), questConditions[i]);
        }

        SaveItemAcquiredsData();

        foreach (var pair in isConditionClear)
        {
            // bool �� int �ɕϊ����ĕۑ��itrue:1, false:0�j
            PlayerPrefs.SetInt(pair.Key, pair.Value ? 1 : 0);
        }


    }

    void SaveItemAcquiredsData()
    {
        string saveStr = ""; // �� ���[�J���ϐ��ŏ��������Ȃ��ƑO��̃f�[�^���c��
        for (int i = 0; i < itemAcquireds.GetLength(0); i++)
        {
            for (int j = 0; j < itemAcquireds.GetLength(1); j++)
            {
                saveStr += itemAcquireds[i, j];
                if (j < itemAcquireds.GetLength(1) - 1) saveStr += ","; // ���؂�
            }
            if (i < itemAcquireds.GetLength(0) - 1) saveStr += ";"; // �s��؂�
        }

        PlayerPrefs.SetString("itemAcquiredsArray", saveStr);
        PlayerPrefs.Save();
    }
    

    //�Q�[���f�[�^�̓ǂݍ���////////////////////////////////////////
    public void LoadGameData()
    {
        isSavaDataExist=PlayerPrefs.GetInt("isSavaDataExist");
        totalOut = PlayerPrefs.GetInt("TotalOut");
        currentMissionNum = PlayerPrefs.GetInt("CurrentMissionNum");
        clearedMissionNum = PlayerPrefs.GetInt("ClearedMissionNum");
        amountGold = PlayerPrefs.GetInt("Gold");
        isStartMsgShowed = PlayerPrefs.GetInt("isStartMsgShowed");
        lifeRemainCount = PlayerPrefs.GetInt("lifeRemainCount");
        isStartMsgShowed = PlayerPrefs.GetInt("isStartMsgShowed ");


        for (int i = 0; i < machineEnable.Length; i++)
        {
            machineEnable[i] = PlayerPrefs.GetInt("Machine" + i);
 
        }



        for (int i = 0; i < questConditions.Length; i++)
        {
            questConditions[i]= PlayerPrefs.GetInt("questConditions" + i.ToString());
        }
        LoadItemAcquiredsData();


        UpdateToalOut();
        UpdateAmountGold();

        string[] conditionKeys= isConditionClear.Keys.ToArray();

        foreach (string key in conditionKeys)
        {
            // ���݂��Ȃ��ꍇ�̓f�t�H���g�� false�i=0�j
            int value = PlayerPrefs.GetInt(key, 0);
            isConditionClear[key] = (value == 1);
        }

        ResetMachineChanged();
        UpdateLifeRemainCount();

        //////�f�o�b�O�p
        //currentMissionNum = currentMission;

        //////�f�o�b�O�p
        //itemAcquireds[11, 0] = 1;
        //itemAcquireds[1, 0] = 1;
        //itemAcquireds[7, 0] = 1;
        //itemAcquireds[5, 0] = 1;
        //itemAcquireds[8, 0] = 1;

        //amountGold = 25000;

        ////Debug.Log("MissionNum=" + currentMissionNum);

    }
    void LoadItemAcquiredsData()
    {
        string loadStr = PlayerPrefs.GetString("itemAcquiredsArray", "");
        if (string.IsNullOrEmpty(loadStr))
        {
            //Debug.LogWarning("�ۑ��f�[�^������܂���");
            return;
        }

        string[] rows = loadStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        // �ő�񐔂��v�Z
        int maxCols = 0;
        foreach (string row in rows)
        {
            int colCount = row.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (colCount > maxCols) maxCols = colCount;
        }

        // �����Ŕz����쐬�imaxCols���m�肵����j
        int[,] loadedData = new int[rows.Length, maxCols];

        // �f�[�^����
        for (int i = 0; i < rows.Length; i++)
        {
            string[] cols = rows[i].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < cols.Length; j++)
            {
                if (!int.TryParse(cols[j], out loadedData[i, j]))
                {
                    loadedData[i, j] = 0; // �ϊ����s����0
                }
            }
        }

        // �ǂݍ��񂾃f�[�^�� itemAcquireds �ɔ��f�i�T�C�Y����v����ꍇ�̂݁j
        if (loadedData.GetLength(0) == itemAcquireds.GetLength(0) &&
            loadedData.GetLength(1) == itemAcquireds.GetLength(1))
        {
            for (int i = 0; i < itemAcquireds.GetLength(0); i++)
            {
                for (int j = 0; j < itemAcquireds.GetLength(1); j++)
                {
                    itemAcquireds[i, j] = loadedData[i, j];
                }
            }
        }
    }


    //���������Z///////////////////////////////////////////////////////
    public void CheckOut()
    {
        amountGold += mochidama * 4;
        mochidama = 0;
        
        UpdateAmountGold();
    }



    public void SavePartData()
    {
        PlayerPrefs.SetInt("TotalOut", totalOut);
        PlayerPrefs.SetInt("Gold", amountGold);
        isSavaDataExist = 1;
        PlayerPrefs.SetInt("isSavaDataExist", 1);
    }

   


    //�A�E�g�̉��Z//////////////////////////////////////////////////
    public void OutAdd()
    {
        totalOut++;
        UpdateToalOut();
    }


    //�V�[���ڍs/////////////////////////////////////////////////////////////////////
    public void ChageScene(string nextSceneName,float fadeDuration,float alphaValueStart, float alphaValueEnd)
    {


        canvasGroup.alpha = alphaValueStart;
        canvasGroup.DOFade(alphaValueEnd, fadeDuration)
            .OnComplete(() => {
                GotoNextScene(nextSceneName);
                rankingButton.SetActive(rankingButtonEnable[nextSceneName]);
                itemDlgButton.gameObject.SetActive(itemDlgButtonEnable[nextSceneName]);

                if (itemDlg.activeSelf)
                    itemDlg.GetComponent<ItemDlgCtrl>().DlgSetActive(false);

                itemDlgButtonText.text= "�A�C�e����\��";

            } );


    }

    void GotoNextScene(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);

    }

    public string GetSceneName(int index)
    {
        return sceneNames[index];
    }

    public void FadeIn(float duration)
    {
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, duration);


    }
    //�����L���O�V�[��////////////////////////////////////////////////////////////////
    public void OnGotoRankingScene()
    {


        if (lastSceneName=="GameScene")
        {
            GameObject gameSceneManager= GameObject.Find("GameSceneManager");
            gameSceneManager.GetComponent<GameSceneManager>().GotoRankingScene();
        }

        SoundManager.instance.PlayButtonSEOK();
        ChageScene(GeneralManager.instance.GetSceneName(5), 2f, 0, 1);

    }





    //�\��/////////////////////////////////////////////////////////

    public void UpdateToalOut()
    {
        toalOut_text.text = "�����ˋʐ� : "+totalOut.ToString("D7") + "��";

    }

    public void UpdateAmountGold()
    {
        amountGold_text.text = "����GOLD : "+amountGold.ToString("D7") + "G";
    }

    //���݂̃V�[���̎擾/////////////////////////////////////////////
    public string GetCurrnetSceneName()
    {
        return lastSceneName;
    } 



    //�X�y�b�N�f�[�^�̎擾//////////////////////////////////////////////////////////
    public int GetPayoutdata(int index)
    {
        int res = 0;
        res=specDataObjs[currentMachineNum].payouts[index];

        return res;

    }

    //�d�`���[�̒��I�m���̎擾/////////////////////////////////////////
    public int GetDenchyuBunbo()
    {
        int res = 0;
        res = specDataObjs[currentMachineNum].fbunbo[gameStatus];

        return res;

    }

    //�X�y�b�N�f�[�^�̎擾//////////////////////////////////////////////
    public SpecDataObj GetCurrentSpecData()
    {
        return specDataObjs[currentMachineNum];

    }

    public SpecDataObj GetSpecData(int index)
    {
        return specDataObjs[index];

    }


    //�~�b�V�����f�[�^///////////////////////////////////////////////////////
    public MissionDataObj GetCurrentMissionData()
    {
        return missionDataObjs[currentMissionNum];
    }

    public MissionDataObj GetLatestClearedMissionData()
    {
        return missionDataObjs[currentMissionNum-1];
    }

    public int GetPrize()
    {
        return missionDataObjs[clearedMissionNum].prize;
    }

    //�A�C�e���\��/////////////////////////////////////////////////////////
    public void ShowItem(int number,float duration)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite=itemSprites[number];

        StartCoroutine(FadeInCoroutine(1.5f, itemImage));
        ShowMsg(itemMsgText[number]);

        DOVirtual.DelayedCall(
             delay:duration, //���b��Ɏ��s���邩
             callback: () => StartCoroutine(FadeOutCoroutine(0.5f,
                              itemImage))//�x������

        );


    }

    public void ShowItem(int itemNumber,int msgNumber, float duration)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = itemSprites[itemNumber];

        StartCoroutine(FadeInCoroutine(1.5f, itemImage));
        ShowMsg(itemMsgText[msgNumber]);

        DOVirtual.DelayedCall(
             delay: duration, //���b��Ɏ��s���邩
             callback: () => StartCoroutine(FadeOutCoroutine(0.5f,
                              itemImage))//�x������

        );


    }

    IEnumerator FadeOutCoroutine(float duration,Image targetImage)
    {
        UnityEngine.Color color = targetImage.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            targetImage.color = new UnityEngine.Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        targetImage.color = new UnityEngine.Color(color.r, color.g, color.b, 0f);
        targetImage.gameObject.SetActive(false);
    }


    IEnumerator FadeInCoroutine(float duration, Image targetImage)
    {
        UnityEngine.Color color = targetImage.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1f, elapsed / duration);
            targetImage.color = new UnityEngine.Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        targetImage.color = new UnityEngine.Color(color.r, color.g, color.b, 1f);
    }

    //�A�C�e���̊l�����̏���///////////////////////////////////////////////////////////
    public void GetItem(int itemNumber,int msgNumber)
    {
        GeneralManager.instance.ShowItem(itemNumber,msgNumber, 5.0f);
        SetItemAcquireds(itemNumber,1);

        GeneralManager.instance.SaveGameData();

        //Spring�̕\������
        if (itemNumber == (int)ItemName.ToolBoxPicture)
        {
            SetCondition("SpringItemShow");
            //Debug.Log("Condition =" + GetCondition("SpringItemShow"));
        }

    }

    public void SetItemAcquireds(int itemNumber,int statusNumber)
    {
        //�����A�C�e��
        if (statusNumber == 1) { //�l������ꍇ
            for (int i = 0; i < itemAcquireds.GetLength(1); i++)
            {
                if (itemAcquireds[itemNumber, i] == 0)
                {
                    itemAcquireds[itemNumber, i] = statusNumber;
                    break;
                }
                


            }
        }
        else//�l���ςŎg�p����ꍇ
        {
            for (int i = 0; i < itemAcquireds.GetLength(1); i++)
            {
                if (itemAcquireds[itemNumber, i] == 1)
                {
                    itemAcquireds[itemNumber, i] = statusNumber;
                    break;
                }

            }

        }

    }


    public void GetItem(string itemName)
    {
        int itemNumber = 77777;
        if (Enum.TryParse(typeof(ItemName),itemName, out object result))
        {
            itemNumber =(int)(ItemName)result;
            //Debug.Log("�������� enum �l: " + itemNumber);
        }
        else
        {
            //Debug.Log("�Y������ enum ��������܂���ł���");
        }

        SetItemAcquireds(itemNumber, 1);
        GeneralManager.instance.SaveGameData();



    }

    //�A�C�e�������݂��āA���łɊl�����Ă���ꍇ��true///////////////////////
    public bool CheckItem(string itemName)
    {
        int itemNumber = 77777;
        bool res = false;
        if (Enum.TryParse(typeof(ItemName), itemName, out object result))
        {
            itemNumber = (int)(ItemName)result;
            
            for(int i = 0; i < itemAcquireds.GetLength(1); i++) { 
                if(itemAcquireds[itemNumber,i] >= 1) { 
                    res = true;
                    break;
                }
                else
                    res = false;
            }
            //Debug.Log("�������� enum �l: " + itemNumber);
        }
        else
        {
               //Debug.Log("�Y������ enum ��������܂���ł���");
        }

        return res;

    }

    public void ShowMsg(string msg)
    {
        itemMsg.SetActive(true);
        itemMsg.GetComponent<MsgTextManager>().ShowMsg(msg);

    }
    public void ShowMsg(int index)
    {
        itemMsg.SetActive(true);
        itemMsg.GetComponent<MsgTextManager>().ShowMsg(itemMsgText[index]);

    }

    public Sprite GetItemSprite(int index)
    {
        return itemSprites[index];
    }

    public void UseItem(int index)
    {
        switch (index)
        {
            case (int)ItemName.Hummer:
                GameObject tempGO=GameObject.FindGameObjectWithTag("HummerManager");
                tempGO.GetComponent<ItemHummerManager>().KugiOpenAnimation();
                break;

            case (int)ItemName.Spring:
                GameObject tempG1 = GameObject.FindGameObjectWithTag("SpringEffectTimer");
                tempG1.GetComponent<EffectTimer>().StartEffect();
                break;

            case (int)ItemName.Medicine:
                GameObject tempG2 = GameObject.FindGameObjectWithTag("MedicineEffectTimer");
                tempG2.GetComponent<EffectTimer>().StartEffect();

                break;
            case (int)ItemName.Magnet:
                GameObject tempGO3 = GameObject.FindGameObjectWithTag("MagnetManager");
                tempGO3.GetComponent<MagnetManager>().VZoneActionStart();

                break;
            case (int)ItemName.USBMemory:
                GameObject tempGO4 = GameObject.FindGameObjectWithTag("USBManager");
                tempGO4.GetComponent<USBMemoryManager>().ItemEffectOn();
                break;
            case (int)ItemName.MachineEnableTicket:
                GameObject tempGO5 = GameObject.FindGameObjectWithTag("SpecSheetManager");
                tempGO5.GetComponent<SpecSheetManager>().EnableMachine();
                break;

            case (int)ItemName.OfficeLockkerKey:
                GeneralManager.instance.SetCondition("OfficeLockkerKey");
                break;



        }

        //0:���l��,1:�l����,2:�g�p��
        SetItemAcquireds(index, 2);

        //�f�o�b�O�p
      //  itemAcquireds[(int)ItemName.USBMemory] = 1;

        if (index == 0)
            SetItemAcquireds(index, 1);
        GeneralManager.instance.SaveGameData();
    }

    public bool CanUseItem(string itemName, string currentLocation)
    {
        if (usableLocationsByItem.TryGetValue(itemName, out List<string> usableLocations))
        {
            return usableLocations.Contains(currentLocation);
        }

        //Debug.Log($"�A�C�e���u{itemName}�v�͑��݂��܂���B");
        return false;
    }

    public void HideItemDlg()
    {
        if (itemDlg.activeSelf)
        {
            itemDlg.SetActive(false);
        }

    }

    //�l���A�C�e���̎g�����///////////////////////////////////////
    public int GetItemCount(int itemNumber)
    {

        int res = 0;
        for (int i = 0; i < itemAcquireds.GetLength(1); i++)
        {
            if (itemAcquireds[itemNumber,i]==1)
                res++;

        }

        return res;
    }

    int GetItemAllCount(int itemNumber)
    {
        int res = 0;
        for (int i = 0; i < itemAcquireds.GetLength(1); i++)
        {
            if (itemAcquireds[itemNumber, i] > 0)
                res++;

        }


        return res;

    }


    public int GetItemRemain(string itemName)
    {
        int res = 0;
        int countMax= itemMaxCount[itemName];
        int itemNumber = 777777;
        int currentCount =0;

        if (Enum.TryParse(typeof(ItemName), itemName, out object result))
        {
            itemNumber = (int)(ItemName)result;

            //���łɊl��������
            currentCount = GetItemAllCount(itemNumber);

            res = countMax - currentCount;

            //Debug.Log("GetItemRemain �������� enum �l: " + itemNumber+"count = "+res);
        }
        else
        {
            //Debug.Log("�Y������ enum ��������܂���ł���");
        }
                

        return res;
    }

    //Life�Ǘ�///////////////////////////////////////////////////////////////////////
    void UpdateLifeRemainCount()
    {
        //��ȉ��ŕ�����ԐF��

        lifeRemainText.text = "�~"+lifeRemainCount.ToString();
        if (lifeRemainCount < 3)
        {
            lifeRemainText.color = UnityEngine.Color.red;
            lifeText.color = UnityEngine.Color.red;
        }
        else
        {
            lifeRemainText.color = UnityEngine.Color.green;
            lifeText.color = UnityEngine.Color.green;
        }

    }
    //���C�t�����邩�̊m�F ����:true,�Ȃ�:false
    public bool CheckLife()
    {
        bool res = true;
        if (lifeRemainCount <= 0)
        {
            res= false;
        }

        return res;
    }

    public int GetLifeDefCount()
    {
        return lifeCountDefault;
    }

    //���C�t�̊l���A����///////////////////
    public void LifeRemainIncDec(int count)
    {
        lifeRemainCount += count;
        SaveGameData();
        UpdateLifeRemainCount();
    }

    public bool CheckMachineChange()
    {
        return isMachineChange;
    }

    public void SetMachineChanged()
    {
        isMachineChange = true;
    }

    public void ResetMachineChanged()
    {
        isMachineChange=false;
    }

    public int  GetLifeCount()
    {
        return lifeRemainCount;
    }

    public void SetLifeCount(int count)
    {
        lifeRemainCount = count;
        SaveGameData();
        UpdateLifeRemainCount();
    }

    public int GetLifeRewardCount()
    {
        return lifeRewardCount;
    }

    //�����Ǘ�////////////////////////////////////////////////////////////

    public void SetCondition(string condition)
    {
        isConditionClear[condition]=true;

        SaveGameData();
    }

    public bool GetCondition(string condition)
    { 
        return isConditionClear[condition]; 
    }


    //���̑�////////////////////////////////////////////////////////////

    public void ShowDefCanvas(bool mode)
    {
        defCanvas.gameObject.SetActive(mode);
        itemDlgButton.gameObject.SetActive(mode);
    }

    public bool CheckShowInterAd()
    {
        bool res=false;

        isShowInterAdCount++;

        if (isShowInterAdCount > 2)
        {
            res = true;
            isShowInterAdCount = 0;

        }


        return res;

    }

}
