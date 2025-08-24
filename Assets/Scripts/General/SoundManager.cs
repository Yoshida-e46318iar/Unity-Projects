using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] AudioSource audioSourceBGM;
    [SerializeField] AudioSource audioSourceSE;
    [SerializeField] Toggle toggleSound;
    [SerializeField] AudioClip[] audioClips_SE;
    [SerializeField] AudioClip[] audioClips_BGM;

    bool isSoundEnable = true;
   
    int currentBGMNum = 0;
    Coroutine playcheckCoroutine;
    int currentPlayMode = 0;

    void Awake()
    {
        CheckInstance();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (PlayerPrefs.GetInt("isSoundEnable",1) == 1)
            toggleSound.isOn = true;
        else
            toggleSound.isOn = false;

    }

    void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //サウンド/////////////////////////////////////////////////////

    public void OnSoundOnOffButtonDown()
    {
        toggleSound.isOn = toggleSound.isOn ? true : false;
        GameObject tempvideoplayer = GameObject.Find("Video Player");

        if (!toggleSound.isOn)
        {
            audioSourceBGM.mute = true;
            audioSourceSE.mute = true;

            isSoundEnable = false;
            if (tempvideoplayer != null)
            {
                tempvideoplayer.GetComponent<VideoPlayer>().SetDirectAudioMute(0, true);
            }

        }
        else
        {
            audioSourceBGM.mute = false;
            audioSourceSE.mute = false;
            isSoundEnable = true;
            if (tempvideoplayer != null)
            {
                tempvideoplayer.GetComponent<VideoPlayer>().SetDirectAudioMute(0, false);
            }
        }

        if (isSoundEnable)
        {
            PlayerPrefs.SetInt("isSoundEnable", 1);
        }
        else
            PlayerPrefs.SetInt("isSoundEnable", 0);

    }

    public void SoundMuteCheck()
    {
        if (!isSoundEnable)
        {
            GameObject tempvideoplayer = GameObject.Find("Video Player");
            audioSourceBGM.mute = true;
            isSoundEnable = false;
            if (tempvideoplayer != null)
            {
                tempvideoplayer.GetComponent<VideoPlayer>().SetDirectAudioMute(0, true);
            }

        }

    }


    public AudioSource GetAudioSource()
    {
        return audioSourceBGM;
    }

    //サウンド再生///////////////////////////////////////////////////
    public void PlaySound(int mode,int index)
    {
        if(mode==0)
            audioSourceSE.PlayOneShot(audioClips_SE[index]);
        else {
            audioSourceBGM.clip = audioClips_BGM[index];
            audioSourceBGM.Play();
        }
    }

   public void PlaySe(AudioClip clip)
    {
        audioSourceSE.PlayOneShot(clip);

    }

    public void PlaySELoop(int clipIndex)
    {
        audioSourceSE.loop = true;
        audioSourceSE.clip = audioClips_SE[clipIndex];
        audioSourceSE.Play();

    }

    public void StopSE()
    {
        audioSourceSE.Stop();
    }


    //ボタンクリック音////////////////////////////////////////////////////////
    public void PlayButtonSEOK()
    {
        audioSourceSE.PlayOneShot(audioClips_SE[0]);
    }

    public void PlayButtonSECancel()
    {
        audioSourceSE.PlayOneShot(audioClips_SE[1]);
    }
    
    //通常時BGM/////////////////////////////////////////////////////////////
    public void PlayBGM(int mode)
    {
        audioSourceBGM.loop = false;
        StopBGM();
        currentPlayMode = mode;
        int bgmCount = audioClips_BGM.Length;
        int nextBGMNum = 0;
        int bgmindexStart = 0;
        if (currentPlayMode < 2)
        {
            if (currentPlayMode == 1)
            {
                bgmindexStart = 5;
            }
            nextBGMNum = UnityEngine.Random.Range(bgmindexStart, bgmCount - 2);

            while (nextBGMNum == currentBGMNum)
            {

                nextBGMNum = UnityEngine.Random.Range(bgmindexStart, bgmCount - 2);

            }

            currentBGMNum = nextBGMNum;

        }
        else if(currentPlayMode==3)
            currentBGMNum = 11;

        else
        {
            currentBGMNum = 10;

        }

        playcheckCoroutine = StartCoroutine("Playcheckloop");

    }

    public void PlayBGMNumber(int bgmNumber)
    {
        audioSourceBGM.loop = true;

        audioSourceBGM.PlayOneShot(audioClips_BGM[bgmNumber]);


    }

    public void FadeInBGM()
    {
        audioSourceBGM.DOFade(0.3f, 2f);

    }

    public void FadeOutBGM()
    {
        audioSourceBGM.DOFade(0f, 2f);


    }

    public void StopBGM()
    {
        if (audioSourceBGM.isPlaying)
        {
            audioSourceBGM.Stop();
            if(playcheckCoroutine!=null)
             StopCoroutine(playcheckCoroutine);
        }


    }

    IEnumerator Playcheckloop()
    {
        audioSourceBGM.PlayOneShot(audioClips_BGM[currentBGMNum]);

        yield return new WaitWhile(() => audioSourceBGM.isPlaying);

        PlayBGM(currentPlayMode);
    }



}


