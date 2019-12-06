using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartReciever : MonoBehaviour
{
    [SerializeField]
    private Transform _flyTarget;

    private void OnTriggerEnter(Collider other)
    {
        //ミット側から侵入したかどうか確認
        if (other.CompareTag(StatusHolder.Instance.HandTag))
        {

            //ゲームを開始しつつ
            //多重起動を防ぐため判定をころす
            Runner.Instance.GameStart();
            transform.root.GetComponentInChildren<Collider>().enabled = false;
            transform.root.DOMove(_flyTarget.position, 1.0f).SetEase(Ease.InCubic);
            transform.root.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                Destroy(transform.root.gameObject);
            });
        }
    }
}
