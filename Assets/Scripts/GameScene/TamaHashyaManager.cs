using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TamaHashyaManager : MonoBehaviour
{
    [SerializeField] GameObject tamaPrefab;
    [SerializeField] GameObject tamaSpawnPos;
    [SerializeField] GameObject displayManager;
    [SerializeField] GameObject payoutManager;
    [SerializeField] GameObject GameSceneManager;
    [SerializeField] int tamaCount;
    [SerializeField] float DefaultPower;
    [SerializeField] float Power;
    [SerializeField] Slider sliderPower;
    [SerializeField] TMP_Text buttonHashyaText;
    [SerializeField] Toggle toggleAutoTamakashi;
    [SerializeField] Button buttonHashya;

    
    GameObject[] tamaStocks;
    bool isHashyaDoing = false;
    const float HASHYAINTERVAL = 60f / 100f;
    bool isBGMPlayFirstTime = true;

    float lastUpdate = 0f;

    void Start()
    {
        SetupTamaObjects();
        Power = DefaultPower;


    }



    void SetupTamaObjects()
    {
        tamaStocks=new GameObject[tamaCount];

        for (int i = 0; i < tamaCount; i++)
        {
            tamaStocks[i]= Instantiate(tamaPrefab, transform.position, transform.rotation);
            tamaStocks[i].SetActive(false);

        }

    }

    public void OnHashyaButtonDown()
    {
        if (!isHashyaDoing)
            StartHashyaLoop(true);
        else
            StartHashyaLoop(false);

    }

    public void StartHashyaLoop(bool start)
    {
        SoundManager.instance.PlayButtonSEOK();
        if (start)
        {
            isHashyaDoing = true;
            buttonHashyaText.text = "停　止";
            lastUpdate = Time.time;

            if (isBGMPlayFirstTime) { 
                SoundManager.instance.PlayBGM(0);
                isBGMPlayFirstTime = false;
                SoundManager.instance.FadeInBGM();
            }
            else
                SoundManager.instance.FadeInBGM();

        }
        else
        {
            isHashyaDoing = false;
            buttonHashyaText.text = "発　射";
            GameSceneManager.GetComponent<GameSceneManager>().OnHashyaStop();

        }
    }
    public void Hashya()
    {
        float currentPower = Power;
        Rigidbody rb;


        for (int i = 0; i < tamaCount; i++)
        {//配列の中から使っていないものを検索


            if (!tamaStocks[i].activeSelf)　//オブジェクト非アクティブならこれを使う
            {

                //玉を発射するための準備　位置、角度、移動量を初期化　
                tamaStocks[i].transform.position = tamaSpawnPos.transform.position;
                tamaStocks[i].transform.rotation = Quaternion.identity;
                rb = tamaStocks[i].GetComponent<Rigidbody>();
                rb.linearVelocity= new Vector3(0, 0, 0); //前回のものが残っていると不都合なので


                //初期化したらアクティブに
                tamaStocks[i].SetActive(true);

                //盤面上に発射

                rb.AddForce(new Vector3(-1f,0.5f,0) * currentPower, ForceMode.VelocityChange);

                //トータルアウトを更新
                GeneralManager.instance.OutAdd();

                //持ち球を更新
                payoutManager.GetComponent<PayoutManager>().Payout(-1);

                //自動玉貸し処理
                if (GeneralManager.mochidama<=30)
                {
                    if (toggleAutoTamakashi.isOn)
                    {
                        if(GeneralManager.amountGold>0)
                           payoutManager.GetComponent<PayoutManager>().TamaKashi();
                    }
                }

                //持ち球がなくなった場合
                if (GeneralManager.mochidama<=0)
                {

                    isHashyaDoing = false;
                    buttonHashya.interactable = false;

                    if (GeneralManager.amountGold<=0)
                    {
                        GotoMissionFailScene();
                    }


                }
                //非アクティブなものが見つかったので、検索は終了
                break;
            }

        }


    }

    private void FixedUpdate()
    {
        if (isHashyaDoing)
        {
            if (Time.time - lastUpdate > HASHYAINTERVAL) { 
                Hashya();
                lastUpdate = Time.time;
            }
        }
    }
    public void OnSliderPowerValueChange()
    {
        Power =sliderPower.value;

    }


    void GotoMissionFailScene()
    {
        SoundManager.instance.FadeOutBGM();
        SoundManager.instance.StopBGM();

        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(4),2f,0,1);
        SoundManager.instance.PlayButtonSEOK();
    }

    public bool CheckHashyaDoing()
    {
        return isHashyaDoing;

    }
}
