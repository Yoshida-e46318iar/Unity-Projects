using UnityEngine;
using DG.Tweening;
public class CabinetManager : MonoBehaviour
{
    [SerializeField] GameObject[] cabinets;
    [SerializeField] GameObject typeText;
    [SerializeField] GameObject ItemBoard_Hummer;
    [SerializeField] AudioClip[] audioClips;

    float defPosZ;
    bool isOpen = false;
    void Start()
    {
        defPosZ = cabinets[0].transform.localPosition.z;
    }

    public void OnCabinetClick(int index)
    {

        if (GeneralManager.instance.GetCondition("OilUsed")) {

            if (!isOpen)
            {
                cabinets[index].transform.DOLocalMoveZ(0.36f, 0.5f);
                isOpen = true;

                typeText.gameObject.SetActive(true);
                typeText.GetComponent<MsgTextManager>().ShowMsgIndex(1);
                SoundManager.instance.PlaySe(audioClips[1]);

                //ƒAƒCƒeƒ€Šl“¾
                if (index == 2||index==1)
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        ItemBoard_Hummer.GetComponent<ItemBoardManager>().ShowItemDlg();
                    });
                }
            }
            else
            {
                cabinets[index].transform.DOLocalMoveZ(defPosZ, 0.5f);
                isOpen=false;
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
