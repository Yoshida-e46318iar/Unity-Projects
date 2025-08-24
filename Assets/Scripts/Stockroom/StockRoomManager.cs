using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class StockRoomManager : MonoBehaviour
{
    [SerializeField] Button button_R;
    [SerializeField] Button button_L;
    [SerializeField] Button button_F;
    [SerializeField] Button button_B;
    [SerializeField] Light flashLight;
    [SerializeField] Material[] materials;
    [SerializeField] GameObject infoBoard;

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

        SelectMaterial();

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
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(7), 2f, 0, 1);
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

        else {

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

        //Debug.Log("Rotation=" + Camera.main.transform.localRotation.y);
        //Debug.Log("PositionZ=" + Camera.main.transform.position.z);

    }

    public void OnInfoBoardPointerEnter()
    {

         flashLight.transform.DOLocalRotate(new Vector3(-20f, 0, 0), 0.5f);

    }

    public void OnInfoBoardPointerExit()
    {
        flashLight.transform.DOLocalRotate(defLightPos, 0.5f);
    }

    void SelectMaterial()
    {

        if (GeneralManager.currentMissionNum == 1)
            infoBoard.GetComponent < MeshRenderer>().material= materials[1];
        else if(GeneralManager.currentMissionNum >= 2)
            infoBoard.GetComponent<MeshRenderer>().material = materials[2];

    }

    public void OnClockClick()
    {
        flashLight.transform.DOLocalRotate(new Vector3(-20f, 0, 0), 0.5f);

    }

}
