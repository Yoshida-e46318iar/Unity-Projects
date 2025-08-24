using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PayoutManager : MonoBehaviour
{
    [SerializeField] GameObject displayManager;
    [SerializeField] Button buttonTamakashi;
    [SerializeField] Button buttonHashya;
    [SerializeField] UnityEvent missionCompEvent;

    private void Start()
    {
        if (GeneralManager.amountGold > 0)
            buttonTamakashi.interactable = true;
        else
            buttonTamakashi.interactable = false;

        if (GeneralManager.mochidama>0)
        {
            buttonHashya.interactable = true;
        }
        else
        {
            buttonHashya.interactable = false;
        }

    }
    public void Payout(int index)
    {
        int amount = 0;

        if (index >= 0)
        {
            amount= GeneralManager.instance.GetPayoutdata(index);
            GeneralManager.mochidama += amount;
            displayManager.GetComponent<DataDisplayManager>().AddValue(amount);

            if(index!=8)
                SoundManager.instance.PlaySound(0, 7);
        }
        else {
            GeneralManager.mochidama--;
            displayManager.GetComponent<DataDisplayManager>().AddValue(-1);
        }


        //�~�b�V�����B���`�F�b�N///////////////////
        if (GeneralManager.currentMissionNum==4|| GeneralManager.currentMissionNum == 9)
        {
            if (GeneralManager.mochidama >= GeneralManager.instance.GetCurrentMissionData().value)
            {
                missionCompEvent.Invoke();
            }
            else if(GeneralManager.instance.GetCurrentMissionData().value- GeneralManager.mochidama <100){

                displayManager.GetComponent<DataDisplayManager>().MissionAlmostComp(true);
            }
            else
            {
                displayManager.GetComponent<DataDisplayManager>().MissionAlmostComp(false);

            }
        }

        //�K��GOLD
        if (GeneralManager.currentMissionNum == 8){

            int currentGold=GeneralManager.amountGold + GeneralManager.mochidama * 4;
            if (currentGold >= GeneralManager.instance.GetCurrentMissionData().value)
            {
                missionCompEvent.Invoke();
            }
            else if (GeneralManager.instance.GetCurrentMissionData().value - currentGold < 1000)
            {
                displayManager.GetComponent<DataDisplayManager>().MissionAlmostComp(true);
            }
            else
            {
                displayManager.GetComponent<DataDisplayManager>().MissionAlmostComp(false);

            }

        }



    }

   

    //�ʑ݂�����/////////////////////////////////////////////////
    public void TamaKashi()
    {
        //////Debug.Log("Mochidama="+GeneralManager.mochidama);

        SoundManager.instance.PlayButtonSEOK();
        //500G�ȏ�̏ꍇ
        if (GeneralManager.amountGold>= 500)
        {
            GeneralManager.mochidama+= 500 / 4;
            GeneralManager.amountGold -= 500;
            buttonHashya.interactable = true;

            if (GeneralManager.amountGold<=0)
            {
                buttonTamakashi.interactable = false;
            }
   
        }
        else if(GeneralManager.amountGold >0)
        {
            //�[���̏ꍇ
            GeneralManager.mochidama += (int)GeneralManager.amountGold / 4;
            GeneralManager.amountGold = 0;
            buttonTamakashi.interactable = false;
            buttonHashya.interactable = true;
  
        }
        else
        {
            buttonTamakashi.interactable = false;
      
        }

        //�\���̍X�V
        GeneralManager.instance.UpdateAmountGold();
        displayManager.GetComponent<DataDisplayManager>().UpdateDisplayText(0, GeneralManager.mochidama, false);

  
    }

}
