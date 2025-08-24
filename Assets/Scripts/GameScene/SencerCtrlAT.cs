using UnityEngine;
using UnityEngine.Events;

public class SencerCtrlAT : MonoBehaviour
{
    [SerializeField] UnityEvent<int> sencingEvents;

    bool isSubSenced=false;

    private void OnTriggerEnter(Collider other)
    {
        if (isSubSenced&&other.CompareTag("Tama"))
        {

                sencingEvents.Invoke(0);
                isSubSenced = false;
           
        }
        
    }

    public void OnSubSencedEvent()
    {
        isSubSenced = true;
    }

}
