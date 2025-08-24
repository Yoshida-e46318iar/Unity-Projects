using UnityEngine;
using UnityEngine.Events;

public class SencerCtrlATSub : MonoBehaviour
{
    [SerializeField] UnityEvent<int> sencingEvents;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tama"))
        {

                sencingEvents.Invoke(0);

           
        }
    }

}
