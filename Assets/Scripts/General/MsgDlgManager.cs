using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MsgDlgManager : MonoBehaviour
{
    [SerializeField] TMP_Text msgText;

    //�Ăяo��������{�^�����������Ƃ��̏���������t������悤�ɁAUnity Action���g�p

    public UnityAction OkAction { get; set; }
    public UnityAction CancelAction { get; set; }


    //�_�C�A���O�����p�̊֐�

    //����

    //���: �_�C�A���O�ɕ\������e�L�X�g

    //���: OK�{�^���������ꂽ���̌Ăяo�����̏���

    //�O��: Cancel�{�^���������ꂽ���̌Ăяo�����̏���

    public void DlgShow(string msg, UnityAction onOK, UnityAction onCancel)
    {

        //�f�t�H���g�ł͔�A�N�e�B�u�ɂ��Ă���̂ŃA�N�e�B�u��
        this.gameObject.SetActive(true);
        msgText.text = msg;

        //Unity Action�̊���t��

        OkAction = onOK;
        CancelAction = onCancel;

    }

    public void OnOK()
    {
        SoundManager.instance.PlayButtonSEOK();
        //����t�����Ă鏈�����Ăяo���@����t�����Ă��Ȃ��ꍇ���l������?���Z�q���g�p
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
