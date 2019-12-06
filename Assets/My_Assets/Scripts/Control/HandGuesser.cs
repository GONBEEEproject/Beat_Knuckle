using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGuesser : MonoBehaviour
{
    public enum HandType
    {
        Left, Right
    }

    [SerializeField]
    private HandType _hand;
    public HandType Hand { get { return _hand; } }
}