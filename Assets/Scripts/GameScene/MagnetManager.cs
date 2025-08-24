using DG.Tweening;
using DG.Tweening.CustomPlugins;
using UnityEngine;
using UnityEngine.UI;

public class MagnetManager : MonoBehaviour
{
    [SerializeField] GameObject vZoneCtrl;
    [SerializeField] Camera cam;
    [SerializeField] Canvas Canvas;
    [SerializeField] GameObject TypeText;
    [SerializeField] CanvasGroup targetPanel;
    [SerializeField] GameObject stopButton;

    private void Start()
    {
        cam.gameObject.SetActive(false);
        Canvas.gameObject.SetActive(false) ;
    }

    public void VZoneActionStart()
    {
        cam.gameObject.SetActive(true);
        Canvas.gameObject.SetActive(true);
        TypeText.SetActive(true);

        targetPanel.DOFade(0f, 3f).SetEase(Ease.OutQuad);


        DOVirtual.DelayedCall(1.5f, () => {

            vZoneCtrl.GetComponent<VRollingCtrl>().StartRotation();
            TypeText.SetActive(true);
            TypeText.GetComponent<MsgTextManager>().ShowMsgIndex(0);
            SoundManager.instance.PlaySELoop(16);

        });

        DOVirtual.DelayedCall(2.5f, () => {

            stopButton.SetActive(true);
            TypeText.SetActive(true);
            TypeText.GetComponent<MsgTextManager>().ShowMsgIndex(1);
        });

    }



    public void OnStopButtonDown()
    {
        vZoneCtrl.GetComponent<VRollingCtrl>().StopRotationSmoothly();

        SoundManager.instance.StopSE();

        SoundManager.instance.PlaySound(0, 17);

        DOVirtual.DelayedCall(1.5f, () => {
            TypeText.SetActive(true);
            TypeText.GetComponent<MsgTextManager>().ShowMsgIndex(2);

        });


        DOVirtual.DelayedCall(5f, () => {

            Canvas.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);

        });
    }

}
