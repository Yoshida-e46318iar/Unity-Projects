using UnityEngine;
using DG.Tweening;
public class LockkerCtrl : MonoBehaviour
{
    [SerializeField] GameObject door;
    bool isDoorOpened=false;
   public void OnDoorClick()
    {
        if (!isDoorOpened)
        {
            door.transform.DOLocalRotate(new Vector3(0, -120, 0), 0.5f);
            isDoorOpened = true;
        }
        else
        {
            door.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
            isDoorOpened = false;

        }

        //Debug.Log("LockkerClicked");
    }

}
