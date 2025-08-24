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


    int sencerCount = 0; //AT�̓��ܐ�
    int outCount = 0; //�r�o���̃J�E���g
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

    //�f�o�b�O�p


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
    [SerializeField] // Inspector �Ō�����悤�ɂ������ꍇ�͕t����i�C�Ӂj
    private GameStatus _gameStatus; // �{�̃t�B�[���h

    private GameStatus gameStatus
    {
        get => _gameStatus;
        set
        {
            if (_gameStatus != value)
            {
                //Debug.Log($"[GameStatus�ύX] {_gameStatus} �� {value}\n{UnityEngine.StackTraceUtility.ExtractStackTrace()}");
                _gameStatus = value;
            }
        }
    }

    void Start()
    {
        gameStatus = GameStatus.IDLE;
        //�I�𒆂̑�̃X�y�b�N�f�[�^�̎擾
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

    //�A�^�b�J�[�֘A����////////////////////////////////////////////////////////////////////////////////////
    //����J�n�@�X�^�[�g���܂�/////////////////////////////////////////////////////////
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
                        if (isItemSpringUsed)//Spring���g���Ă�
                            actionCoroutine = DoOnOffSequence(OnOffDatas_itemUsed);
                        else
                            actionCoroutine = DoOnOffSequence(OnOffDatas);

                        startScrollTextManager.GetComponent<StartTextScrol>().ShowStartText(0);
                        lightManager.GetComponent<CenterLightManager>().LightFlash(1);

                        break;
                    case 2:
                        if (isItemSpringUsed)//Spring���g���Ă�
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


    //�H���̊J����////////////////////////////////////////////////////////////////////////////////
    IEnumerator DoOnOffSequence(OnOffData[] onOffDatas)
    {
        //�Z�b�g����f�[�^�̍\���́A�J���O���ԁA�J�����ԁA���̊J���܂ł̎���

        //�ƂȂ��Ă��āA���̃Z�b�g���z��ɂȂ��Ă���@
        isCountMax = false;
        isActionDoing = true;

        for (int i = 0; i < onOffDatas.Length; i++)
        {

            //�J���O���ԁ@��莞�ԑ҂�
            yield return new WaitForSeconds(onOffDatas[i].preopen);


            WingOnOff(true);

            //�V���b�^�[�J���@��p�̕ʂ̃R���[�`�����Ăяo��

            CountDownRoutine = TimerCountDown(onOffDatas[i].open);

            //���L�̂悤�ɂ���Ə�L�̏����̏I����҂��Ă����

            yield return StartCoroutine(CountDownRoutine);


            //�J�����Ԃ��߂��邩�r���ŏI���ƂȂ��������

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
    //����I���㏈��///////////////////////////////////////////////////////
    void ActionFinished()
    {

        switch(gameStatus)
        {
            case GameStatus.STARTACTION://������o�R

                ShortActionEnding();


                break;
            case GameStatus.ROUND://���E���h��
                if (roundReamains <= 0)//�ŏI���E���h���I��
                {
                    gameStatus = GameStatus.ENDING;
                    if (isShutterON)
                    {
                        DoGimick(currentSpecData.machineType, false);

                    }
                    //�G���f�B���O��
                    AtariEnding();

                }
                else//�ŏI���E���h�ȊO
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


    //Hinge�̃��[�^�[�I���I�t///////////////////////////////////////////////////////////////////////////
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

    //�V���b�^�[�J�����̕ʂ̃R���[�`��////////////////////////////////////////////////////
    IEnumerator TimerCountDown(float duration)
    {
        //���s���Ԃ̃Z�b�g
        CountDownTimer = duration;

        //���[�v�J�n
        while (true)
        {

            //���Ԃ����Z
            CountDownTimer -= 0.1f;

            //��莞�ԑ҂�

            yield return new WaitForSecondsRealtime(0.1f);

            //�^�C�}�[��0�ɂȂ�����I��

            if (CountDownTimer <= 0f)
            {
                yield break;

            }

        }

    }
    
    //�ʂ���萔���������Ƃɂ��J����r���I������ꍇ�̏���//////////////////////////////////////////////
    //�ʂ̃X�N���v�g����Ăяo�����
    public void ActionStop()
    {

        //�J�E���g�_�E���p�̃^�C�}�[��0�ɂ��āA�R���[�`�����O������I��������
        CountDownTimer = 0f;
        DoGimick(currentSpecData.machineType, false);

    }

    //�J�E���g�X�C�b�`�ʉ߃C�x���g////////////////////////////////////////////////////////////////
    public void SencerOnEvent(int index)
    {
        if (index == 7)
        {
            //�J�E���g��
            SoundManager.instance.PlaySound(0, 3);

            if (GeneralManager.currentMissionNum==1)
            {
                DOVirtual.DelayedCall(1f, () => {
                    MissionCompEvent.Invoke();
                });
            }

            //�A�C�e����i�g�p��//////////////////////////////////////
            if (isItemMedicineUsed)
            {
         
                Bunretsu();

            }



            if (gameStatus == GameStatus.ROUND) { 
                sencerCount++;
     

                dots.GetComponent<CountDotCtrl>().UpdateOnOff(sencerCount-1, 1);

                if (sencerCount>=8)
                {
                    //�V���b�^�[��������
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
            //�o�ʌv���p
            PayOutEvent.Invoke(GeneralManager.instance.GetPayoutdata(index));


        }


      }

    public void OnOutSenceEvent()
    {
        int payAmountIndex = 7;

        if (isItemUSBUsed)
            payAmountIndex = 9;             //�X�Ԃ߂́~�P�O�O�{�̕��o�f�[�^���g�p

        outCount++;
        payOutManager.GetComponent<PayoutManager>().Payout(payAmountIndex);
        //�o�ʌv���p
        PayOutEvent.Invoke(GeneralManager.instance.GetPayoutdata(payAmountIndex));

    }

    void Bunretsu()
    {
            int n = UnityEngine.Random.Range(1, 3);

            for (int i = 0; i < n; i++) {

                Instantiate(tamaPrefab, tamaSenser.transform.position, Quaternion.identity);
            }


    }


    //V�ʉߏ���////////////////////////////////////////////////////////////////////
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

        //V�ʉ߃A�j���[�V����
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
        return -1; // �z����Œ��I�l�ɒB���Ȃ��ꍇ


    }
    //�哖��I�[�v�j���O  V�A�j���[�V�����I���C�x���g����Ăяo��/////////////////////////////////////////////////////
    public void SetupAtariAction()
    {
        //Debug.Log("SetupAtariAction called");

        if (gameStatus==GameStatus.VPASSED)
        {
            gameStatus = GameStatus.OPENNING;
            //�哖�蓮��p�̃f�[�^�ݒ�
            PicChyuSen();
            roundReamains = currentSpecData.rounddatas[currentPicNum];
            finalRound = currentSpecData.rounddatas[currentPicNum];
            currentRound = 1;



            if (isJitan)
                jitanRemains = currentSpecData.jitandataH[currentPicNum];
            else
                jitanRemains = currentSpecData.jitandataL[currentPicNum];

            //Debug.Log("���E���h���I�A�j�����s");
            DoRoundChyusenAnime();
        }


    }
    //�}�����I//////////////////////////////////////////////////////////////
    void PicChyuSen()
    {
        int maxValue = 0;

        for (int i = 0; i < currentSpecData.rounddatas.Length; i++)
        {
            maxValue += currentSpecData.picWeight[i];
        }


        int r = UnityEngine.Random.Range(0, maxValue);

        currentPicNum = GetIndexBySum(currentSpecData.picWeight, r);

        //�A�C�e�����g�p����Ă�����K��15R
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

            //BGM�Đ�
            SoundManager.instance.PlayBGM(1);
            RoundStart();

            int res = 0;
            if (isJitan)
                res = 1;

            StartAtariEvent.Invoke(res);
        });


    }

    //���E���h�J�n   ���E���h�̃A�j���[�V�����I����Ƀ^�C�����C�����炱�̏������Ăяo�����/////////////////////////////////////////////////////
    public void RoundStart()
    {
        isVPass = false;
        GeneralManager.gameStatus = 2;//�哖��


        //�����\���𓮍삳����
        if (currentRound != finalRound) {
            DoGimick(currentSpecData.machineType, true);
            GimmikActionChange(false);
        }
        //�J�E���g���N���A
        sencerCount = 0;
        outCount = 0;
        dots.GetComponent<CountDotCtrl>().AllClear();

        //���E���h�\��

        DisplayCtrl.GetComponent<DisplayCtrl>().ShowRound(currentRound, finalRound);

        //�H���̊J���n�߂�
        gameStatus = GameStatus.ROUND;
        if(isItemSpringUsed)//Spring�g�p
            actionCoroutine = DoOnOffSequence(OnOffDatasAtari_ItemUsed);
        else
            actionCoroutine = DoOnOffSequence(OnOffDatasAtari);
        StartCoroutine(actionCoroutine);
    }

    //�����\���̐���///////////////////////////////////////////////////////
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

        //�f�o�b�O�p
        //currentMode = 2;

        center_Gimmiks[currentMode].gameObject.SetActive(true);


    }

    //VCheck//////////////////////////////////////////////////////////////
    void VPassCheck()
    {
        if (isVPass)
        {
            //���̃��E���h��
            StartNextRound();

        }
        else
        {
            //�G���f�B���O��
            AtariEnding();


        }

    }


    //���E���h�I��////////////////////////////////////////////////////////////////
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


    //�哖��I��//////////////////////////////////////////////////////////
    void AtariEnding()
    {
        DisplayCtrl.GetComponent<DisplayCtrl>().HideRound(true);
        //�J�E���g�\�����N���A
        dots.GetComponent<CountDotCtrl>().AllClear();
        //�T�E���h���X�g�b�v
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


             if (jitanRemains > 0)//���Z�񐔃Z�b�g����Ă�����
            {
                StartRush();
            }
            else {  //���Z�łȂ���ΏI��
                isJitan = false;
                gameStatus = GameStatus.IDLE;
                DisplayCtrl.GetComponent<DisplayCtrl>().ShowIdleText();
                GeneralManager.gameStatus = 0;//�ʏ�
                SoundManager.instance.PlayBGM(0);
                SoundManager.instance.FadeInBGM();
            }

        });

    }

    //������I��///////////////////////////////////////////////////////////
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
            mainTween = null; // �Â�Tween���Q�Ƃ��Ȃ��悤��
        }

        //Debug.Log("isVPass =" + isVPass);

        mainTween = DOVirtual.DelayedCall(delay, () =>
        {
            if (!isVPass)
            {
                //Debug.Log("isVPass = False");
                //V�ʉ߂��Ȃ���΁A��莞�Ԍ�ɃA�C�h����ԂɈڍs
                //V�ʉߎ���V�A�j���[�V�������J�n�A�I�����SetupAtariAction()���Ăяo�����

                if (isJitan)
                {
                    gameStatus = GameStatus.RUSH;
                }
                else {
                    //Debug.Log("IDle�ɕύX");
                    gameStatus = GameStatus.IDLE;
                    DisplayCtrl.GetComponent<DisplayCtrl>().ShowIdleText();
                }

            }


        });

        // ��ԊĎ��R���[�`���J�n
        StartCoroutine(MonitorStateCoroutine());

        lightManager.GetComponent<CenterLightManager>().LightFlash(0);
    }

    IEnumerator MonitorStateCoroutine()
    {
        while (mainTween != null && mainTween.IsActive() && mainTween.IsPlaying())
        {
            if (isVPass||gameStatus==GameStatus.STARTACTION) // ��ԕω������i�����͎��R�ɕύX�j
            {
                mainTween.Kill(); // DelayedCall�L�����Z��
                //Debug.Log("��ԕω������m���ăL�����Z���A�ʏ������s");


                yield break; // �R���[�`���I��
            }

            yield return null; // ���̃t���[���܂őҋ@
        }
    }


    public void OnRushStartAnimeFinished()
    {
        DisplayCtrl.GetComponent<DisplayCtrl>().ShowJitanRemain(jitanRemains);
    }

    //Rush�J�n///////////////////////////////////////////////////////////////////
    void StartRush()
    {
        gameStatus = GameStatus.RUSH;
        isVPass = false;
        isJitan = true;
        GeneralManager.gameStatus = 1;//���Z


    }
    //���Z�J�E���g�_�E��///////////////////////////////////////////////////////
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

            GeneralManager.gameStatus = 0;//�ʏ�
            //BGM�Đ�
            SoundManager.instance.PlayBGM(0);
        }

    }


    //�X�e�[�^�X�̎擾/////////////////////////////////////////////////////
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
