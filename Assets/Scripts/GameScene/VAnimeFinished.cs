using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class VAnimeFinished : MonoBehaviour
{
    [SerializeField] UnityEvent animeStartEvent;
    [SerializeField] UnityEvent animeFinishedEvent;


    public void OnAnimeFinished()
    {
       // this.gameObject.SetActive(false);
        //Debug.Log("VAnimeFinished");

        DOVirtual.DelayedCall(0.5f, () => {

            animeFinishedEvent.Invoke();
        });
    }

    public void OnVAnimeStart()
    {

        animeStartEvent.Invoke();

    }
}
