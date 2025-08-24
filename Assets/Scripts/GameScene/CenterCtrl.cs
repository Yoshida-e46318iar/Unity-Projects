using System;
using System.Collections;
using DG.Tweening;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CenterCtrl : MonoBehaviour
{
    [SerializeField] GameObject LeftWing;
    [SerializeField] GameObject RightWing;
    [SerializeField] GameObject[] shutters;
    [SerializeField] GameObject dots;
    [SerializeField] GameObject[] center_Gimmiks;
    [SerializeField] GameObject tower_Gimmik;
    [SerializeField] OnOffData[] OnOffDatas;
    [SerializeField] OnOffData[] OnOffDatas_itemUsed;
    [SerializeField] OnOffData[] OnOffDatas2;
    [SerializeField] OnOffData[] OnOffDatas2_itemUsed;
    [SerializeField] OnOffData[] OnOffDatasAtari;
    [SerializeField] OnOffData[] OnOffDatasAtari_ItemUsed;
    [SerializeField] int countMax;
    [SerializeField] GameObject payOutManager;
    [SerializeField] GameObject DisplayCtrl;
    [SerializeField] float atariEndingDuration;
    [SerializeField] float[] ShortActionEndingDuration;
    [SerializeField] float roundInterval;

    [SerializeField] Transform[] StartkTransforms;
    [SerializeField] Transform[] HanekTransforms;

    [SerializeField] GameObject tamaPrefab;
    [SerializeField] GameObject tamaSenser;

    [SerializeField] GameObject lightManager;
    [SerializeField] GameObject startScrollTextManager;

    IEnumerator CountDownRoutine;
    IEnumerator actionCoroutine;
    float CountDownTimer = 0f;

    public UnityEvent<int> StartAtariEvent;
    [SerializeField] UnityEvent ShortActionStartEvent;
    [SerializeField] UnityEvent AtariFinishedEvent;
    [SerializeField] UnityEvent JitanFinishedEvent;
    [SerializeField] UnityEvent MissionCompEvent;
    public UnityEvent<int> PayOutEvent;


    int sencerCount = 0; //ATの入賞数
    int outCount = 0; //排出球のカウント
    bool isCountMax = false;
    bool isActionDoing = false;
    bool isOpen = false;
    bool isVPass = false;
    bool isJitan = false;
    bool isShutterON = false;


    SpecDataObj currentSpecData;
    int currentPicNum = -1;
    int currentRound = 0;
    int finalRound = 0;
    int jitanRemains = 0;
    int roundReamains = 0;

    public bool isItemSpringUsed=false;
    public bool isItemMedicineUsed=false;
    public bool isItemUSBUsed=false;

    private Tween vwaitTween;
    private Tween mainTween;

    //デバッグ用


    enum GameStatus
    {
        IDLE,
        STARTACTION,
        VWAIT,
        VPASSED,
        OPENNING,
        ROUND,
        ROUNDINTERVAL,
        ENDING,
        RUSH,


    }
    [SerializeField] // Inspector で見えるようにしたい場合は付ける（任意）
    private GameStatus _gameStatus; // 本体フィールド

    private GameStatus gameStatus
    {
        get => _gameStatus;
        set
        {
            if (_gameStatus != value)
            {
                //Debug.Log($"[GameStatus変更] {_gameStatus} → {value}\n{UnityEngine.StackTraceUtility.ExtractStackTrace()}");
                _gameStatus = value;
            }
        }
    }

    void Start()
    {
        gameStatus = GameStatus.IDLE;
        //選択中の台のスペックデータの取得
        currentSpecData=GeneralManager.instance.GetCurrentSpecData();
        AdjustKPos();

        SelectGimmik();

        isItemSpringUsed = false;

    }


    void AdjustKPos()
    {
    

        for (int i = 0; i < StartkTransforms.Length; i++)
        {
            StartkTransforms[i].localPosition += new Vector3(currentSpecData.startKposOffsets[i], 0, 0);
        
        }

        for (int i = 0; i < HanekTransforms.Length; i++)
        {
            HanekTransforms[i].localPosition += new Vector3(currentSpecData.haneKposOffsets[i], 0, 0);
    
        }

    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

            ActionStart(2);
        }


        if (Input.GetKeyDown(KeyCode.H))
        {
            DisplayCtrl.GetComponent<DisplayCtrl>().PlayRushTextAnime();

        }



        if (Input.GetKeyDown(KeyCode.V))
        {
            OnVScenserOn();

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //Debug.Log("GameStatus = " + gameStatus+","+(int)gameStatus);

        }



    }
