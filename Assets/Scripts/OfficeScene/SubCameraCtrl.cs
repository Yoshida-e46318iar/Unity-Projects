using UnityEngine;

public class SubCameraCtrl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas doorLockCanvas;
    public void OnDisableCamera()
    {
        this.gameObject.SetActive(false);
        doorLockCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);  

    }
}
