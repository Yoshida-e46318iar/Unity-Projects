using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] GameObject resultsTextParent;
    [SerializeField] GameObject playFabLoginManager;
    [SerializeField] GameObject playFabRankingManager;
    [SerializeField] TMP_Text totalOutText;
    [SerializeField] TMP_Text totalOutPositionText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text goldPositionText;
    [SerializeField] Button gotoTitleButton;
    int totalOutPosition = 9999;
    int goldPosition = 9999;
    

    void Start()
    {
        gotoTitleButton.gameObject.SetActive(false);
        if (GeneralManager.clearedMissionNum == 9)
            playFabLoginManager.GetComponent<PlayFabLogin>().Login();

    }

    public void OnLoginSuccess()
    {
        //Debug.Log("ログイン成功通知　キャッチ");

        playFabRankingManager.GetComponent<PlayFabRainkingManager>().UpdatePlayerStatistics(
            0, GeneralManager.totalOut);

        playFabRankingManager.GetComponent<PlayFabRainkingManager>().UpdatePlayerStatistics(
           1, GeneralManager.amountGold);

        playFabRankingManager.GetComponent<PlayFabRainkingManager>().GetLeaderboardAroundPlayerSingle(0);

    }

    public void ShowResults()
    {
        resultsTextParent.transform.DOLocalMoveY(750, 20f)
            .OnComplete(() => Showbutton());


    }

    public void SetupText()
    {
        totalOutText.text = GeneralManager.totalOut.ToString("D7") + "発";
        goldText.text = GeneralManager.amountGold.ToString("D7") + "発";
        totalOutPositionText.text = totalOutPosition.ToString("D4") + "位";
        goldPositionText.text = goldPosition.ToString("D4") + "位";

    
    }

    void Showbutton()
    {
        gotoTitleButton.gameObject.SetActive(true);
    }

    public void OnGotoTiteButtonDown()
    {
        GeneralManager.instance.ChageScene(GeneralManager.instance.GetSceneName(0), 2f, 0, 1);
    }

    public void OnSendTotalOutPos()
    {
        totalOutPosition = playFabRankingManager.GetComponent<PlayFabRainkingManager>().GetPosition(0);
        playFabRankingManager.GetComponent<PlayFabRainkingManager>().GetLeaderboardAroundPlayerSingle(1);
    }
    public void OnSendGoldPos()
    {
        goldPosition =  playFabRankingManager.GetComponent<PlayFabRainkingManager>().GetPosition(1);
        SetupText();
    }
}
