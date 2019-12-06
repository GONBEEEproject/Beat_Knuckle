using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUpdate : MonoBehaviour
{
    [SerializeField]
    private Transform target, move;

    private void Update()
    {
        //moveの座標を常時targetで上書きするやつ
        //子オブジェクトにできないときに使う
        move.position = target.position;
    }
}
