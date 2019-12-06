using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーン別にマーカーを保持するクラス
/// </summary>
[System.Serializable]
public class LanePath
{
    public Transform Start, StartOut, EndIn, End;
}

public class MarkerHolder : MonoBehaviour
{
    private static MarkerHolder instance;
    public static MarkerHolder Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<MarkerHolder>();
            return instance;
        }
    }

    //ノーツが飛ぶレーン情報、DOPath用
    [SerializeField]
    private LanePath[] _lanes;

    //手元通過後消滅までのレーン情報
    [SerializeField]
    private Transform _endOut, _afterIn, _after;

    /// <summary>
    /// ノーツの初期位置を取得する
    /// </summary>
    /// <param name="lane">レーン番号を渡す</param>
    /// <returns></returns>
    public Vector3 GetStartPos(int lane)
    {
        return _lanes[lane].Start.position;
    }

    /// <summary>
    /// DOPath用のベジエパスを取得する
    /// </summary>
    /// <param name="lane">レーン番号を渡す</param>
    /// <returns></returns>
    public Vector3[] GetPath(int lane)
    {
        var path = new Vector3[]
        {
            _lanes[lane].End.position,
            _lanes[lane].StartOut.position,
            _lanes[lane].EndIn.position
        };

        return path;
    }

    /// <summary>
    /// 手元まで飛んだ後の移動用のDOPathベジエパスを取得
    /// </summary>
    /// <returns></returns>
    public Vector3[] GetAfterPath()
    {
        var path = new Vector3[]
        {
            _after.position,
            _endOut.position,
            _afterIn.position
        };

        return path;
    }

    /// <summary>
    /// ノーツが等速で移動するように座標から移動時間を取得
    /// </summary>
    /// <param name="lane">レーン番号取得</param>
    /// <returns></returns>
    public float GetAfterOffset(int lane)
    {
        //ノーツスタート地点からゴール地点
        var dist = (_lanes[lane].Start.position - _lanes[lane].End.position).sqrMagnitude;

        //ノーツゴール地点から消滅地点
        var afterDist = (_lanes[lane].End.position - _after.position).sqrMagnitude;

        //ゴールから消滅までにかける時間
        var offset = (float)System.Math.Pow(TimeController.Instance.Offset, 2) * (afterDist / dist);

        return offset;
    }
}
