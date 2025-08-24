using UnityEngine;


public class OutBoxManager : MonoBehaviour
{


    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tama"))
        {
            other.gameObject.SetActive(false);　//非アクティブ化


        }
    }
}
