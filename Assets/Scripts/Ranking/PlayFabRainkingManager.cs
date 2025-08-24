using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PlayFabRainkingManager : MonoBehaviour
{
    [SerializeField] string[] rankingNames;
    [SerializeField] TMP_Text[] rankingTexts;

    [SerializeField] UnityEvent saveSuccessEvent;
    [SerializeField] UnityEvent positionData0Event;
    [SerializeField] UnityEvent positionData1Event;
        

    int mode;
    int[] userPosition = new int[2];

    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";

    bool isplayfabLogin = false;


    private void Start()
    {

        //表示するランキングデータが二種類あるので、デフォルトを設定
        mode = 0;
    }


    public void OnLoginSuccess()
    {
        isplayfabLogin = true;

        //Debug.Log("ランキングデータの取得開始イベントキャッチ");

        //ログイン成功の通知を受けて、ランキングデータ取得関数を実行する
        GetLeaderboard(mode);
    }


    //ランキングデータの更新//////////////////////////////////////////////////////////////
    public void UpdatePlayerStatistics(int mode, int scoredata)
    {
        //Debug.Log("スコアの更新開始");

        UpdatePlayerStatisticsRequest request;

        if (mode == 0)
        {
            request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>{
                        new StatisticUpdate{
                            StatisticName = rankingNames[mode],   //ランキング名　二種類ある
                            Value =-scoredata　　　　　　　　　　　//ゲーム上のスコア 降順にしたいのでマイナス
                         }
                 }
            };
        }
        else
        {
            request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>{
                        new StatisticUpdate{
                            StatisticName = rankingNames[mode],   //ランキング名　二種類ある
                            Value =scoredata　　　　　　　　　　　//ゲーム上のスコア
                         }
                 }
            };

        }


        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdatePlayerStatisticsSuccess, OnUpdatePlayerStatisticsFailure);
    }

    //スコア(統計情報)の更新成功
    private void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        //Debug.Log($"スコア(統計情報)の更新が成功しました");

        saveSuccessEvent.Invoke();
    }

    //スコア(統計情報)の更新失敗
    private void OnUpdatePlayerStatisticsFailure(PlayFabError error)
    {
        //Debug.LogError($"スコア(統計情報)更新に失敗しました\n{error.GenerateErrorReport()}");
    }




    //ランキング取得//////////////////////////////////////////////////////////

    public void GetLeaderboard(int num)
    {
        //ランキングデータの取得開始
        //Debug.Log("ランキングデータの取得開始");
        mode = num;
        //GetLeaderboardRequestのインスタンスを生成
        var request = new GetLeaderboardRequest
        {

            //PlayFabに登録しているランキング名が二種類あるのでどちらを呼び出すかを指定　
            StatisticName = rankingNames[mode],
            StartPosition = 0,                 //何位以降のランキングを取得するか
            MaxResultsCount = 5                 //ランキングデータを何件取得するか(最大100)
        };

        //ランキング(リーダーボード)を取得

        if (mode == 0)
        {
            PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
        }
        else
            PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess2, OnGetLeaderboardFailure);
    }


    //ランキング(リーダーボード)の取得成功
    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        if(SceneManager.GetActiveScene().name != "RankingScene") 
            return;


        //Debug.Log("ランキングデータの取得成功");
        int myIDnum = 99999;

        //result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
        //Debug.Log("RankingData Count=" + result.Leaderboard.Count);

        if (result.Leaderboard.Count == 0)
        {

            rankingTexts[0].text = "ランキングデータがありません";

            for (int j = 1; j < 5; j++)
            {
                rankingTexts[j].text = "";
            }


            return;
        }



        int i = 1;

        //UITextのテキストをモードに応じたフォーマットに調整(空の場合も想定して)

        foreach (TMP_Text tmp_Text in rankingTexts)
        {
            tmp_Text.text = $"{i}位,0000000発,\n   NoName,ID : XXXXXXX";
            i++;
        }

        string[] str = new string[rankingTexts.Length];


        //取得したデータ数分のデータ取得し、名前が設定されていれば名前を表示し、設定されていなければNoNameと表示する
        i = 0;
        

        foreach (var entry in result.Leaderboard)
        {


            if (string.IsNullOrEmpty(entry.DisplayName))

                //DisplayNameが設定されていない場合                          
                str[i] = $"{entry.Position + 1}位,{-entry.StatValue}発, NoName\n ID : {entry.PlayFabId}";

            else
                //DisplayNameが設定されていた場合
                str[i] = $"{entry.Position + 1}位,{-entry.StatValue}発, {entry.DisplayName}\n ID : {entry.PlayFabId}";

            //自分のデータをアンダーライン表示するためにIDを照合して、合致したら順番を記憶

            if (entry.PlayFabId == PlayerPrefs.GetString("PlayFabID"))
            {
                myIDnum = i;
            }

            i++;
        }


        for (int j = 0; j < 5; j++)
        {
            if (!string.IsNullOrEmpty(rankingTexts[j].text))
                rankingTexts[j].text = str[j];

            if (j == myIDnum)

                //IDがユーザーと一致したら、フォントにアンダーラインをつける
                rankingTexts[j].fontStyle = FontStyles.Underline;
            else
                rankingTexts[j].fontStyle = FontStyles.Normal;


        }




    }

    private void OnGetLeaderboardSuccess2(GetLeaderboardResult result)
    {
        if (SceneManager.GetActiveScene().name != "RankingScene")
            return;

        
        //Debug.Log("ランキングデータの取得成功");
        int myIDnum = 99999;

        //result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている

        //Debug.Log("RankingData Count=" + result.Leaderboard.Count);

        if (result.Leaderboard.Count == 0)
        {

            rankingTexts[0].text = "ランキングデータがありません";

            for (int j = 1; j < 5; j++)
            {
                rankingTexts[j].text = "";
            }


            return;
        }
        int i = 1;

        //UITextのテキストをモードに応じたフォーマットに調整(空の場合も想定して)

        foreach (TMP_Text tmp_Text in rankingTexts)
        {
            tmp_Text.text = $"{i}位,0000000G, NoName \n ID : XXXXXXX";
            i++;
        }

        string[] str = new string[rankingTexts.Length];



        //取得したデータ数分のデータ取得し、名前が設定されていれば名前を表示し、設定されていなければNoNameと表示する
        i = 0;

        foreach (var entry in result.Leaderboard)
        {


            if (string.IsNullOrEmpty(entry.DisplayName))

                //DisplayNameが設定されていない場合


                str[i] = $"{entry.Position + 1}位,{entry.StatValue}発, NoName\n ID : {entry.PlayFabId}";


            else

                //DisplayNameが設定されていた場合
                str[i] = $"{entry.Position + 1}位,{entry.StatValue}発, {entry.DisplayName}\n ID : {entry.PlayFabId}";


            //自分のデータをアンダーライン表示するためにIDを照合して、合致したら順番を記憶

            if (entry.PlayFabId == PlayerPrefs.GetString("PlayFabID"))
            {
                myIDnum = i;
            }

            i++;

        }


        for (int j = 0; j < 5; j++)
        {
            if (!string.IsNullOrEmpty(rankingTexts[j].text))
                rankingTexts[j].text = str[j];

            if (j == myIDnum)

                //IDがユーザーと一致したら、フォントにアンダーラインをつける
                rankingTexts[j].fontStyle = FontStyles.Underline;
            else
                rankingTexts[j].fontStyle = FontStyles.Normal;


        }


    }

    //ランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        //Debug.LogError($"ランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }


    //プレイヤーの周辺のデータの表示/////////////////////////////////////////////////////////
    public void GetLeaderboardAroundPlayer(int num)
    {
        //ランキングデータの取得開始
        //Debug.Log("ランキングデータの取得開始");
        mode = num;
        //GetLeaderboardRequestのインスタンスを生成
        var request = new GetLeaderboardAroundPlayerRequest
        {

            //PlayFabに登録しているランキング名が二種類あるのでどちらを呼び出すかを指定　
            StatisticName = rankingNames[mode],
            MaxResultsCount = 5                 //ランキングデータを何件取得するか(最大100)
        };

        //ランキング(リーダーボード)を取得

        if (mode == 0)
        {
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerSuccess, OnGetLeaderboardAroundPlayerFailure);
        }
        else
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerSuccess2, OnGetLeaderboardAroundPlayerFailure);

    }


    //ランキング(リーダーボード)の取得成功
    private void OnGetLeaderboardAroundPlayerSuccess(GetLeaderboardAroundPlayerResult result)
    {
        if (SceneManager.GetActiveScene().name != "RankingScene")
            return;

        //Debug.Log("ランキングデータの取得成功");
        int myIDnum = 99999;

        //result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
          //Debug.Log("RankingData Count=" + result.Leaderboard.Count);

        if (result.Leaderboard.Count == 0)
        {

            rankingTexts[0].text = "ランキングデータがありません";

            for (int j = 1; j < 5; j++)
            {
                rankingTexts[j].text = "";
            }


            return;
        }
        int i = 1;

        //UITextのテキストをモードに応じたフォーマットに調整(空の場合も想定して)

        foreach (TMP_Text tmp_Text in rankingTexts)
        {
            tmp_Text.text = $"{i}位,0000000発,\n   NoName,ID : XXXXXXX";
            i++;
        }

        string[] str = new string[rankingTexts.Length];


        //取得したデータ数分のデータ取得し、名前が設定されていれば名前を表示し、設定されていなければNoNameと表示する
        i = 0;

        foreach (var entry in result.Leaderboard)
        {


            if (string.IsNullOrEmpty(entry.DisplayName))

                //DisplayNameが設定されていない場合                          
                str[i] = $"{entry.Position + 1}位,{-entry.StatValue}発, NoName\n ID : {entry.PlayFabId}";

            else
                //DisplayNameが設定されていた場合
                str[i] = $"{entry.Position + 1}位,{-entry.StatValue}発, {entry.DisplayName}\n ID : {entry.PlayFabId}";

            //自分のデータをアンダーライン表示するためにIDを照合して、合致したら順番を記憶

            if (entry.PlayFabId == PlayerPrefs.GetString("PlayFabID"))
            {
                myIDnum = i;
            }

            i++;
        }


        for (int j = 0; j < 5; j++)
        {
            if (!string.IsNullOrEmpty(rankingTexts[j].text))
                rankingTexts[j].text = str[j];

            if (j == myIDnum)

                //IDがユーザーと一致したら、フォントにアンダーラインをつける
                rankingTexts[j].fontStyle = FontStyles.Underline;
            else
                rankingTexts[j].fontStyle = FontStyles.Normal;


        }




    }

    private void OnGetLeaderboardAroundPlayerSuccess2(GetLeaderboardAroundPlayerResult result)
    {
        if (SceneManager.GetActiveScene().name != "RankingScene")
            return;

        //Debug.Log("ランキングデータの取得成功");
        int myIDnum = 99999;

        //result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
        if (result.Leaderboard.Count == 0)
        {

            rankingTexts[0].text = "ランキングデータがありません";

            for (int j = 1; j < 5; j++)
            {
                rankingTexts[j].text = "";
            }


            return;
        }
        int i = 1;

        //UITextのテキストをモードに応じたフォーマットに調整(空の場合も想定して)

        foreach (TMP_Text tmp_Text in rankingTexts)
        {
            tmp_Text.text = $"{i}位,0000000G, NoName \n ID : XXXXXXX";
            i++;
        }

        string[] str = new string[rankingTexts.Length];


        //取得したデータ数分のデータ取得し、名前が設定されていれば名前を表示し、設定されていなければNoNameと表示する
        i = 0;

        foreach (var entry in result.Leaderboard)
        {


            if (string.IsNullOrEmpty(entry.DisplayName))

                //DisplayNameが設定されていない場合


                str[i] = $"{entry.Position + 1}位,{entry.StatValue}発, NoName\n ID : {entry.PlayFabId}";


            else

                //DisplayNameが設定されていた場合
                str[i] = $"{entry.Position + 1}位,{entry.StatValue}発, {entry.DisplayName}\n ID : {entry.PlayFabId}";


            //自分のデータをアンダーライン表示するためにIDを照合して、合致したら順番を記憶

            if (entry.PlayFabId == PlayerPrefs.GetString("PlayFabID"))
            {
                myIDnum = i;
            }

            i++;

        }


        for (int j = 0; j < 5; j++)
        {
            if (!string.IsNullOrEmpty(rankingTexts[j].text))
                rankingTexts[j].text = str[j];

            if (j == myIDnum)

                //IDがユーザーと一致したら、フォントにアンダーラインをつける
                rankingTexts[j].fontStyle = FontStyles.Underline;
            else
                rankingTexts[j].fontStyle = FontStyles.Normal;


        }


    }

    //ランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardAroundPlayerFailure(PlayFabError error)
    {
        //Debug.LogError($"ランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }


    //名前の登録//////////////////////////////////////////////////////////////////////////////////////

    public void SetUserName(string name)
    {
        // ユーザー名を設定するリクエストを作る
        var request = new UpdateUserTitleDisplayNameRequest
        {
            // ユーザー名の設定
            DisplayName = name
        };

        // リクエストをPlayFabに送信する
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSetUserNameSuccess, OnSetUserNameFailure);

        // 送信成功時の処理
        void OnSetUserNameSuccess(UpdateUserTitleDisplayNameResult result)
        {
            //Debug.Log("プレイヤー名の変更に成功しました");
        }

        // 送信失敗時の処理
        void OnSetUserNameFailure(PlayFabError error)
        {
            //Debug.Log("プレイヤー名の変更に失敗しました");
        }
    }

    //リザルト表示用のユーザーの順位の取得//////////////////////////////////////////////////////////////////

    public void GetLeaderboardAroundPlayerSingle(int num)
    {

        //ランキングデータの取得開始
        //Debug.Log("ランキングデータの取得開始");
        mode = num;

        //Debug.Log("Mode="+num+","+ rankingNames[mode]);
        GetLeaderboardAroundPlayerRequest request;

        //GetLeaderboardRequestのインスタンスを生成
         request = new GetLeaderboardAroundPlayerRequest
        {
            //PlayFabに登録しているランキング名が二種類あるのでどちらを呼び出すかを指定　
            StatisticName = rankingNames[mode],
            MaxResultsCount = 1                 //ユーザーのみ
        };

        //ランキング(リーダーボード)を取得

        if (mode == 0)
        {
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerSuccessSingle, OnGetLeaderboardAroundPlayerFailureSingle);
        }
        else
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerSuccessSingle2, OnGetLeaderboardAroundPlayerFailureSingle);

    }

    public void OnGetLeaderboardAroundPlayerSuccessSingle(GetLeaderboardAroundPlayerResult result)
    {
        userPosition[0] = result.Leaderboard[0].Position+1;//0始まりのため+1

        positionData0Event.Invoke();
        //Debug.Log("mode0 Position="+userPosition[0]);

    }
    private void OnGetLeaderboardAroundPlayerSuccessSingle2(GetLeaderboardAroundPlayerResult result)
    {
        userPosition[1] = result.Leaderboard[0].Position+1;//0始まりのため+1
        positionData1Event.Invoke();
        //Debug.Log("mode1 Position=" + userPosition[1]);
    }

    private void OnGetLeaderboardAroundPlayerFailureSingle(PlayFabError error)
    {
        //Debug.LogError($"ランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }

    public int GetPosition(int mode)
    {
       return userPosition[mode];
    }

}
