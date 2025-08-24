using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.Events;
using System.Text;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] UnityEvent loginSuccessEvent;


    //アカウントを作成するか
    private bool _shouldCreateAccount;

    //ログイン時に使うID
    private string _customID;


    //ログイン処理////////////////////////////////////////////////////

    //ランキングを表示するシーンに移行した時に、シーンのマネージャースクリプトから呼び出される
    public void Login()
    {
        _customID = LoadCustomID();
        var request = new LoginWithCustomIDRequest { CustomId = _customID, CreateAccount = _shouldCreateAccount };

        //ログインのID等を設定し、成功した時、失敗した時の処理の割り当てをしてログインを実行

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    //ログイン成功
    private void OnLoginSuccess(LoginResult result)
    {
        //アカウントを作成しようとしたのに、IDが既に使われていて、出来なかった場合
        if (_shouldCreateAccount && !result.NewlyCreated)
        {
            //Debug.LogWarning($"CustomId : {_customID} は既に使われています。");
            Login();//ログインしなおし
            return;
        }

        //アカウント作成時にIDを保存
        if (result.NewlyCreated)
        {
            SaveCustomID();

            PlayerPrefs.SetString("PlayFabID", result.PlayFabId);
        }
        //Debug.Log($"PlayFabのログインに成功\nPlayFabId : {result.PlayFabId}, CustomId : 　　{_customID}\nアカウントを作成したか : {result.NewlyCreated}");




        //ログインが成功したら、ランキングデータの取得に移るので、成功通知を発行

        loginSuccessEvent.Invoke();
    }

    //ログイン失敗
    private void OnLoginFailure(PlayFabError error)
    {
        //Debug.LogError($"PlayFabのログインに失敗\n{error.GenerateErrorReport()}");
    }


    //=================================================================================
    //カスタムIDの取得
    //=================================================================================

    //IDを保存する時のKEY
    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";

    //IDを取得
    private string LoadCustomID()
    {
        //IDを取得
        string id = PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY);

        //保存されていなければ新規生成
        _shouldCreateAccount = string.IsNullOrEmpty(id);
        return _shouldCreateAccount ? GenerateCustomID() : id;
    }

    //IDの保存
    private void SaveCustomID()
    {
        PlayerPrefs.SetString(CUSTOM_ID_SAVE_KEY, _customID);
    }

    //=================================================================================
    //カスタムIDの生成
    //=================================================================================

    //IDに使用する文字
    private static readonly string ID_CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyz";

    //IDを生成する
    private string GenerateCustomID()
    {
        int idLength = 32;//IDの長さ
        StringBuilder stringBuilder = new StringBuilder(idLength);
        var random = new System.Random();

        //ランダムにIDを生成
        for (int i = 0; i < idLength; i++)
        {
            stringBuilder.Append(ID_CHARACTERS[random.Next(ID_CHARACTERS.Length)]);
        }

        return stringBuilder.ToString();
    }

}
