using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ノーツアイテム用のベースクラス、何かと便利なので分離させておくといいかもしれない
/// </summary>
public class ItemBehaviour : MonoBehaviour
{
    /* 外部からアクセスするためにpublicだが
     * インスペクタから設定するわけではないので隠す
     */
    [HideInInspector]
    public float Offset;
    [HideInInspector]
    public int LaneNum;
    [HideInInspector]
    public float ScoreTime;
}