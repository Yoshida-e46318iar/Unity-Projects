using DG.Tweening;
using UnityEngine;

public class ItemHummerManager : MonoBehaviour
{
    [SerializeField] GameObject[] kugiObjs;
    [SerializeField] float[] kugiAngles;
    [SerializeField] Camera[] cameras;


    Quaternion[] defAngles;
    void Start()
    {
        defAngles=new Quaternion[kugiObjs.Length];
        for (int i = 0; i < kugiObjs.Length; i++)
        {
            defAngles[i] = kugiObjs[i].transform.rotation;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            KugiOpenAnimation();

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            for (int i = 0; i < kugiObjs.Length; i++)
                RotateKugiDef(kugiObjs[i], defAngles[i]);
        }
    }

    public void KugiOpenAnimation()
    {
        cameras[0].gameObject.SetActive(true);
        cameras[1].gameObject.SetActive(false);


        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(1f)
           .AppendCallback(() => StartKugiOpen())
           .AppendInterval(2.5f)
           .AppendCallback(() => {
               cameras[0].gameObject.SetActive(false);
               cameras[1].gameObject.SetActive(true);

           })
           .AppendInterval(1f)
           .AppendCallback(() => {

                HaneKugiOpen();
            })
           .AppendInterval(2.5f) // •K—v‚É‰ž‚¶‚Ä‘Ò‚¿ŽžŠÔ‚ð’Ç‰Á
           .AppendCallback(() => cameras[1].gameObject.SetActive(false));

    }

    void StartKugiOpen()
    {
        SoundManager.instance.PlaySound(0, 15);
        for (int i = 0; i < 5; i++)
            RotateKugi(kugiObjs[i], kugiAngles[i]);

    }
    void HaneKugiOpen()
    {
        SoundManager.instance.PlaySound(0, 15);
        for (int i = 5; i < kugiObjs.Length; i++)
            RotateKugi(kugiObjs[i], kugiAngles[i]);

    }


    public void RotateKugi(GameObject kugiObj, float angle)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.up) * transform.localRotation;

        kugiObj.transform.DOLocalRotateQuaternion(targetRotation, 0.8f).SetEase(Ease.OutQuad);

    }

    public void RotateKugiDef(GameObject kugiObj, Quaternion defq)
    {
        kugiObj.transform.rotation= defq;
    }

 
}
