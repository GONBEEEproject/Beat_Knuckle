using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Punch_Item : ItemBehaviour
{
    //殴られる対象の手
    [SerializeField]
    private HandGuesser.HandType _targetHand;

    /// <summary>
    /// ノーツの初期化とアニメーション
    /// </summary>
    /// <param name="_offset">生成からジャストまでの時間</param>
    /// <param name="_lane">どのレーンに流すか</param>
    public void Initialize(float _offset, int _lane)
    {
        //Runnerから送られてきたデータを保持しておく
        Offset = _offset;
        LaneNum = _lane;

        /* アニメーション表現
         * 初期化からジャストまでの座標をOffsetジャストでDOPathする
         * その後消滅するまでの距離を計算してもう一度DOPathする 
         * その分の時間はマーカーとOffsetから算出
         */
        Vector3[] path = MarkerHolder.Instance.GetPath(LaneNum);
        Vector3[] afterPath = MarkerHolder.Instance.GetAfterPath();
        float afterOffset = MarkerHolder.Instance.GetAfterOffset(LaneNum);

        //ふわっと生成するためにPrefab状態でのスケールを保持しておく
        var seq = DOTween.Sequence();
        var scale = transform.localScale;

        //ノーツを生成場所に転送とスケールを0にして消しておく
        transform.position = MarkerHolder.Instance.GetStartPos(LaneNum);
        transform.localScale = Vector3.zero;

        //スケールを戻して出現
        seq.Join(
            transform.DOScale(
                scale,
                Offset * 0.1f)
                    .SetEase(StatusHolder.Instance.ScaleEase));

        //出現と同時に移動を開始
        seq.Join(
            transform.DOLocalPath(
                path,
                Offset,
                PathType.CubicBezier)
                    .SetLookAt(0.05f, Vector3.forward)
                    .SetEase(StatusHolder.Instance.MoveEase));
        
        //ジャストまで移動した後少しオーバーランさせる
        seq.Append(
            transform.DOLocalPath(
                afterPath,
                afterOffset,
                PathType.CubicBezier)
                    .SetLookAt(0.05f, Vector3.forward)
                    .SetEase(Ease.Linear));

        //オーバーランまで完了したのに生きてる場合はミス判定
        seq.OnComplete(() =>
        {
            OverRun();
        });

        //シークエンスを再生
        seq.Play();
    }

    private void Update()
    {
        //生成してからの経過時間を保持しておく
        ScoreTime += Time.deltaTime;

        //デバッグ用のオートプレイオプション、Offset分経過したら触ったことにする
        if (ScoreTime >= Offset && StatusHolder.Instance.IsAuto)
        {
            Interact(_targetHand);
        }
    }

    /// <summary>
    /// ミス時の処理
    /// </summary>
    private void OverRun()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 触ったとき
    /// </summary>
    /// <param name="hand"></param>
    public void Interact(HandGuesser.HandType hand)
    {
        //触った手とこのアイテムのターゲット手が一致したときだけちゃんと通過
        if (hand == _targetHand)
        {
            ScoreCalculator.Instance.Punch(hand);
            EffectRunner.Instance.Punch(hand);
            SoundShooter.Instance.PlayPunch(hand);
        }
        else
        {
            SoundShooter.Instance.PlayJab();
        }

        Destroy(gameObject);
    }
}