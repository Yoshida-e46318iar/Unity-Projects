using UnityEngine;
using UnityEngine.Video;

public class USBMemoryManager : MonoBehaviour
{
    [SerializeField] GameObject centerCtrl;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject denchyuCtrl;
    [SerializeField] GameObject vRollingCtrl;
    [SerializeField] GameObject DisplayCanvas;
    [SerializeField] string movieFileName;

    GameObject generalCanvas;
    GameObject itemCanvas;
    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            
        }

            // イベントに関数を登録
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.N))
    //        PlayMovie();

    //}

    public void ItemEffectOn()
    {
        videoPlayer.gameObject.SetActive(true);
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, movieFileName);
        DisplayCanvas.SetActive(false);
        generalCanvas = GameObject.FindGameObjectWithTag("GeneralCanvas");
        generalCanvas.SetActive(false);
        itemCanvas = GameObject.FindGameObjectWithTag("ItemCanvas");
        itemCanvas.SetActive(false);
        SoundManager.instance.FadeOutBGM();

        videoPlayer.url = path;
        videoPlayer.Play();



    }

    void OnVideoFinished(VideoPlayer vp)
    {
        //Debug.Log("動画の再生が終了しました");
        videoPlayer.gameObject.SetActive(false);
        DisplayCanvas.SetActive(true);
        generalCanvas.SetActive(true);
        itemCanvas.SetActive(true);
        SoundManager.instance.FadeInBGM();

        ItemEffectStart();
    }

    void ItemEffectStart()
    {
        //アイテム使用をオンに
        centerCtrl.GetComponent<CenterCtrl>().isItemUSBUsed = true;

        //賞球を１０００倍
        //スペックオブジェクトの９番の賞球を使用するように、CenterCtrl側で調整

        //電チューを開きっぱなしに
        denchyuCtrl.GetComponent<DenchuCtrl>().ItemEffectStart();

        //羽根の開放時間を延長
        centerCtrl.GetComponent<CenterCtrl>().isItemSpringUsed = true;

        //Vの回転を停止
        vRollingCtrl.GetComponent<VRollingCtrl>().StopUp();

        //玉分裂
        centerCtrl.GetComponent<CenterCtrl>().isItemMedicineUsed = true;

        //ラウンドを15Rのみに
        //CenterCtrlの図柄抽選を必ず0に

    }

}
