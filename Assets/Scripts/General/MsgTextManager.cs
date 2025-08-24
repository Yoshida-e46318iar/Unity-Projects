
using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;

public class MsgTextManager : MonoBehaviour
{
    [SerializeField] TMP_Text msgText;
    [SerializeField] string[] msgs;
    [SerializeField] UnityEvent<int> msgTypeFinnished;
    float charInterval = 0.05f;
    Coroutine coroutine;
    int showedMsgIndex = 0;

    public void ShowMsg(string msg)
    {
        if (!this.isActiveAndEnabled)
            return;


        Sequence sequence = DOTween.Sequence();


        sequence.Append(DOVirtual.DelayedCall(
               delay: 1.0f,
               callback: () =>coroutine= StartCoroutine(ShowText(msg))))
                .Append(DOVirtual.DelayedCall(
                   delay: 2.0f,
                   callback: () => MsgClear()
                   
                   )                           
                   
                );


    }

    public void ShowMsgIndex(int index)
    {
        if (!this.isActiveAndEnabled)
            return;



        ShowMsg(msgs[index]);
        showedMsgIndex=index;

    }



    IEnumerator ShowText(string msg)
    {
        msgText.text = "";  // èâä˙âª

        for (int i = 0; i <= msg.Length; i++)
        {
            msgText.text = msg.Substring(0, i);
            yield return new WaitForSeconds(charInterval);
        }
    }

    void MsgClear()
    {
        msgText.text = "";
        if(coroutine != null)
            StopCoroutine(coroutine);
        msgTypeFinnished.Invoke(showedMsgIndex);
        this.gameObject.SetActive(false);

    }

}
