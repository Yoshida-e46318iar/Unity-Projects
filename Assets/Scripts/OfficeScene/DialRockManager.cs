using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Sequence = DG.Tweening.Sequence;
public class DialRockManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] textNumbers;
    [SerializeField] string[] doorLockNumber;
    [SerializeField] GameObject msgText;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] Camera camera_Sub;
    [SerializeField] GameObject dial;
    [SerializeField] GameObject door;

    int currentKeta = 0;
    float inputInterval = 10f;
    float lastInput = 0;
    Vector3 defCameraPos_Sub = Vector3.zero;

    private void Start()
    {
        defCameraPos_Sub = camera_Sub.transform.position;
    }

    public void OnNumberButtonDown(int number)
    {
        if (Time.time - lastInput > inputInterval)
        {
            lastInput = Time.time;
            currentKeta = 0;
        }
     
        textNumbers[currentKeta].text = number.ToString();
        currentKeta++;
        if (currentKeta > textNumbers.Length-1)
            currentKeta = 0;

        SoundManager.instance.PlaySe(audioClips[0]);
    }

    public void OnEnterButtonDown()
    {
        bool res = true;
        msgText.gameObject.SetActive(true);
        for (int i = 0; i < textNumbers.Length; i++)
        {
            if (doorLockNumber[i] != textNumbers[i].text) { 
                res = false;

                msgText.GetComponent<MsgTextManager>().ShowMsgIndex(1);
                SoundManager.instance.PlaySe(audioClips[1]);

                break;
            }
        }

        if (res)
        {
            SoundManager.instance.PlaySe(audioClips[2]);
            SoundManager.instance.PlaySe(audioClips[3]);
            msgText.GetComponent<MsgTextManager>().ShowMsgIndex(2);
            //ÉçÉbÉNâèú
            GeneralManager.instance.SetCondition("KinkoLock");

        }
        else
        {
            for (int j = 0; j < textNumbers.Length; j++)
                textNumbers[j].text = "-";
            lastInput = 0;
        }



    }

    public void OnLockOpenMsgFinnished(int index)
    {
        //åÆÇ™âèúÇ≥ÇÍÇƒÇ¢ÇΩÇÁ
        if (GeneralManager.instance.GetCondition("KinkoLock")&&
            GeneralManager.itemAcquireds[11,0]==0)
        {

            Sequence seq = DOTween.Sequence();
            seq.Append(camera_Sub.transform.DOMove(new Vector3(1.01f, 0.12f, 0.524f), 0.5f));
            seq.Append(dial.transform.DOLocalRotate(new Vector3(0f, -90f, 00f), 1f));
            seq.AppendInterval(1.0f);
            seq.Append(door.transform.DOLocalRotate(new Vector3(-90f, 0f, -90f), 1.0f));
        }
    }


}
