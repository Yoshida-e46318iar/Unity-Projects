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
    //スタート、大当り回数、連荘回数、連荘出玉、最高出玉、最高連荘回数

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



        //ライフの減算
        if (GeneralManager.instance.CheckLife()&&!GeneralManager.instance.CheckMachineChange())
            GeneralManager.instance.LifeRemainIncDec(-1);

    }


    //スタートセンサー通過時/////////////////////////////////////////
    public void OnStartSencerOn(int index)
    {
        if (GeneralManager.currentMissionNum == 0)
        {
            MissionClear();
        }
        else
            CenterCtrl.GetComponent<CenterCtrl>().ActionStart(index);

    }

    //ゲームデータ表示用/////////////////////////////////////////////////////////////
    //スタート通過による小当り開始///////////////////////////////////////
    public void OnShortActionStart()
    {
        UpdateGameData(0,1, false);

    }

    //大当り開始時//////////////////////////////////////////////////////////
    public void OnAtariStart(int status)
    {
        //大当り回数
        UpdateGameData(1,1, false);

        //連荘回数
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

        //最大連荘回数
        if (GameDatas[2] >= GameDatas[5])
        {

            UpdateGameData(5, 1, false);
        }


        //連荘出玉計数用
        if (!isRenchan)
        {
            //初期化
            UpdateGameData(3, 0, true);
            isRenchan = true;
        }


    }


    //大当り動作終了時//////////////////////////////////////////////////////////////
    public void OnAtariFinished()
    {

        UpdateGameData(0,1, true);

    }

    //連荘終了///////////////////////////////////////////////////////////////////////
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

    //ゲームデータ表示////////////////////////////////////////////
    public void UpdateGameData(int index,int value, bool init)
    {
        //0～5:スタート、大当り回数、連荘回数、連荘出玉、最高出玉、最高連荘回数

        if (init)
            GameDatas[index] = 0;
        else
            GameDatas[index]+=value;

        DataDisplayCtrl.GetComponent<DataDisplayManager>().UpdateDisplayText
                (index + 1, GameDatas[index], false);

    }

    public void GotoRankingScene()
    {
        //発射を停止
        if (HashyaManager.GetComponent<TamaHashyaManager>().CheckHashyaDoing())
            HashyaManager.GetComponent<TamaHashyaManager>().StartHashyaLoop(false);

        //持ち球を清算
        GeneralManager.instance.CheckOut();

        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();

        SoundManager.instance.PlayButtonSEOK();
        if (GeneralManager.totalOut > 0)
        {
            GeneralManager.instance.SaveGameData();

        }

    }

    //清算してタイトルへ戻る/////////////////////////////////////////////////////
    public void OnChecOut()
    {
        //発射を停止
        if (HashyaManager.GetComponent<TamaHashyaManager>().CheckHashyaDoing())
            HashyaManager.GetComponent<TamaHashyaManager>().StartHashyaLoop(false);

        //持ち球を清算
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
    //台移動/////////////////////////////////////////////////////////////////////////
    public void OnMachineChange()
    {
        //発射を停止
        if (HashyaManager.GetComponent<TamaHashyaManager>().CheckHashyaDoing())
            HashyaManager.GetComponent<TamaHashyaManager>().StartHashyaLoop(false);

        //持ち球を清算
        GeneralManager.instance.CheckOut();

        SoundManager.instance.PlayButtonSEOK();
        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();
        if (GeneralManager.totalOut > 0)
        {
            GeneralManager.instance.SaveGameData();

        }

        //ライフが減らないように設定
        GeneralManager.instance.SetMachineChanged();

        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(1), 2f, 0, 1);
    }
    //視点切り替え////////////////////////////////////////////////////////////////////////
    public void OnViewChange()
    {
        SoundManager.instance.PlayButtonSEOK();
    }


    //ミッション表示///////////////////////////////////////
    void UpdateMissionTerop()
    {
        missionTerop.text= "Mission "+( GeneralManager.currentMissionNum+1).ToString()+"  "+
            GeneralManager.instance.GetCurrentMissionData().missionTitleShort;

    }

    //ミッション達成条件クリア時////////////////////////////////////////////////
   public void MissionClear()
    {
        //Debug.Log("MissionClear called");

        //物理演算を停止
        Physics.simulationMode = SimulationMode.Script;

        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();


        //発射を停止
        if (HashyaManager.GetComponent<TamaHashyaManager>().CheckHashyaDoing())
            HashyaManager.GetComponent<TamaHashyaManager>().StartHashyaLoop(false);


        DOVirtual.DelayedCall(1f, () => {
            SoundManager.instance.PlaySound(0, 18);
        });

        DOVirtual.DelayedCall(2f, () => {
            //持ち球を清算
            GeneralManager.instance.CheckOut();

            //ミッションクリア
            GeneralManager.clearedMissionNum = GeneralManager.currentMissionNum;
            GeneralManager.currentMissionNum++;

            //使用した台を使用済に
            GeneralManager.machineEnable[GeneralManager.currentMachineNum] = 1;

            //ゲームデータをセーブ
            GeneralManager.instance.SaveGameData();



            //ミッションコンプリートシーンへシーン移動
            GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(3), 2f, 0, 1);
        });




    }

    void CheckMissionComp()
    {
    

        //連荘回数
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

        //連荘出玉
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
