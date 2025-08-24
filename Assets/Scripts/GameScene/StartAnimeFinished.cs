using UnityEngine;
using UnityEngine.Events;
public class StartAnimeFinished : MonoBehaviour
{
    [SerializeField] UnityEvent startAnimeStartedEvent;
    [SerializeField] UnityEvent startAnimeFinishedEvent;


  public void OnAnimeFinished()
    {
       // this.gameObject.SetActive(false);
        startAnimeFinishedEvent.Invoke();
    }

    public void OnAnimeStart()
    {
        this.gameObject.SetActive(true);
        startAnimeStartedEvent.Invoke();
    }
}
