using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StatusHolder : MonoBehaviour
{
    private static StatusHolder instance;
    public static StatusHolder Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<StatusHolder>();
            return instance;
        }
    }

    /* タグや共通で使うEasingカーブなど
     * 一箇所でステータス管理するクラス
     */

    [Header("Tag")]
    [SerializeField]
    private string _noteTag;
    public string NoteTag { get { return _noteTag; } }

    [SerializeField]
    private string _handTag;
    public string HandTag { get { return _handTag; } }

    [Header("Ease")]
    [SerializeField]
    private Ease _moveEase;
    public Ease MoveEase { get { return _moveEase; } }

    [SerializeField]
    private Ease _scaleEase;
    public Ease ScaleEase { get { return _scaleEase; } }

    [Header("Clap")]
    [SerializeField]
    private float _clapSpeed;
    public float ClapSpeed { get { return _clapSpeed; } }

    [Header("Debug")]
    [SerializeField]
    private bool _isAuto;
    public bool IsAuto { get { return _isAuto; } }
}