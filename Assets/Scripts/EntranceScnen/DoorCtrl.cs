using UnityEngine;
using DG.Tweening;

public class DoorCtrl : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject msgText;

    void Start()
    {
        
    }

    public void DoorClick(int index)
    {
        //Debug.Log("Door" + index + "Clicked");
        string doorName = "";
        switch (index) {
            case 0:
                doorName = "HoleDoorLock";
                break;
            case 1:
                doorName = "OfficeRoomDoorLock";
                break;
            case 2:
                doorName = "StockRoomDoorLock";
                break;
        }

        DoorOpen(doorName);

    }

    void DoorOpen(string doorName)
    {


        //èåèÇ™ê¨óßÇµÇƒÇ¢ÇÍÇŒ1
        if (GeneralManager.instance.GetCondition(doorName))
        {
            string roomName = "";
            switch (doorName)
            {
                case "HoleDoorLock":
                    roomName = "MachineSelectScene";
                    break;
                case "OfficeRoomDoorLock":
                    roomName = "OfficeScene";
                    break;
                case "StockRoomDoorLock":
                    roomName = "StockRoomScene";
                    break;

            }

            SoundManager.instance.PlaySound(0, 10);

            DoorOpenAndSceneChange(roomName);

            SoundManager.instance.FadeOutBGM();
            SoundManager.instance.StopBGM();

        }
        else {
            SoundManager.instance.PlaySound(0, 9);
            msgText.gameObject.SetActive(true);
            msgText.GetComponent<MsgTextManager>().ShowMsgIndex(0);

        }



    }

    void DoorOpenAndSceneChange(string roomName)
    {
        Sequence sequence = DOTween.Sequence();


        if (roomName == "OfficeScene")
        {
            sequence.Append(door.transform.DOLocalRotate(new Vector3(-90f, 0, -120), 1f))
                .Append(Camera.main.transform.DOLocalMoveX(0.5f, 0.5f))
                .OnComplete(() =>
                {
                    GeneralManager.instance.ChageScene(roomName, 0.1f, 0, 1);
                });

        }
        else
        {
            sequence.Append(door.transform.DOLocalRotate(new Vector3(-90f, 0, -120), 1f))
                    .Append(Camera.main.transform.DOLocalMoveZ(0.5f, 0.5f))
                    .OnComplete(() =>
                    {
                        GeneralManager.instance.ChageScene(roomName, 0.1f, 0, 1);
                    });

        }
    }


}
