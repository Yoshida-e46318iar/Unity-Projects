using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoardManager : MonoBehaviour
{
    [SerializeField] Sprite itemSprite;
    [SerializeField] string itemName;
    [SerializeField] Image itemImage;
    [SerializeField] GameObject itemPanelSet;
    [SerializeField] GameObject itemObject;
    [SerializeField] Button buttonYes;
    [SerializeField] GameObject MsgText;


    int msgFinnishedEventTimes = 0;
    bool isItemAcquired = false;
    void Start()
    {
        itemPanelSet.SetActive(false);
        
        if(itemObject != null) //3Dオブジェクトを使わないときもある
            itemObject.SetActive(false);


        if (!GeneralManager.instance.CheckItem(itemName))
        {//すでに獲得済でなければ表示
            if (itemObject != null)
            {
                if(itemName== "Magnet")
                {
                    if(GeneralManager.instance.GetCondition("OfficeRoomLockkerLock"))
                    itemObject.SetActive(true);
                }
                else
                    itemObject.SetActive(true);

                //Debug.Log(itemName + "を表示");
            }

        }
        else {  
            
            if(GeneralManager.instance.GetItemRemain(itemName) <= 0) 
                isItemAcquired = true;//すでに獲得済
            else
                isItemAcquired = false;
        }

    }

    

    public void ShowItemDlg()
    {
        if (isItemAcquired)
            return;

        GameObject tempObj=GameObject.FindGameObjectWithTag("ItemDlgCtrl");
        if(tempObj != null && tempObj.GetComponent<ItemDlgCtrl>().GetDlgActive())
        {
            tempObj.GetComponent<ItemDlgCtrl>().DlgSetActive(false);

        }

        itemPanelSet.SetActive(true);
        MsgText.SetActive(true);
        itemImage.sprite = itemSprite;
        MsgText.GetComponent<MsgTextManager>().ShowMsgIndex(0);
    }

    public void DelayShowItemDlg(float delayTime)
    {
        DOVirtual.DelayedCall(
             delay: delayTime, //何秒後に実行するか
             callback: () => ShowItemDlg()//遅延処理

        );

    }


    public void OnOKButtonDown()
    {


        if(itemObject!=null)
           itemObject.SetActive(false);

        if (itemName == "Magnet") {
            GeneralManager.instance.GetItem(itemName);
            GeneralManager.instance.GetItem(itemName);
        }
        else
            GeneralManager.instance.GetItem(itemName);

        SoundManager.instance.PlayButtonSEOK();

        Destroy(itemObject );
        Destroy(this.gameObject);

    }


    public void OnMsgTypeFinnished()
    {

        if(msgFinnishedEventTimes == 0)
        {
            buttonYes.interactable = true;
        }



    }


}
