using DG.Tweening;
using DG.Tweening.CustomPlugins;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] GameObject CenterCtrl;
    [SerializeField] GameObject HashyaManager;
    [SerializeField] GameObject DataDisplayCtrl;
    [SerializeField] TMP_Text missionTerop;
    [SerializeField] GameObject MissionTitleManager;

    int[] GameDatas = new int[6];

    bool isRenchan = false;
    //�X�^�[�g�A�哖��񐔁A�A���񐔁A�A���o�ʁA�ō��o�ʁA�ō��A����

    void Start()
    {
        GeneralManager.lastSceneName = SceneManager.GetActiveScene().name;
        Physics.simulationMode = SimulationMode.FixedUpdate;
        GeneralManager.instance.FadeIn(2.5f);
        DOVirtual.DelayedCall(4f, () => {
            MissionTitleManager.GetComponent<MissionTitleManager>().FadeOut(0.5f);
        });


        SoundManager.instance.SoundMuteCheck();
        SoundManager.instance.FadeInBGM();

        UpdateMissionTerop();
        for (int i = 0; i < GameDatas.Length; i++)
        {
            GameDatas[i] = 0;
        }

        CenterCtrl.GetComponent<CenterCtrl>().StartAtariEvent.AddListener(OnAtariStart);
        CenterCtrl.GetComponent<CenterCtrl>().PayOutEvent.AddListener(OnPayOut);



        //���C�t�̌��Z
        if (GeneralManager.instance.CheckLife()&&!GeneralManager.instance.CheckMachineChange())
            GeneralManager.instance.LifeRemainIncDec(-1);

    }


    //�X�^�[�g�Z���T�[�ʉߎ�/////////////////////////////////////////
    public void OnStartSencerOn(int index)
    {
        if (GeneralManager.currentMissionNum == 0)
        {
            MissionClear();
        }
        else
            CenterCtrl.GetComponent<CenterCtrl>().ActionStart(index);

    }

    //�Q�[���f�[�^�\���p/////////////////////////////////////////////////////////////
    //�X�^�[�g�ʉ߂ɂ�鏬����J�n///////////////////////////////////////
    public void OnShortActionStart()
    {
        UpdateGameData(0,1, false);

    }

    //�哖��J�n��//////////////////////////////////////////////////////////
    public void OnAtariStart(int status)
    {
        //�哖���
        UpdateGameData(1,1, false);

        //�A����
        if (status==0)
        {
            UpdateGameData(2,1, true);
            UpdateGameData(2,1, false);
        }
        else
        {
            UpdateGameData(2,1, false);

        }
        CheckMissionComp();

        //�ő�A����
        if (GameDatas[2] >= GameDatas[5])
        {

            UpdateGameData(5, 1, false);
        }


        //�A���o�ʌv���p
        if (!isRenchan)
        {
            //������
            UpdateGameData(3, 0, true);
            isRenchan = true;
        }


    }


    //�哖�蓮��I����//////////////////////////////////////////////////////////////
    public void OnAtariFinished()
    {

        UpdateGameData(0,1, true);

    }

    //�A���I��///////////////////////////////////////////////////////////////////////
    public void OnJitanEnd()
    {

        isRenchan = false;
    }

    public void OnPayOut(int value)
    {
        if (isRenchan)
        {
            UpdateGameData(3, value, false);
        //   Debug.Log("OnPayOut Value=" + value);

            if (GameDatas[4] <= GameDatas[3])
            {
                UpdateGameData(4, value, false);
            }

            CheckMissionComp();
        }
    }

    //�Q�[���f�[�^�\��////////////////////////////////////////////
    public void UpdateGameData(int index,int value, bool init)
    {
        //0�`5:�X�^�[�g�A�哖��񐔁A�A���񐔁A�A���o�ʁA�ō��o�ʁA�ō��A����

        if (init)
            GameDatas[index] = 0;
        else
            GameDatas[index]+=value;

        DataDisplayCtrl.GetComponent<DataDisplayManager>().UpdateDisplayText
                (index + 1, GameDatas[index], false);

    }

    public void GotoRankingScene()
    {
        //���˂��~
        if (HashyaManager.GetComponent<TamaHashyaManager>().CheckHashyaDoing())
            HashyaManager.GetComponent<TamaHashyaManager>().StartHashyaLoop(false);

        //�������𐴎Z
        GeneralManager.instance.CheckOut();

        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();

        SoundManager.instance.PlayButtonSEOK();
        if (GeneralManager.totalOut > 0)
        {
            GeneralManager.instance.SaveGameData();

        }

    }

    //���Z���ă^�C�g���֖߂�/////////////////////////////////////////////////////
    public void OnChecOut()
    {
        //���˂��~
        if (HashyaManager.GetComponent<TamaHashyaManager>().CheckHashyaDoing())
            HashyaManager.GetComponent<TamaHashyaManager>().StartHashyaLoop(false);

        //�������𐴎Z
        GeneralManager.instance.CheckOut();


        SoundManager.instance.PlayButtonSEOK();
        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();
        if (GeneralManager.totalOut>0)
        {
            GeneralManager.instance.SaveGameData();

        }

        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(6), 2f, 0, 1);
    }
    //��ړ�/////////////////////////////////////////////////////////////////////////
    public void OnMachineChange()
    {
        //���˂��~
        if (HashyaManager.GetComponent<TamaHashyaManager>().CheckHashyaDoing())
            HashyaManager.GetComponent<TamaHashyaManager>().StartHashyaLoop(false);

        //�������𐴎Z
        GeneralManager.instance.CheckOut();

        SoundManager.instance.PlayButtonSEOK();
        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();
        if (GeneralManager.totalOut > 0)
        {
            GeneralManager.instance.SaveGameData();

        }

        //���C�t������Ȃ��悤�ɐݒ�
        GeneralManager.instance.SetMachineChanged();

        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(1), 2f, 0, 1);
    }
    //���_�؂�ւ�////////////////////////////////////////////////////////////////////////
    public void OnViewChange()
    {
        SoundManager.instance.PlayButtonSEOK();
    }


    //�~�b�V�����\��///////////////////////////////////////
    void UpdateMissionTerop()
    {
        missionTerop.text= "Mission "+( GeneralManager.currentMissionNum+1).ToString()+"  "+
            GeneralManager.instance.GetCurrentMissionData().missionTitleShort;

    }

    //�~�b�V�����B�������N���A��////////////////////////////////////////////////
   public void MissionClear()
    {
        //Debug.Log("MissionClear called");

        //�������Z���~
        Physics.simulationMode = SimulationMode.Script;

        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();


        //���˂��~
        if (HashyaManager.GetComponent<TamaHashyaManager>().CheckHashyaDoing())
            HashyaManager.GetComponent<TamaHashyaManager>().StartHashyaLoop(false);


        DOVirtual.DelayedCall(1f, () => {
            SoundManager.instance.PlaySound(0, 18);
        });

        DOVirtual.DelayedCall(2f, () => {
            //�������𐴎Z
            GeneralManager.instance.CheckOut();

            //�~�b�V�����N���A
            GeneralManager.clearedMissionNum = GeneralManager.currentMissionNum;
            GeneralManager.currentMissionNum++;

            //�g�p��������g�p�ς�
            GeneralManager.machineEnable[GeneralManager.currentMachineNum] = 1;

            //�Q�[���f�[�^���Z�[�u
            GeneralManager.instance.SaveGameData();



            //�~�b�V�����R���v���[�g�V�[���փV�[���ړ�
            GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(3), 2f, 0, 1);
        });




    }

    void CheckMissionComp()
    {
    

        //�A����
        if (GeneralManager.currentMissionNum == 3 || GeneralManager.currentMissionNum == 6)
        {
            if (GeneralManager.instance.GetCurrentMissionData().value - GameDatas[2] <= 2)
            {
                DataDisplayCtrl.GetComponent<DataDisplayManager>().MissionAlmostComp(true);

            }

            if (GeneralManager.instance.GetCurrentMissionData().value == GameDatas[2])
            {
                MissionClear();
            }


        }

        //�A���o��
        if (GeneralManager.currentMissionNum == 5||GeneralManager.currentMissionNum == 7)
        {
            if (GeneralManager.instance.GetCurrentMissionData().value - GameDatas[3] <= 200)
            {
                DataDisplayCtrl.GetComponent<DataDisplayManager>().MissionAlmostComp(true);

            }

            if (GeneralManager.instance.GetCurrentMissionData().value <= GameDatas[3])
            {
                //Debug.Log("GamaDatas[3]="+GameDatas[3]);

                MissionClear();
            }
        }


    }


    public void OnHashyaStop()
    {
        if(CenterCtrl.GetComponent<CenterCtrl>().GetGameStatus()== 0
            || CenterCtrl.GetComponent<CenterCtrl>().GetGameStatus() == 1)
        {
            DOVirtual.DelayedCall(10f, () => {

                SoundManager.instance.FadeOutBGM();
            });


        }

    }
}
