using UnityEngine;
using TMPro; // TextMeshPro ���g���ꍇ
using System.Collections;

public class EffectTimer : MonoBehaviour
{
    [SerializeField] float effectDuration; // ���ʂ̕b���i��: 10��10�b = 610�b�j
    [SerializeField] TMP_Text timerText; // UI�ɃA�^�b�`���� TextMeshPro �̎Q��
    [SerializeField] GameObject centerCtrl;
    [SerializeField] string itemName;
    private bool isEffectActive = false;

    public void StartEffect()
    {
        if (!isEffectActive)
        {
            timerText.gameObject.SetActive(true);
            StartCoroutine(EffectCoroutine());
        }
    }

    private IEnumerator EffectCoroutine()
    {
        isEffectActive = true;
        string labelText = "";

        if (itemName == "Spring")
        {
            centerCtrl.GetComponent<CenterCtrl>().isItemSpringUsed = true;
            labelText = "�o�l�̌��� �c��";

        }
        else if (itemName == "Medicine")
        {
            centerCtrl.GetComponent<CenterCtrl>().isItemMedicineUsed = true;
            labelText = "��i�̌��� �c��";
        }

            float remainingTime = effectDuration;

        while (remainingTime > 0f)
        {
            // ���ƕb�ɕϊ�
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // UI�ɕ\�� (��: 10:05)
            timerText.text = labelText+string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // �Ō��00:00�ŏI���\��


        if (itemName == "Spring")
        {
            timerText.text = "�o�l�̌��� �c��00:00";
            centerCtrl.GetComponent<CenterCtrl>().isItemSpringUsed = false;

        }
        else if (itemName == "Medicine")
        {
            timerText.text = "��i�̌��� �c��00:00";
            centerCtrl.GetComponent<CenterCtrl>().isItemMedicineUsed = false;
        }
        isEffectActive = false;
        timerText.gameObject.SetActive(false);
    }
}
