using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DoorLockManager : MonoBehaviour
{
    [SerializeField] GameObject numberDisplay;
    [SerializeField] int[] doorOpenNumbers;
    [SerializeField] GameObject msgText;
    [SerializeField] Button[] buttons;


    int currentKeta = 0;
    int[] enteredNumber = new int[4];
    float inputInterval = 5f;
    float lastInput = 0;
    bool isCountDownDoing=false;

    public void OnNuberButtonDown(int number)
    {
        SoundManager.instance.PlaySound(0, 12);


        numberDisplay.GetComponent<NumberDisplayCtrl>().ShowNumber(currentKeta, number);
        enteredNumber[currentKeta] = number;

        if (currentKeta < 3)
            currentKeta++;
        else
            currentKeta = 0;

        if (!isCountDownDoing)
            StartCoroutine(NumberReset());
    }

    IEnumerator NumberReset()
    {
        isCountDownDoing = true;

        yield return new WaitForSeconds(inputInterval);
        for (int i = 0; i < doorOpenNumbers.Length; i++)
            numberDisplay.GetComponent<NumberDisplayCtrl>().ShowNumber(i, 0);
        isCountDownDoing=false;
        currentKeta = 0;
    }


    public void OnEnterButtonDown()
    {
        SoundManager.instance.PlaySound(0, 12);

        int res = 0;
        if (CheckNumber())
        {

            SoundManager.instance.PlaySound(0, 9);
            GeneralManager.instance.SetCondition("OfficeRoomDoorLock");

            res = 1;
        }
        else
            SoundManager.instance.PlaySound(0, 14);

        msgText.gameObject.SetActive(true);

        msgText.GetComponent<MsgTextManager>().ShowMsgIndex(res);

     }

    bool CheckNumber()
    {
        return doorOpenNumbers.SequenceEqual(enteredNumber);
    }

    public void ButtonEnable(bool enable)
    {
        for(int i = 0;i < buttons.Length;i++) 
            buttons[i].enabled = enable;    
    }

}
