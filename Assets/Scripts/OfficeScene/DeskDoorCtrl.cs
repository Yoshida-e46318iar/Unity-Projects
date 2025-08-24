using DG.Tweening;
using UnityEngine;

public class DeskDoorCtrl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject door;
    bool isDoorOpened = false;
    [SerializeField] AudioClip[] audioClips;
    public void OnDoorClick()
    {
        if (!isDoorOpened)
        {
            door.transform.DOLocalRotate(new Vector3(0, 120, 0), 0.5f);
            isDoorOpened = true;
            SoundManager.instance.PlaySe(audioClips[0]);
        }
        else
        {
            door.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
            isDoorOpened = false;

        }

    }

}
