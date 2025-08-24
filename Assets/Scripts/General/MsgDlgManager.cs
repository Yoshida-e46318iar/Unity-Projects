using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MsgDlgManager : MonoBehaviour
{
    [SerializeField] TMP_Text msgText;

    //呼び出し元からボタンを押したときの処理を割り付けられるように、Unity Actionを使用

    public UnityAction OkAction { get; set; }
    public UnityAction CancelAction { get; set; }


    //ダイアログ生成用の関数

    //引数

    //一つ目: ダイアログに表示するテキスト

    //二つ目: OKボタンが押された時の呼び出し元の処理

    //三つ目: Cancelボタンが押された時の呼び出し元の処理

    public void DlgShow(string msg, UnityAction onOK, UnityAction onCancel)
    {

        //デフォルトでは非アクティブにしているのでアクティブ化
        this.gameObject.SetActive(true);
        msgText.text = msg;

        //Unity Actionの割り付け

        OkAction = onOK;
        CancelAction = onCancel;

    }

    public void OnOK()
    {
        SoundManager.instance.PlayButtonSEOK();
        //割り付けられてる処理を呼び出す　割り付けられていない場合を考慮して?演算子を使用
        OkAction?.Invoke();
        this.gameObject.SetActive(false);
    }
    public void OnCancel()
    {
        SoundManager.instance.PlayButtonSECancel();
        CancelAction?.Invoke();
        this.gameObject.SetActive(false);
    }
}
