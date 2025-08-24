using UnityEngine;
using UnityEngine.Events;

public class ATOutBoxCtrl : MonoBehaviour
{
    [SerializeField] UnityEvent OutCountEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tama"))
        {
            OutCountEvent.Invoke();
            other.gameObject.SetActive(false); //非アクティブ化
           
        }
    }
}
