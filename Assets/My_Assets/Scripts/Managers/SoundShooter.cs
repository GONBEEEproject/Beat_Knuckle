using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundShooter : MonoBehaviour
{
    private static SoundShooter instance;
    public static SoundShooter Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SoundShooter>();
            return instance;
        }
    }

    [SerializeField]
    private AudioClip _jab, _punch, _lightClap, _heavyClap, _gameStart;

    private OVRHapticsClip _jabClip, _punchClip, _lightClip, _heavyClip, _startClip;

    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();

        _jabClip = new OVRHapticsClip(_jab);
        _punchClip = new OVRHapticsClip(_punch);
        _lightClip = new OVRHapticsClip(_lightClap);
        _heavyClip = new OVRHapticsClip(_heavyClap);
        _startClip = new OVRHapticsClip(_gameStart);        
    }

    /// <summary>
    /// 軽い打撃音
    /// </summary>
    public void PlayJab()
    {
        _source.PlayOneShot(_jab);
        OVRHaptics.RightChannel.Mix(_jabClip);
    }

    /// <summary>
    /// 重めの打撃
    /// </summary>
    public void PlayPunch(HandGuesser.HandType hand)
    {
        switch (hand)
        {
            case HandGuesser.HandType.Left:
                _source.PlayOneShot(_punch);
                OVRHaptics.LeftChannel.Mix(_punchClip);
                break;
            case HandGuesser.HandType.Right:
                _source.PlayOneShot(_jab);
                OVRHaptics.RightChannel.Mix(_jabClip);
                break;
        }
    }

    /// <summary>
    /// 手同士を打ち合わせた時の音
    /// 拳で抵抗する21歳想定
    /// </summary>
    /// <param name="isPower">音の強さ</param>
    public void PlayClap(bool isPower = false)
    {
        if (isPower)
        {
            _source.PlayOneShot(_heavyClap);
            OVRHaptics.RightChannel.Mix(_heavyClip);
            OVRHaptics.LeftChannel.Mix(_heavyClip);
        }
        else
        {
            _source.PlayOneShot(_lightClap);
            OVRHaptics.RightChannel.Mix(_lightClip);
            OVRHaptics.LeftChannel.Mix(_lightClip);
        }
    }

    /// <summary>
    /// ゲーム開始時の音
    /// </summary>
    public void GameStart()
    {
        _source.PlayOneShot(_gameStart);
        OVRHaptics.RightChannel.Mix(_startClip);
        OVRHaptics.LeftChannel.Mix(_startClip);
    }
}
