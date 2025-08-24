using UnityEngine;
using TMPro; // TextMeshPro を使う場合
using System.Collections;

public class EffectTimer : MonoBehaviour
{
    [SerializeField] float effectDuration; // 効果の秒数（例: 10分10秒 = 610秒）
    [SerializeField] TMP_Text timerText; // UIにアタッチした TextMeshPro の参照
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
            labelText = "バネの効果 残り";

        }
        else if (itemName == "Medicine")
        {
            centerCtrl.GetComponent<CenterCtrl>().isItemMedicineUsed = true;
            labelText = "薬品の効果 残り";
        }

            float remainingTime = effectDuration;

        while (remainingTime > 0f)
        {
            // 分と秒に変換
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // UIに表示 (例: 10:05)
            timerText.text = labelText+string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        // 最後は00:00で終了表示


        if (itemName == "Spring")
        {
            timerText.text = "バネの効果 残り00:00";
            centerCtrl.GetComponent<CenterCtrl>().isItemSpringUsed = false;

        }
        else if (itemName == "Medicine")
        {
            timerText.text = "薬品の効果 残り00:00";
            centerCtrl.GetComponent<CenterCtrl>().isItemMedicineUsed = false;
        }
        isEffectActive = false;
        timerText.gameObject.SetActive(false);
    }
}
