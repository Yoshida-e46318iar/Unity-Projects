using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemDlgCtrl : MonoBehaviour
{
    [SerializeField] GameObject itemDlg;
    [SerializeField] GameObject itemLSizePanelBase;
    [SerializeField] GameObject itemLSizePanel;
    [SerializeField] Button itemUseButton;
    [SerializeField] TMP_Text itmeDlgShowButtonText;

    [SerializeField] GameObject[] ItemButtons;
    [SerializeField] GameObject[] ItemButtonsTexts;
    [SerializeField] TMP_Text itemDlgShowButtnText;
    [SerializeField] TMP_Text[] itemCountText;

    [SerializeField] GameObject canvasCameraManager;

    int currentItemNum = 0;
    bool isDlgShowd=false;

    void Start()
    {
        itemLSizePanel.SetActive(false);
        for (int i = 0; i < ItemButtons.Length; i++)
        {
            //GeneralManager.itemAcquireds[1] = 1;
            ShowImage(i);

        }

        

    }

    public void Init()
    {
        for (int i = 0; i < ItemButtons.Length; i++)
        {
            ItemButtonsTexts[i].SetActive(false);

        }
    }
    // Update is called once per frame

    public void OnItemClick(int index)
    {
        itemDlg.SetActive(false);
        itemLSizePanelBase.SetActive(true);
        itemLSizePanel.SetActive(true);
        itemLSizePanel.GetComponent<Image>().sprite = GeneralManager.instance.GetItemSprite(index);
        SoundManager.instance.PlayButtonSEOK();

        currentItemNum = index;
        //Debug.Log("Item " + index + " Clicked");

        if (index == 4 || index == 10)
            itemUseButton.gameObject.SetActive(false);
        else
            itemUseButton.gameObject.SetActive(true);



        if (GeneralManager.instance.CanUseItem(GeneralManager.instance.GetItemSprite(currentItemNum).name,
            SceneManager.GetActiveScene().name))
        {
            //Debug.Log("can use");

            //アイテムがカギの場合/////////////////////////////////
            if(index==1)
            {
                //向いている扉がホールの場合のみボタンを使えるようにする
                if(GeneralManager.entranceSceneDiriction==1)
                    itemUseButton.interactable = true;
                else
                    itemUseButton.interactable = false;
            }
            else
                itemUseButton.interactable = true;

        }
        else
        {

            itemUseButton.interactable = false;

        }

        //Debug.Log("Entrance Direction = " + GeneralManager.entranceSceneDiriction);
    
    }
    public void OnClosebuttonDown()
    {
        DlgSetActive(false);
        SoundManager.instance.PlayButtonSECancel();

    }

    public void ShowImage(int index)
    {
        int count = GeneralManager.instance.GetItemCount(index);
        if (count > 0)
        {
            ItemButtons[index].GetComponent<Image>().sprite = GeneralManager.instance.GetItemSprite(index);
            ItemButtons[index].GetComponent<Image>().color = Color.white;
            ItemButtons[index].GetComponent<Button>().interactable = true;
            ItemButtonsTexts[index].SetActive(false);

            //複数獲得できるアイテムの場合は個数表示を行う
            if (index == 3 || index == 5 || index == 7 || index == 9 || index == 12)
            {
                int i = 0;
                switch (index)
                {
                    case 3:

                        break;
                    case 5:
                        i = 1;
                        break;
                    case 7:
                        i = 2;
                        break;
                    case 9:
                        i = 3;
                        break;
                    case 12:
                        i = 4;
                        break;


                }
                itemCountText[i].gameObject.SetActive(true);
                itemCountText[i].text = "×" + count.ToString();
            }
        }
        else if (GeneralManager.itemAcquireds[index, 0] == 2)
        {
            ItemButtons[index].GetComponent<Image>().sprite = GeneralManager.instance.GetItemSprite(index);
            ItemButtons[index].GetComponent<Image>().color = Color.white;
            ItemButtons[index].GetComponent<Button>().interactable = false;


            if (index == 3 || index == 5 || index == 7 || index == 9 || index == 12)
            {

                int i = 0;
                switch (index)
                {
                    case 3:

                        break;
                    case 5:
                        i = 1;
                        break;
                    case 7:
                        i = 2;
                        break;
                    case 9:
                        i = 3;
                        break;
                    case 12:
                        i = 4;
                        break;


                }
                itemCountText[i].gameObject.SetActive(false);


            }


                ItemButtonsTexts[index].SetActive(true);

       
        }
        else
        {
            ItemButtons[index].GetComponent<Image>().sprite = null;
            ItemButtons[index].GetComponent<Image>().color = Color.black;
            ItemButtons[index].GetComponent<Button>().interactable = false;
        }

    }

    public void OnItemDlgShowButtonDown()
    {
        if (!isDlgShowd) {


            DlgSetActive(true);



        }
        else
        {
            DlgSetActive(false);

        }
    }

    public void DlgSetActive(bool mode)
    {

        if (mode)
        {
            isDlgShowd = true;
            itemDlgShowButtnText.text = "アイテムを非表示";

            itemDlg.SetActive(true);

            SoundManager.instance.PlayButtonSEOK();
            UpdateItemPicture();

        }

        else
        {
            itemDlg.SetActive(false);
            itemDlgShowButtnText.text = "アイテムを表示";
            isDlgShowd = false;
            SoundManager.instance.PlayButtonSECancel();
        }
    }

    public void HideItemDlg()
    {
        isDlgShowd=false;
        itemDlg.SetActive(false);


    }


    public bool GetDlgActive()
    {
        return isDlgShowd;
    }

    public void OnItemLSizePanelCloseButtonDown()
    {
        itemLSizePanelBase.SetActive(false);
        DlgSetActive(true);
        SoundManager.instance.PlayButtonSECancel();
    }

    public void OnItemUseButtonDown()
    {
        itemLSizePanelBase.SetActive(false);
        DlgSetActive(true);
        SoundManager.instance.PlayButtonSEOK();

        GeneralManager.instance.UseItem(currentItemNum);

        int msgIndex = 88888;
        switch (currentItemNum)//アイテムを使った時のテロップの番号
        {
            case 0:
                msgIndex = 11;
                GameObject FlashLight = GameObject.FindWithTag("FlashLight");
                FlashLight.GetComponent<Light>().enabled = true;
                break;

            case 1://鍵
                msgIndex = 12;
                //ホールのドアが開く
                GeneralManager.instance.SetCondition("HoleDoorLock");

                SoundManager.instance.PlaySound(0, 11);
                break;
            case 2://Oil
                msgIndex = 22;
                GeneralManager.instance.SetCondition("OilUsed");
                break;
            case 3://ハンマー
                msgIndex = 13;
                GeneralManager.instance.SetCondition("HummerUsed");

                break;
            case 5://バネ
                msgIndex = 14;
                GeneralManager.instance.SetCondition("SpringUsed");
                break;
            case 6://オフィスのロック解除
                msgIndex = 17;
                GeneralManager.instance.SetCondition("OfficeRoomDoorLock");
                break;
            case 7://薬品
                msgIndex = 15;
                GeneralManager.instance.SetCondition("MedicineUsed");
                break;
            case 8://オフィスロッカーのカギ
                msgIndex = 12;
                GeneralManager.instance.SetCondition("OfficeLockkerKey");
                break;
            case 9://磁石
                msgIndex = 16;
                GeneralManager.instance.SetCondition("MagnetUsed");
                break;
            case 10://金庫が開いた
                msgIndex = 18;
                GeneralManager.instance.SetCondition("KinkoLock");
                break;
            case 11://USBメモリ
                msgIndex = 19;
                break;
            case 12://台開放チケット
            case 13:
                msgIndex = 24;
                break;
        }
        if (msgIndex != 88888)
        {
            GeneralManager.instance.ShowMsg(msgIndex);
            DlgSetActive(false);
        }


        UpdateItemPicture();
    }

    private void UpdateItemPicture()
    {
        for (int i = 0; i < ItemButtons.Length; i++)
        {
            ShowImage(i);

        }
    }


}
