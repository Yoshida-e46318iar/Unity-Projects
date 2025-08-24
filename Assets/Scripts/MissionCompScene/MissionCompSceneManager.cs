using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MissionCompSceneManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
  //  [SerializeField] VideoClip[] videoClips;
    [SerializeField] float[] movieDurations;
    [SerializeField] GameObject prizeManager;
    [SerializeField] GameObject resultsManager;
    [SerializeField] int lifeRewardCount;
    [SerializeField] string[] movieFileName;

    int prizeGold = 0;
    void Start()
    {
        GeneralManager.instance.FadeIn(2f);
        GeneralManager.lastSceneName = SceneManager.GetActiveScene().name;
        videoPlayer.SetTargetAudioSource(0, SoundManager.instance.GetAudioSource());
        SoundManager.instance.SoundMuteCheck();

        prizeGold = GeneralManager.instance.GetPrize();



        Invoke("MoviePlay", 2);
        //���[�r�[�̍Đ����I��邱��Ɏ��s

        if (GeneralManager.clearedMissionNum == 9)
            DOVirtual.DelayedCall(
             delay: movieDurations[GeneralManager.clearedMissionNum], //���b��Ɏ��s���邩
             callback: () => resultsManager.GetComponent<ResultsManager>().ShowResults()

            );

        else
            DOVirtual.DelayedCall(
                 delay: movieDurations[GeneralManager.clearedMissionNum], //���b��Ɏ��s���邩
                 callback: () => prizeManager.GetComponent<PrizeManager>().AddPrize(prizeGold, lifeRewardCount)//�x������

            );
    }

    //void MoviePlay()
    //{
    //    videoPlayer.clip = videoClips[GeneralManager.clearedMissionNum];
    //    videoPlayer.Play();

    //}

    void MoviePlay()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, movieFileName[GeneralManager.clearedMissionNum]);

        videoPlayer.url = path;
        videoPlayer.Play();

    }




}
