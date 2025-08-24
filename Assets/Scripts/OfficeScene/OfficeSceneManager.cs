using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class OfficeSceneManager : MonoBehaviour
{

    [SerializeField] Button button_R;
    [SerializeField] Button button_L;
    [SerializeField] Button button_F;
    [SerializeField] Button button_B;
    [SerializeField] Camera camera_Sub;
    [SerializeField] GameObject kinkoDoorSencer;
    [SerializeField] GameObject msgText;
    [SerializeField] Canvas canvasDoorLock;
    [SerializeField] Canvas canvasMain;

    Vector3 defCameraPos = Vector3.zero;
    Vector3 defLightPos = Vector3.zero;

    private void Start()
    {
        GeneralManager.instance.FadeIn(2.5f);
        button_B.interactable = false;

        defCameraPos = Camera.main.transform.position;


        SoundManager.instance.SoundMuteCheck();
        SoundManager.instance.PlayBGMNumber(12);
        SoundManager.instance.FadeInBGM();

        canvasMain.gameObject.SetActive(true);
        camera_Sub.gameObject.SetActive(false);
        canvasDoorLock.gameObject.SetActive(false);
        kinkoDoorSencer.gameObject.SetActive(true);

        // Debug.Log("PositionZ=" + Camera.main.transform.position.z);
    }
    public void OnRightButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        Camera.main.transform.DORotate(transform.eulerAngles + new Vector3(0, 90, 0), 0.5f, RotateMode.WorldAxisAdd);

        if (Camera.main.transform.rotation.y == 0)
        {
            button_R.interactable = false;

        }

        button_L.interactable = true;
        FBButtonEnable();

    }
    public void OnLeftButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();
        Camera.main.transform.DORotate(transform.eulerAngles + new Vector3(0, -90, 0), 0.5f, RotateMode.WorldAxisAdd);

        if (Camera.main.transform.rotation.y == 0)
        {
            button_L.interactable = false;

        }

        button_R.interactable = true;

        FBButtonEnable();

    }

    public void OnGotoEntranceButtonDown()
    {
        GeneralManager.instance.HideItemDlg();
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(8), 2f, 0, 1);
        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();
    }

    public void OnFBButtonDown(int direction)
    {

        SoundManager.instance.PlayButtonSEOK();

        if (direction == 0)
        {
            Camera.main.transform.DOLocalMoveZ(0.6f, 0.5f);
            button_F.interactable = false;
            button_B.interactable = true;
        }
        else
        {
            Camera.main.transform.DOLocalMove(defCameraPos, 0.5f);
            button_F.interactable = true;
            button_B.interactable = false;
        }

    }

    void FBButtonEnable()
    {
        if (Camera.main.transform.rotation.y == 0)
        {
            button_F.interactable = false;
            button_B.interactable = false;

        }

        else
        {

            if (Camera.main.transform.position.z == defCameraPos.z)
            {
                button_F.interactable = true;
                button_B.interactable = false;
            }
            else
            {
                button_F.interactable = false;
                button_B.interactable = true;
            }

        }

    }

    public void OnKinkoDoorScnerClicked()
    {
        //USBMemory‚ðŠl“¾Œã‚Í”à‚ÍŠJ‚©‚È‚¢
        if (GeneralManager.itemAcquireds[11,0] == 0) { 

            camera_Sub.gameObject.SetActive(true);
            kinkoDoorSencer.gameObject.SetActive(false);
            canvasMain.gameObject.SetActive(false);
            canvasDoorLock.gameObject.SetActive(true);
            msgText.SetActive(true);
            msgText.GetComponent<MsgTextManager>().ShowMsgIndex(0);
        }
    }

    public void OnReturnButtonDown()
    {
        camera_Sub.gameObject.SetActive(false);
        kinkoDoorSencer.gameObject.SetActive(true);
        canvasMain.gameObject.SetActive(true);
        canvasDoorLock.gameObject.SetActive(false);
    }


}
