using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch_Reciever : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //ノーツ側から手に入場したかを確認する
        if (other.CompareTag(StatusHolder.Instance.HandTag))
        {
            transform.root.GetComponentInChildren<Punch_Item>().Interact(other.GetComponent<HandGuesser>().Hand);
        }
    }
}