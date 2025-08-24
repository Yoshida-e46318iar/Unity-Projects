using DG.Tweening;
using UnityEngine;

public class CalenderCloseUpManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    Vector3 defPos;
    Vector3 closeUpViewPos = new Vector3(0f, 1.172f, -0.772f);

    private void Start()
    {
        defPos = mainCamera.transform.localPosition;
    }
    // Update is called once per frame
    public void CloseUpCalender()
    {
        //mainCamera.transform.localPosition = closeUpViewPos;
        mainCamera.transform.DOLocalMove(closeUpViewPos, 0.3f);

    }

    public void ReturnDefView()
    {
       // mainCamera.transform.localPosition= defPos;
        mainCamera.transform.DOLocalMove(defPos, 0.3f);
    }
}
