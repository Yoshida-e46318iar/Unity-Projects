using UnityEngine;
using DG.Tweening;
public class ToolBoxCtrl : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject ItemBoard_Spring;
    [SerializeField] GameObject msgText;
    bool isDoorOpened = false;

    public void OnDoorClick()
    {
        if (!isDoorOpened)
        {
            door.transform.DOLocalRotate(new Vector3(-85,0, 0), 0.5f);
            isDoorOpened = true;

            //ƒAƒCƒeƒ€Šl“¾
            if (GeneralManager.instance.GetCondition("SpringItemShow")) { 
                DOVirtual.DelayedCall(1f, () =>
                {
                    ItemBoard_Spring.GetComponent<ItemBoardManager>().ShowItemDlg();
                });

            }
            else
            {
                msgText.GetComponent<MsgTextManager>().ShowMsgIndex(0);

            }


        }
        else
        {
            door.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
            isDoorOpened = false;

        }

        //Debug.Log("LockkerClicked");
    }
}
