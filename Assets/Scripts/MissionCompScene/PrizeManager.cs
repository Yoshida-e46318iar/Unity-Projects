using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeManager : MonoBehaviour
{
    [SerializeField] GameObject prizeDlg;
    [SerializeField] GameObject adManager;
    [SerializeField] Button okButton;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text lifeTitleText;
    [SerializeField] TMP_Text lifeCountText;
    [SerializeField] int itemGetMissionNum;
    [SerializeField] float itemGetAnimDuration;

    int addRemains = 0;
    int currentGold = 0;
    int currentLife = 0;
    int addLifeRemains = 0;
    int[] titlepos = { 112, 12 };
    void Start()
    {
        SoundManager.instance.FadeInBGM();
        okButton.interactable = false;
    }

    public void AddPrize(int value,int lifevalue)
    {
        prizeDlg.SetActive(true);
        if (value > 0) {
            titleText.text = value.ToString("D5") + "G Šl“¾";

            addRemains = value;
            currentGold = GeneralManager.amountGold;
            //‰ÁŽZ‘O‚ð•\Ž¦
            goldText.text = currentGold.ToString("D7") + "G";
        }
        else
        {
            titleText.gameObject.SetActive(false);
            goldText.gameObject.SetActive(false);
        }

        if (lifevalue > 0) {

            if (value <= 0)
            {
                lifeTitleText.transform.localPosition=new Vector3(0,titlepos[0],0);
                lifeCountText.transform.localPosition= new Vector3(0, titlepos[1], 0);

            }

            lifeTitleText.text = "LIFE " + lifevalue.ToString() + "ŒÂŠl“¾";

            addLifeRemains = lifevalue;
            currentLife = GeneralManager.instance.GetLifeCount();
            lifeCountText.text = "LIFE ~" + currentLife.ToString();
        }
        else
        {
            lifeTitleText.gameObject.SetActive(false);
            lifeCountText.gameObject.SetActive(false);
        }

        //­‚µ‘Ò‚Á‚Ä‚©‚ç‰ÁŽZ‚ðŠJŽn
        DOVirtual.DelayedCall(
             delay:1.0f, //‰½•bŒã‚ÉŽÀs‚·‚é‚©
             callback: () => StartCoroutine(AddAnime())//’x‰„ˆ—
          
        );

        GeneralManager.amountGold += value;
    }

    IEnumerator AddAnime()
    {
        yield return new WaitForSeconds(1.0f);

        while (addRemains>0)
        {
                      
            yield return new WaitForSeconds(0.05f);
            addRemains -=50;
            currentGold+=50;
            goldText.text = currentGold.ToString("D7") + "G";
            SoundManager.instance.PlaySound(0, 3);

        }

        if(addRemains>0) 
            yield return new WaitForSeconds(1.5f);

        while (addLifeRemains > 0)
        {

            yield return new WaitForSeconds(0.05f);
            addLifeRemains -= 1;
            currentLife += 1;
            lifeCountText.text = "LIFE ~"+currentLife.ToString();
            SoundManager.instance.PlaySound(0, 3);

        }



        okButton.interactable = true;

        //ƒ‰ƒCƒt‰ÁŽZ
        GeneralManager.instance.SetLifeCount(currentLife);

        GeneralManager.instance.UpdateAmountGold();
        GeneralManager.instance.SaveGameData();
    }

    public void OnOkButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        prizeDlg.SetActive(false);

        int index = GeneralManager.instance.GetLatestClearedMissionData().itemNumber;
        ////Debug.Log("ItemNumber="+index);

        int msgNumber = 21;
        if (index!=88888)
        {
  
            switch (index)
            {
                case 2:
                    msgNumber = 20;
                    break;
                case 4:
                    msgNumber = 3;

                    break;
                case 6:
                    msgNumber = 5;

                    break;
                case 8:
                    msgNumber = 7;

                    break;
                case 10:
                    msgNumber = 9;

                    break;
                case 12:
                    msgNumber = 23;

                    break;

            }       



            GeneralManager.instance.GetItem(index,msgNumber);

 
            if (GeneralManager.instance.GetCurrnetSceneName() == "MissionCompleteScene")
            {
                DOVirtual.DelayedCall(
                 delay: itemGetAnimDuration, //‰½•bŒã‚ÉŽÀs‚·‚é‚©
                 callback: () => GotoNextMission()//’x‰„ˆ—

              );
            }
        }
        else {

            GotoNextMission();
        }




    }

    void GotoNextMission()
    {
        if (GeneralManager.instance.GetCurrnetSceneName() == "MissionCompleteScene")
        {
            if(GeneralManager.clearedMissionNum>1)
                adManager.GetComponent<AdMobManager>().ShowInterstitialAdWithTimeout();
        }

        GeneralManager.instance.UpdateAmountGold();
        GeneralManager.instance.SaveGameData();

        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(1), 2f, 0, 1);

    }


    public int[,] GetItemAcquireds()
    {
        return GeneralManager.itemAcquireds;
    }



}
