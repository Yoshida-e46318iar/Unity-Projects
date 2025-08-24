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
       //�A�C�e�����g�p����Ă�����A�J�����ςȂ��ɂ��邽�߂ɏ���������[
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
                isActionDoing = false;
                StopCoroutine(actionCoroutine);
            }


        }
        isActionDoing = false;

    }




    //�V���b�^�[�J�����̕ʂ̃R���[�`��

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




    //�ʂ���萔���������Ƃɂ��J����r���I������ꍇ�̏���

    //�ʂ̃X�N���v�g����Ăяo�����

    public void ActionStop()
    {

        //�J�E���g�_�E���p�̃^�C�}�[��0�ɂ��āA�R���[�`�����O������I��������
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
