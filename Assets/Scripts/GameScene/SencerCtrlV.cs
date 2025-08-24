using UnityEngine;
using UnityEngine.Events;

public class SencerCtrlV : MonoBehaviour
{
    [SerializeField] UnityEvent<int> sencingEvents;
    [SerializeField] UnityEvent OutCountEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tama"))
        {
            sencingEvents.Invoke(0);
            OutCountEvent.Invoke();
            other.gameObject.SetActive(false);　//非アクティブ化
        }
    }
}