#endif

    //アタッカー関連制御////////////////////////////////////////////////////////////////////////////////////
    //動作開始　スタート入賞で/////////////////////////////////////////////////////////
    public void ActionStart(int index )
    {

        //Debug.Log("ActionStart gameStatus = " + gameStatus);

        if (gameStatus==GameStatus.IDLE||gameStatus==GameStatus.RUSH||gameStatus==GameStatus.VWAIT)
        {
            sencerCount = 0;
            if (!isActionDoing)
            {
                gameStatus = GameStatus.STARTACTION;
                SoundManager.instance.PlaySound(0, 6);
                ShortActionStartEvent.Invoke();
                DisplayCtrl.GetComponent<DisplayCtrl>().HideIdleText();
      

                switch (index)
                {
                    case 0:
                    case 1:
                        if (isItemSpringUsed)//Springが使われてる
                            actionCoroutine = DoOnOffSequence(OnOffDatas_itemUsed);
                        else
                            actionCoroutine = DoOnOffSequence(OnOffDatas);

                        startScrollTextManager.GetComponent<StartTextScrol>().ShowStartText(0);
                        lightManager.GetComponent<CenterLightManager>().LightFlash(1);

                        break;
                    case 2:
                        if (isItemSpringUsed)//Springが使われてる
                            actionCoroutine = DoOnOffSequence(OnOffDatas2_itemUsed);
                        else
                            actionCoroutine = DoOnOffSequence(OnOffDatas2);

                        startScrollTextManager.GetComponent<StartTextScrol>().ShowStartText(1);
                        lightManager.GetComponent<CenterLightManager>().LightFlash(2);
                        break;


                }

                StartCoroutine(actionCoroutine);

            }


        }
    }


    //羽根の開閉制御////////////////////////////////////////////////////////////////////////////////
    IEnumerator DoOnOffSequence(OnOffData[] onOffDatas)
    {
        //セットするデータの構造は、開放前時間、開放時間、次の開放までの時間

        //となっていて、このセットが配列になっている　
        isCountMax = false;
        isActionDoing = true;

        for (int i = 0; i < onOffDatas.Length; i++)
        {

            //開放前時間　一定時間待つ
            yield return new WaitForSeconds(onOffDatas[i].preopen);


            WingOnOff(true);

            //シャッター開放　専用の別のコルーチンを呼び出す

            CountDownRoutine = TimerCountDown(onOffDatas[i].open);

            //下記のようにすると上記の処理の終了を待ってくれる

            yield return StartCoroutine(CountDownRoutine);


            //開放時間が過ぎるか途中で終了となったら閉じる

            WingOnOff(false);
            yield return new WaitForSeconds(onOffDatas[i].close);

            if (isCountMax)
            {
                break;
            }


        }
        isActionDoing = false;

        ActionFinished();

    }
    //動作終了後処理///////////////////////////////////////////////////////
    void ActionFinished()
    {

        switch(gameStatus)
        {
            case GameStatus.STARTACTION://小当り経由

                ShortActionEnding();


                break;
            case GameStatus.ROUND://ラウンド中
                if (roundReamains <= 0)//最終ラウンドが終了
                {
                    gameStatus = GameStatus.ENDING;
                    if (isShutterON)
                    {
                        DoGimick(currentSpecData.machineType, false);

                    }
                    //エンディングへ
                    AtariEnding();

                }
                else//最終ラウンド以外
                {
                    if (isShutterON)
                    {
                        DoGimick(currentSpecData.machineType, false);
                    }

                    if (CheckInOut()) {
                        gameStatus = GameStatus.ROUNDINTERVAL;
                        VPassCheck();
                    }
                    else { 
                        DOVirtual.DelayedCall(3.0f, () =>
                        {
                            gameStatus = GameStatus.ROUNDINTERVAL;
                            VPassCheck();
                        });
                    }
                }


                break;


        }

    }


    //Hingeのモーターオンオフ///////////////////////////////////////////////////////////////////////////
    void WingOnOff(bool flag)
    {

        if (flag)
        {

            LeftWing.GetComponent<HingeJoint>().useMotor = true;

            RightWing.GetComponent<HingeJoint>().useMotor = true;

            if(gameStatus!=GameStatus.ROUND)
                SoundManager.instance.PlaySound(0, 4);
        }
        else
        {
            LeftWing.GetComponent<HingeJoint>().useMotor = false;

            RightWing.GetComponent<HingeJoint>().useMotor = false;
        }

    }

    //シャッター開放中の別のコルーチン////////////////////////////////////////////////////
    IEnumerator TimerCountDown(float duration)
    {
        //実行時間のセット
        CountDownTimer = duration;

        //ループ開始
        while (true)
        {

            //時間を減算
            CountDownTimer -= 0.1f;

            //一定時間待つ

            yield return new WaitForSecondsRealtime(0.1f);

            //タイマーが0になったら終了

            if (CountDownTimer <= 0f)
            {
                yield break;

            }

        }

    }
    
    //玉が一定数入ったことにより開放を途中終了する場合の処理//////////////////////////////////////////////
    //別のスクリプトから呼び出される
    public void ActionStop()
    {

        //カウントダウン用のタイマーを0にして、コルーチンを外部から終了させる
        CountDownTimer = 0f;
        DoGimick(currentSpecData.machineType, false);

    }

    //カウントスイッチ通過イベント////////////////////////////////////////////////////////////////
    public void SencerOnEvent(int index)
    {
        if (index == 7)
        {
            //カウント音
            SoundManager.instance.PlaySound(0, 3);

            if (GeneralManager.currentMissionNum==1)
            {
                DOVirtual.DelayedCall(1f, () => {
                    MissionCompEvent.Invoke();
                });
            }

            //アイテム薬品使用時//////////////////////////////////////
            if (isItemMedicineUsed)
            {
         
                Bunretsu();

            }



            if (gameStatus == GameStatus.ROUND) { 
                sencerCount++;
     

                dots.GetComponent<CountDotCtrl>().UpdateOnOff(sencerCount-1, 1);

                if (sencerCount>=8)
                {
                    //シャッターを下げる
                    DoGimick(currentSpecData.machineType, false);
                }


                if (sencerCount == countMax)
                {
                    isCountMax = true;
                    ActionStop();
                }
            }
        }
        else
        {
            payOutManager.GetComponent<PayoutManager>().Payout(index);
            //出玉計数用
            PayOutEvent.Invoke(GeneralManager.instance.GetPayoutdata(index));


        }


      }

    public void OnOutSenceEvent()
    {
        int payAmountIndex = 7;

        if (isItemUSBUsed)
            payAmountIndex = 9;             //９番めの×１００倍の払出データを使用

        outCount++;
        payOutManager.GetComponent<PayoutManager>().Payout(payAmountIndex);
        //出玉計数用
        PayOutEvent.Invoke(GeneralManager.instance.GetPayoutdata(payAmountIndex));

    }

    void Bunretsu()
    {
            int n = UnityEngine.Random.Range(1, 3);

            for (int i = 0; i < n; i++) {

                Instantiate(tamaPrefab, tamaSenser.transform.position, Quaternion.identity);
            }


    }


    //V通過処理////////////////////////////////////////////////////////////////////
    public void OnVScenserOn()
    {
        //Debug.Log("VPass gameStatus = " + gameStatus); 

        if (gameStatus==GameStatus.STARTACTION||gameStatus==GameStatus.VWAIT)
        {
            if (GeneralManager.currentMissionNum == 2)
            {
   
                DOVirtual.DelayedCall(1f, () => {
                    MissionCompEvent.Invoke();
                });
            }



            if (!isVPass)
                {
                    VPassAction();

                }

   

        }

        else if (gameStatus == GameStatus.ROUND)
        {
            if (currentRound != finalRound)
            {

                if (!isVPass)
                {
                    VPassAction();

                }

            }

        }



    }

    void VPassAction()
    {
        //Debug.Log("VPassAction called "+gameStatus);
        isVPass = true;

        //V通過アニメーション
        startScrollTextManager.GetComponent<StartTextScrol>().HideStartText();
        DisplayCtrl.GetComponent<DisplayCtrl>().PlayAnime(0);
        SoundManager.instance.PlaySound(0, 2);

        if (gameStatus != GameStatus.ROUND)
        {
            gameStatus = GameStatus.VPASSED;
            ActionStop();
            SoundManager.instance.StopBGM();
        }

    }
    

    static int GetIndexBySum(int[] values, int drawnValue)
    {
        int sum = 0;
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
            if (sum >= drawnValue)
            {
                return i;
            }
        }
        return -1; // 配列内で抽選値に達しない場合


    }
    //大当りオープニング  Vアニメーション終了イベントから呼び出し/////////////////////////////////////////////////////
    public void SetupAtariAction()
    {
        //Debug.Log("SetupAtariAction called");

        if (gameStatus==GameStatus.VPASSED)
        {
            gameStatus = GameStatus.OPENNING;
            //大当り動作用のデータ設定
            PicChyuSen();
            roundReamains = currentSpecData.rounddatas[currentPicNum];
            finalRound = currentSpecData.rounddatas[currentPicNum];
            currentRound = 1;



            if (isJitan)
                jitanRemains = currentSpecData.jitandataH[currentPicNum];
            else
                jitanRemains = currentSpecData.jitandataL[currentPicNum];

            //Debug.Log("ラウンド抽選アニメ実行");
            DoRoundChyusenAnime();
        }


    }
    //図柄抽選//////////////////////////////////////////////////////////////
    void PicChyuSen()
    {
        int maxValue = 0;

        for (int i = 0; i < currentSpecData.rounddatas.Length; i++)
        {
            maxValue += currentSpecData.picWeight[i];
        }


        int r = UnityEngine.Random.Range(0, maxValue);

        currentPicNum = GetIndexBySum(currentSpecData.picWeight, r);

        //アイテムが使用されていたら必ず15R
        if (isItemUSBUsed)
            currentPicNum = 0;

    }
    void DoRoundChyusenAnime()
    {
        DisplayCtrl.GetComponent<DisplayCtrl>().DoRoundChyusenAnime(finalRound);

    }

    public void OnRoundChyusenAnimeFininished()
    {
        //Debug.Log("RoundChyusenAnimeFinished");
        DisplayCtrl.GetComponent<DisplayCtrl>().PlayAnime(1);
        DOVirtual.DelayedCall(2.5f, () => {

            //BGM再生
            SoundManager.instance.PlayBGM(1);
            RoundStart();

            int res = 0;
            if (isJitan)
                res = 1;

            StartAtariEvent.Invoke(res);
        });


    }

    //ラウンド開始   ラウンドのアニメーション終了後にタイムラインからこの処理が呼び出される/////////////////////////////////////////////////////
    public void RoundStart()
    {
        isVPass = false;
        GeneralManager.gameStatus = 2;//大当り


        //内部構造を動作させる
        if (currentRound != finalRound) {
            DoGimick(currentSpecData.machineType, true);
            GimmikActionChange(false);
        }
        //カウントをクリア
        sencerCount = 0;
        outCount = 0;
        dots.GetComponent<CountDotCtrl>().AllClear();

        //ラウンド表示

        DisplayCtrl.GetComponent<DisplayCtrl>().ShowRound(currentRound, finalRound);

        //羽根の開閉を始める
        gameStatus = GameStatus.ROUND;
        if(isItemSpringUsed)//Spring使用
            actionCoroutine = DoOnOffSequence(OnOffDatasAtari_ItemUsed);
        else
            actionCoroutine = DoOnOffSequence(OnOffDatasAtari);
        StartCoroutine(actionCoroutine);
    }

    //内部構造の制御///////////////////////////////////////////////////////
    public void DoGimick(int index,bool mode)
    {
        if (index < 2)
        {
            if (mode)
            {
                shutters[index].transform.DOLocalMoveY(-0.0125f, 1f);

            }
            else
            {
                shutters[index].transform.DOLocalMoveY(-0.018f, 1f);
            }
            isShutterON = mode;
        }
    }


    void GimmikActionChange(bool mode)
    {
        if (currentSpecData.machineType == 1) { 
             tower_Gimmik.GetComponent<HingeJoint>().useMotor = mode;


        }

    }
    void SelectGimmik()
    {
        for (int i = 0; i < center_Gimmiks.Length; i++)
        {
            center_Gimmiks[i].gameObject.SetActive(false);
        }


        int currentMode = currentSpecData.machineType;

        //デバッグ用
        //currentMode = 2;

        center_Gimmiks[currentMode].gameObject.SetActive(true);


    }

    //VCheck//////////////////////////////////////////////////////////////
    void VPassCheck()
    {
        if (isVPass)
        {
            //次のラウンドへ
            StartNextRound();

        }
        else
        {
            //エンディングへ
            AtariEnding();


        }

    }


    //ラウンド終了////////////////////////////////////////////////////////////////
    void StartNextRound()
    {
        GimmikActionChange(true);

        DOVirtual.DelayedCall(roundInterval, () => {
            if (roundReamains > 0)
            {
                roundReamains--;
                currentRound++;
                isVPass = false;

                RoundStart();
            }


        });

    }


    //大当り終了//////////////////////////////////////////////////////////
    void AtariEnding()
    {
        DisplayCtrl.GetComponent<DisplayCtrl>().HideRound(true);
        //カウント表示をクリア
        dots.GetComponent<CountDotCtrl>().AllClear();
        //サウンドをストップ
        SoundManager.instance.FadeOutBGM();

        AtariFinishedEvent.Invoke();

        GimmikActionChange(true);

        isVPass = false;

        if (jitanRemains > 0)
        {

            DOVirtual.DelayedCall(atariEndingDuration-2f, () => {
                DisplayCtrl.GetComponent<DisplayCtrl>().PlayRushTextAnime();
                SoundManager.instance.PlayBGM(2);
                SoundManager.instance.FadeInBGM();
            });
        }

        DOVirtual.DelayedCall(atariEndingDuration, () => {


             if (jitanRemains > 0)//時短回数セットされていたら
            {
                StartRush();
            }
            else {  //時短でなければ終了
                isJitan = false;
                gameStatus = GameStatus.IDLE;
                DisplayCtrl.GetComponent<DisplayCtrl>().ShowIdleText();
                GeneralManager.gameStatus = 0;//通常
                SoundManager.instance.PlayBGM(0);
                SoundManager.instance.FadeInBGM();
            }

        });

    }

    //小当り終了///////////////////////////////////////////////////////////
    void ShortActionEnding()
    {

        if(gameStatus!=GameStatus.VPASSED)
            gameStatus = GameStatus.VWAIT;

        if(isJitan)
            JatanCountDown(); 

        float delay = ShortActionEndingDuration[(int)currentSpecData.machineType];
        if (mainTween != null && mainTween.IsActive())
        {
            mainTween.Kill();
            mainTween = null; // 古いTweenを参照しないように
        }

        //Debug.Log("isVPass =" + isVPass);

        mainTween = DOVirtual.DelayedCall(delay, () =>
        {
            if (!isVPass)
            {
                //Debug.Log("isVPass = False");
                //V通過がなければ、一定時間後にアイドル状態に移行
                //V通過時にVアニメーションを開始、終了後はSetupAtariAction()が呼び出される

                if (isJitan)
                {
                    gameStatus = GameStatus.RUSH;
                }
                else {
                    //Debug.Log("IDleに変更");
                    gameStatus = GameStatus.IDLE;
                    DisplayCtrl.GetComponent<DisplayCtrl>().ShowIdleText();
                }

            }


        });

        // 状態監視コルーチン開始
        StartCoroutine(MonitorStateCoroutine());

        lightManager.GetComponent<CenterLightManager>().LightFlash(0);
    }

    IEnumerator MonitorStateCoroutine()
    {
        while (mainTween != null && mainTween.IsActive() && mainTween.IsPlaying())
        {
            if (isVPass||gameStatus==GameStatus.STARTACTION) // 状態変化条件（ここは自由に変更可）
            {
                mainTween.Kill(); // DelayedCallキャンセル
                //Debug.Log("状態変化を検知してキャンセル、別処理実行");


                yield break; // コルーチン終了
            }

            yield return null; // 次のフレームまで待機
        }
    }


    public void OnRushStartAnimeFinished()
    {
        DisplayCtrl.GetComponent<DisplayCtrl>().ShowJitanRemain(jitanRemains);
    }

    //Rush開始///////////////////////////////////////////////////////////////////
    void StartRush()
    {
        gameStatus = GameStatus.RUSH;
        isVPass = false;
        isJitan = true;
        GeneralManager.gameStatus = 1;//時短


    }
    //時短カウントダウン///////////////////////////////////////////////////////
    void JatanCountDown()
    {
        jitanRemains--;
        if (jitanRemains > 0) { 

            DisplayCtrl.GetComponent<DisplayCtrl>().ShowJitanRemain(jitanRemains);
        }
        else {
            DisplayCtrl.GetComponent<DisplayCtrl>().HideJitanRemain();
            gameStatus = GameStatus.IDLE;
            DisplayCtrl.GetComponent<DisplayCtrl>().ShowIdleText();
            isJitan = false;
            JitanFinishedEvent.Invoke();

            GeneralManager.gameStatus = 0;//通常
            //BGM再生
            SoundManager.instance.PlayBGM(0);
        }

    }


    //ステータスの取得/////////////////////////////////////////////////////
    public int GetGameStatus()
    {

        return (int)gameStatus;
    }

    bool CheckInOut()
    {
        bool res= false;

        if(outCount>=sencerCount) 
            res=true;

      //  //Debug.Log("outCount=" + outCount+",Res="+res);

        return res;

    }
}
