using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;


public class DenchuCtrl : MonoBehaviour
{
    [SerializeField] GameObject LeftWing;
    [SerializeField] GameObject RightWing;
    [SerializeField] OnOffData[] OnOffDatas;
    [SerializeField] int countMax;
    [SerializeField] GameObject payOutManager;


    IEnumerator CountDownRoutine;
    IEnumerator actionCoroutine;
    float CountDownTimer = 0f;

    int sencerCount = 0;

    bool isCountMax = false;
    bool isActionDoing = false;
    bool isItemUsed=false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            WingOnOff(true);


    }

    void WingOnOff(bool flag)
    {

        if (flag)
        {

            LeftWing.GetComponent<HingeJoint>().useMotor = true;

            RightWing.GetComponent<HingeJoint>().useMotor = true;

            SoundManager.instance.PlaySound(0, 5);
        }
        else
        {
            LeftWing.GetComponent<HingeJoint>().useMotor = false;

            RightWing.GetComponent<HingeJoint>().useMotor = false;
        }

    }

    public void ActionStart()
    {
       //アイテムが使用されていたら、開きっぱなしにするために処理をするー
        if (isItemUsed)
            return;

        if (!isActionDoing)
        {
            actionCoroutine = DoOnOffSequence(OnOffDatas);
            StartCoroutine(actionCoroutine);
        }
    }

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
                isActionDoing = false;
                StopCoroutine(actionCoroutine);
            }


        }
        isActionDoing = false;

    }




    //シャッター開放中の別のコルーチン

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




    //玉が一定数入ったことにより開放を途中終了する場合の処理

    //別のスクリプトから呼び出される

    public void ActionStop()
    {

        //カウントダウン用のタイマーを0にして、コルーチンを外部から終了させる
        CountDownTimer = 0f;
        

    }

    public void SencerOnEvent(int value)
    {

 
        payOutManager.GetComponent<PayoutManager>().Payout(value);

        if (value == 2)
        {
            sencerCount++;

            if (sencerCount == countMax)
            {
                isCountMax = true;
                ActionStop();
                sencerCount = 0;
            }
        }
        else if (value == 8)
        {
            DenchyuChusen();
        }
    }


    void DenchyuChusen()
    {
        if (!isActionDoing) { 
            System.Random random = new System.Random();
            int maxValue = GeneralManager.instance.GetDenchyuBunbo()*10;
            int res = random.Next(0, maxValue);


            if (res < 10){
                ActionStart();
            }
        }

    }

    public void ItemEffectStart()
    {
        isItemUsed = true;
        WingOnOff(true);

    }

}
