using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clap : MonoBehaviour
{

    /* どちらか片方の手に貼るコンポーネント
     * 反対側の手を保持、こっち側の手は自分でGetComponentする
     */
    [SerializeField]
    private SpeedChecker _oppositeHand;

    private SpeedChecker _myHand;

    private void Start()
    {
        _myHand = GetComponent<SpeedChecker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //入場したものが手の時だけ発火
        if (other.CompareTag(StatusHolder.Instance.HandTag))
        {
            var speed = _oppositeHand.Speed + _myHand.Speed;

            //持っているスピードがsqrMagnitudeなので
            //StatusHolderからスピードを取ってきて2乗してから比較
            if (speed>=Mathf.Pow(StatusHolder.Instance.ClapSpeed,2))
            {
                SoundShooter.Instance.PlayClap(true);
            }
            else
            {
                SoundShooter.Instance.PlayClap();
            }
        }       
    }
}
