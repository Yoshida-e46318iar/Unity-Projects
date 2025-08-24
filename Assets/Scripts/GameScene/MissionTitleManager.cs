using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionTitleManager : MonoBehaviour
{
    [SerializeField] Canvas canvasTitle;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image imageTitle;
    [SerializeField] Sprite[] sprites;
    [SerializeField] TMP_Text machineNum;

    void Start()
    {

        imageTitle.sprite = sprites[GeneralManager.currentMissionNum];
        canvasTitle.gameObject.SetActive(true);

        machineNum.text = "Machine No. " + (GeneralManager.currentMachineNum+1).ToString();
    }


    public void FadeOut(float duration)
    {
        canvasGroup.alpha = 1;
        canvasGroup.DOFade(0, duration).OnComplete(() => {
            
            canvasTitle.gameObject.SetActive(false);

         });
    }



}
