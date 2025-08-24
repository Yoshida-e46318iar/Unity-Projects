using UnityEngine;
using UnityEngine.Events;

public class SencerCtrl : MonoBehaviour
{
    [SerializeField] UnityEvent<int> sencingEvents;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tama"))
        {
            sencingEvents.Invoke(0);
            other.gameObject.SetActive(false);　//非アクティブ化
        }
    }
}
