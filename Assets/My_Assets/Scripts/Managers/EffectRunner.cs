using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

/// <summary>
/// ノリノリにさせるオブジェクトを設定別に保持するクラス
/// </summary>
[System.Serializable]
public class RhythmObject
{
    [SerializeField]
    private Transform[] _beatObjects;

    [SerializeField]
    private float _BPM;

    [SerializeField]
    private bool _isBackBeat;

    [SerializeField]
    private Vector3 _punch;

    [SerializeField]
    private Ease _punchEase;

    /// <summary>
    /// 楽曲再生開始時に発火、もちもちさせ始める
    /// </summary>
    public IEnumerator BeatStart()
    {
        //Second per beat
        var SPB = 60 / _BPM;

        //裏打ちの場合1ビートの半分待つ
        var delay = 0f;
        if (_isBackBeat)
        {
            delay = SPB / 2;
        }
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < _beatObjects.Length; i++)
        {
            //スピーディなもちもち感を出すため、アニメーションする時間はSPBの40%
            //残りは待機する
            Sequence sequence = DOTween.Sequence()
                .AppendInterval(SPB * 0.3f)
                .Append(_beatObjects[i].DOPunchScale(_punch, SPB * 0.4f, 1, 0).SetEase(_punchEase))
                .AppendInterval(SPB * 0.3f);

            sequence.Play().SetLoops(-1);
        }
    }
}

public class EffectRunner : MonoBehaviour
{
    private static EffectRunner instance;
    public static EffectRunner Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<EffectRunner>();
            return instance;
        }
    }

    [SerializeField]
    private RhythmObject[] _rhythmEffects;

    [Header("JabEffect")]
    [SerializeField]
    private GameObject _jabEffect;
    [SerializeField]
    private Transform[] _jabWay;
    [SerializeField]
    private float _jabTime;
    [SerializeField]
    private Ease _jabEase;

    [Header("StraightEffect")]
    [SerializeField]
    private GameObject _straightBeam;
    [SerializeField]
    private Transform _beamOrigin;

    /// <summary>
    /// ゲーム開始時にエフェクト系をもちもちさせる
    /// </summary>
    public void GameStart()
    {
        for(int i = 0; i < _rhythmEffects.Length; i++)
        {
            StartCoroutine(_rhythmEffects[i].BeatStart());
        }
    }

    /// <summary>
    /// パンチしたときの処理を両手で分離
    /// </summary>
    /// <param name="hand"></param>
    public void Punch(HandGuesser.HandType hand)
    {
        switch (hand)
        {
            case HandGuesser.HandType.Right:
                Jab();
                break;
            case HandGuesser.HandType.Left:
                Straight();
                break;
        }
    }

    /// <summary>
    /// 右手パンチ時
    /// </summary>
    private void Jab()
    {
        //手元からパワーオーブに飛ばす
        var g = Instantiate(_jabEffect);
        g.transform.position = _jabWay[0].position;

        List<GameObject> particle = new List<GameObject>();

        //Transform[]からVector3[]に変換する
        //アズマゴロー先生@azumagoroありがとう！
        Vector3[] path = _jabWay.Select(x => x.position).ToArray();

        g.transform.DOLocalPath(path, _jabTime, PathType.CatmullRom)
            .SetEase(_jabEase)
            .OnComplete(() =>
            {
                //Particleを持ってるオブジェクトだけを遅延させて削除する
                foreach(Transform child in g.transform)
                {
                    if (child.GetComponent<ParticleSystem>() == null)
                    {
                        Destroy(child.gameObject);   
                    }
                }

                Destroy(g, 1.5f);
            });
    }

    /// <summary>
    /// 左手パンチ時
    /// </summary>
    private void Straight()
    {
        //オーブからビームを飛ばす
        var g = Instantiate(_straightBeam, _beamOrigin.position, _beamOrigin.rotation);
    }
}