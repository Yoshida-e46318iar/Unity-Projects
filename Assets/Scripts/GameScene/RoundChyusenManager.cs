using UnityEngine;
using UnityEngine.Events;

public class RoundChyusenManager : MonoBehaviour
{
    [SerializeField] UnityEvent animeFinishedEvent;

    public void OnRoundChyusenAnimeFinished()
    {

        animeFinishedEvent.Invoke();

        //Debug.Log("RoundAnimeFinishedEvent Invoked");

        this.gameObject.SetActive(false);
    }


}
