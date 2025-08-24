using DG.Tweening;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VRollingCtrl : MonoBehaviour
{
    Sequence sequence;
    Tween rotateTween;
    public float[] targetAngles = new float[] { -30f, -15f, 0f, 15f, 30f }; // スナップ角度


    void Start()
    {
        sequence = DOTween.Sequence();

        sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 50), 1.5f));
        sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 0), 1.5f));
        sequence.AppendInterval(0.2f);
        sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, -50), 1.5f));
        sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 0), 1.5f));
        sequence.AppendInterval(0.2f);
        sequence.SetLoops(-1, LoopType.Restart);


    }

    // 回転開始（無限ループ）
    public void StartRotation()
    {
        // 途中の sequence を止める
        sequence.Kill();

        // 回転前に角度を一度リセット（localEulerAngles は 0〜360 の範囲）
        Vector3 current = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(current.x, current.y, 0f);

        // 無限回転を開始（Z軸）
        rotateTween = this.transform.DOLocalRotate(
            new Vector3(0f, 0f, 360f),
            0.2f,
            RotateMode.FastBeyond360
        )
        .SetLoops(-1, LoopType.Restart)
        .SetEase(Ease.Linear);
    }

    // 停止ボタンから呼ばれる

    public void StopRotationSmoothly()
    {
        if (rotateTween != null && rotateTween.IsActive())
        {
            rotateTween.Kill();
        }

        // 現在のZ軸角度（0〜360）を -180〜180 に正規化
        float currentZ = transform.localEulerAngles.z;
        if (currentZ > 180f) currentZ -= 360f;

        // 停止候補角度リスト（0度の確率を上げる）
        List<float> candidates = new List<float> { 0f, 0f, 0f, 15f, -15f, 45f, -45f }; // 0°を2回入れて50%確率

        // 最も近い角度を探す
        float targetZ = candidates.OrderBy(angle => Mathf.Abs(Mathf.DeltaAngle(currentZ, angle))).First();

        // 滑らかにその角度へ補正（例：0.5秒）
        transform.DOLocalRotate(
            new Vector3(0f, 0f, targetZ),
            0.5f
        ).SetEase(Ease.OutQuad);
    }

    // 角度を -180°〜180°に正規化
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    public void StopUp()
    {
        sequence.Kill();
        transform.DOLocalRotate(
        new Vector3(0f, 0f, 0f),
        0.5f
        ).SetEase(Ease.OutQuad);
    }

}