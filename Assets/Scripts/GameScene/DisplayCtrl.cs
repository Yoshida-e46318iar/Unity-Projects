using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.Events;

public class DisplayCtrl : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;
    [SerializeField] PlayableAsset[] playableAssets;
    [SerializeField] PlayableAsset[] round_playableAsses;
    [SerializeField] GameObject[] animeTexts;
    [SerializeField] GameObject text_Idle;

    [SerializeField] GameObject RoundText_Belt;
    [SerializeField] GameObject FinalRound_Text;
    [SerializeField] GameObject Round_Chusen_Obj;
    [SerializeField] GameObject RushText;
    [SerializeField] GameObject VBG;
    [SerializeField] TMP_Text JitanCountDownText;

    [SerializeField] UnityEvent RushStartAnimeFinished;

    static float ROUNDOFFSET = 110f;

    bool isRoundShowing = false;
    bool isJitanCDDoing = false;

    bool isStartAnimeDoing = false;
    bool isShowIdleTextCmdCatch=false;

    int roundindex = 0;
    Vector3 finalRoundDespos;
    Vector3 jiatnCDTextDefPos;


    void Start()
    {
        Round_Chusen_Obj.SetActive(false);

        finalRoundDespos = FinalRound_Text.transform.localPosition;
        jiatnCDTextDefPos= JitanCountDownText.transform.localPosition;
    }


    public void PlayAnime(int index)
    {
        

        if (RushText.activeSelf)
            RushText.SetActive(false);


       playableDirector.Play(playableAssets[index]);

        //Debug.Log("DisplayCtrl PlayAnime index= " + index); 
    }

    public void OnVAnimeStart()
    {
        VBG.SetActive(true);
        if (isJitanCDDoing)
        {
            HideJitanRemain();
            isJitanCDDoing = false;
        }
    }

    public void OnVAnimeFinished()
    {
        VBG.SetActive(false);


    }

    public void ShowRound(int currentRound,int maxRound)
    {


        if (currentRound<maxRound)
        {
            UpdateRound(currentRound-1);
            isRoundShowing = true;
        }
        else
        {
            HideRound(false);
            ShowFinalRound();
        }

    }
    public void HideRound(bool all)
    {
        if (all)
        {
            FinalRound_Text.SetActive(false);
        }
        RoundText_Belt.SetActive(false);
        isRoundShowing = false;
    }


    public void UpdateRound(int r)
    {
        if (r == 0)
            RoundText_Belt.transform.localPosition = new Vector3(110f, -0.9996532f, -0.0001377633f);

        RoundText_Belt.SetActive(true);
        RoundText_Belt.transform.DOLocalMoveX(r * -ROUNDOFFSET, 1.0f);

    }

    public void ShowFinalRound()
    {

        FinalRound_Text.SetActive(true);
        FinalRound_Text.transform.localPosition = finalRoundDespos;

        FinalRound_Text.transform.DOLocalMoveX(0, 1.0f);
        //DOVirtual.DelayedCall(1.5f, () => {

        //    FinalRound_Text.SetActive(false);
        //    FinalRound_Text.transform.localPosition = defpos;
        //});

    }

    public void DoRoundChyusenAnime(int round)
    {
        int patern = 0;
        switch (round)
        {
            case 15:
                patern = UnityEngine.Random.Range(7, 13);
                    break;
            case 10:
                patern = UnityEngine.Random.Range(3, 7);
                break;
            case 2:
                patern = 0;
                break;
            case 4:
                patern = 1;
                break;
            case 8:
                patern = 2;
                break;

        }

        animeTexts[0].SetActive(true);
        playableDirector.Play(round_playableAsses[patern]);
        //Debug.Log("RoundChusenAnimPat=" + patern);

    }

    public void PlayRushTextAnime()
    {
        RushText.SetActive(true);
        RushText.transform.DOLocalMoveY(0f, 1.0f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(1.5f, () => {

                RushText.SetActive(false);
                RushText.transform.localPosition = new Vector3(0f, -95f, 0);
                RushStartAnimeFinished.Invoke();
         
            });
   
        });

    }



    public void ShowJitanRemain(int count)
    {
        isJitanCDDoing = true;

        JitanCountDownText.gameObject.SetActive(true);
        if(count>1)
            JitanCountDownText.text = "Žc‚è\n" + count.ToString() + "‰ñ";
        else
            JitanCountDownText.text = "ƒ‰ƒXƒg";

       
    }

    public void HideJitanRemain()
    {

        JitanCountDownText.gameObject.SetActive(false);

    }

    public void JitanEnd()
    {

        HideJitanRemain();
        isJitanCDDoing = false;
    }

    public void OnStartAnimeStartted()
    {
        //Debug.Log("OnStartAnimeStartted called");

        isStartAnimeDoing = true;
        if (JitanCountDownText.isActiveAndEnabled)
            HideJitanRemain();
    }

    public void OnStartAnimeFinished()
    {
        //Debug.Log("OnStartAnimeFinished called");

        isStartAnimeDoing = false;

        if (isJitanCDDoing) { 
            JitanCountDownText.gameObject.SetActive(true);

        }

        if (isShowIdleTextCmdCatch) {

            ShowIdleText();
            isShowIdleTextCmdCatch = false;
        }

    }

    public void ShowIdleText()
    {
        if(!isStartAnimeDoing)
            text_Idle.SetActive(true);
        else
        {
            isShowIdleTextCmdCatch = true;
        }
    }

    public void HideIdleText()
    {
        text_Idle.SetActive(false);
        isShowIdleTextCmdCatch = false;
    }


}
