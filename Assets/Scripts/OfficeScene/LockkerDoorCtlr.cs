using DG.Tweening;
using UnityEngine;

public class LockkerDoorCtlr : MonoBehaviour
{
    [SerializeField] GameObject door;
    bool isDoorOpened = false;
    [SerializeField] GameObject typeText;
    [SerializeField] GameObject itemMagnet;
    [SerializeField] AudioClip[] audioClips;
    public void OnDoorClick()
    {
        if (GeneralManager.instance.GetCondition("OfficeLockkerKey"))
        {
            if (!isDoorOpened)
            {
                door.transform.DOLocalRotate(new Vector3(0, -120, 0), 0.5f);
                isDoorOpened = true;
                //typeText.gameObject.SetActive(true);
                //typeText.GetComponent<MsgTextManager>().ShowMsgIndex(1);
                itemMagnet.gameObject.SetActive(true);
                SoundManager.instance.PlaySe(audioClips[1]);
            }
            else
            {
                door.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
                isDoorOpened = false;

            }
        }
        else
        {
            typeText.gameObject.SetActive(true);
            typeText.GetComponent<MsgTextManager>().ShowMsgIndex(0);
            SoundManager.instance.PlaySe(audioClips[0]);

        }


    }
}
