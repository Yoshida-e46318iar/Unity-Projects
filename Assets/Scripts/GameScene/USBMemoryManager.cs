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

            // �C�x���g�Ɋ֐���o�^
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
        //Debug.Log("����̍Đ����I�����܂���");
        videoPlayer.gameObject.SetActive(false);
        DisplayCanvas.SetActive(true);
        generalCanvas.SetActive(true);
        itemCanvas.SetActive(true);
        SoundManager.instance.FadeInBGM();

        ItemEffectStart();
    }

    void ItemEffectStart()
    {
        //�A�C�e���g�p���I����
        centerCtrl.GetComponent<CenterCtrl>().isItemUSBUsed = true;

        //�܋����P�O�O�O�{
        //�X�y�b�N�I�u�W�F�N�g�̂X�Ԃ̏܋����g�p����悤�ɁACenterCtrl���Œ���

        //�d�`���[���J�����ςȂ���
        denchyuCtrl.GetComponent<DenchuCtrl>().ItemEffectStart();

        //�H���̊J�����Ԃ�����
        centerCtrl.GetComponent<CenterCtrl>().isItemSpringUsed = true;

        //V�̉�]���~
        vRollingCtrl.GetComponent<VRollingCtrl>().StopUp();

        //�ʕ���
        centerCtrl.GetComponent<CenterCtrl>().isItemMedicineUsed = true;

        //���E���h��15R�݂̂�
        //CenterCtrl�̐}�����I��K��0��

    }

}
