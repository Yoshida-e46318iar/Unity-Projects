using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Video;
public class StartMsgManager : MonoBehaviour
{
    [SerializeField] GameObject msgText;
    [SerializeField] Canvas currentCanvas;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] string movieFileName;
    void Start()
    {
        
    }

    public void ShowMsg()
    {
        PlayOpenningMovie();

        msgText.transform.DOLocalMoveY(6328, 80f)
             .SetDelay(2f)
            .SetEase(Ease.Linear)
        .OnComplete(() => HideMsg());


    }

    void HideMsg()
    {

        canvasGroup.DOFade(0, 1.5f)
          .OnComplete(() => {

            GeneralManager.isStartMsgShowed = 1;
            GeneralManager.instance.SaveGameData();
            GeneralManager.instance.ShowDefCanvas(true);
 
            currentCanvas.gameObject.SetActive(false);
            SoundManager.instance.StopBGM();
            SoundManager.instance.PlayBGMNumber(12);
            SoundManager.instance.FadeInBGM();

        });

    }

    public void StopMsg()
    {
        currentCanvas.gameObject.SetActive(false);
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlayBGMNumber(12);
        SoundManager.instance.FadeInBGM();

        GeneralManager.isStartMsgShowed = 1;
        GeneralManager.instance.SaveGameData();
        GeneralManager.instance.ShowDefCanvas(true);
  
    }

    void PlayOpenningMovie()
    {

        string path = System.IO.Path.Combine(Application.streamingAssetsPath, movieFileName);

        videoPlayer.url = path;
        videoPlayer.Play();

    }
}
