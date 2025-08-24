using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] Camera SubCamera;
    [SerializeField] Vector3[] camerapos;
    [SerializeField] GameObject MissionTitleText;

    int status = 0;
    void Start()
    {
        
    }

    public void OnViewChangeButtonDown()
    {
        SoundManager.instance.PlayButtonSEOK();

        status++;
        if (status > 5)
            status = 0;

        if (status > 0) { 
            SubCamera.gameObject.SetActive(true);
            SubCamera.transform.localPosition = camerapos[status-1];
            MissionTitleText.SetActive(false);
        }
        else { 
            SubCamera.gameObject.SetActive(false);
            MissionTitleText.SetActive(true);
        }

    }



}
