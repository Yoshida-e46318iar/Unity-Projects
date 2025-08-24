using UnityEngine;
using DG.Tweening;
using System;
public class StartTextScrol : MonoBehaviour
{
    [SerializeField] GameObject[] scrollText;
    [SerializeField] float[] destPositions;
    [SerializeField] float[] durations;

    Vector3[] defposs=new Vector3[2];

    Tween statrTween;
    bool isDoing = false;
    void Start()
    {
        for (int i = 0; i < scrollText.Length; i++)
        {
            defposs[i] = scrollText[i].transform.localPosition;
        }
    }

    public void ShowStartText(int index)
    {
        if (isDoing)
        {
            HideStartText();
        }

        isDoing = true;
        scrollText[index].gameObject.SetActive(true);
        statrTween= scrollText[index].transform.DOLocalMoveX(destPositions[index], durations[index])
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {

            scrollText[index].SetActive(false);
            scrollText[index].transform.localPosition=defposs[index];
            isDoing= false;

        });

    }

    public void HideStartText()
    {
        if (statrTween != null) { 
            statrTween.Kill();
            statrTween= null;
            isDoing=false;
        }


        for (int i = 0; i < scrollText.Length; i++)
        {
            scrollText[i].SetActive(false);
            scrollText[i].transform.localPosition = defposs[i];
        }
    }

}
