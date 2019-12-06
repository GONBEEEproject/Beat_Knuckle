using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private static ScoreCalculator instance;
    public static ScoreCalculator Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<ScoreCalculator>();
            return instance;
        }
    }

    private int _combo;


    /// <summary>
    /// 触ったとき、コンボ増加
    /// </summary>
    /// <param name="hand"></param>
    public void Punch(HandGuesser.HandType hand)
    {
        _combo++;
    }

    /// <summary>
    /// ミスしたとき、コンボリセット
    /// </summary>
    public void Missed()
    {
        _combo = 0;
    }
}
