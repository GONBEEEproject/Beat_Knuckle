using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static TimeController instance;
    public static TimeController Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<TimeController>();
            return instance;
        }
    }

    /* ノーツの飛ぶ時間など
     * 音ゲーのプレイ感触に関する時間系を
     * 一箇所で管理する用のクラス
     */

    [SerializeField]
    private float _offset;
    public float Offset { get { return _offset; } }
}