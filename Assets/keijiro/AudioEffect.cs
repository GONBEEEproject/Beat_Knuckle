using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : MonoBehaviour
{
    public AudioSpectrum spectrum;
    public Transform[] effects;
    public float scale;

    void Update()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            var effect = effects[i];
            var local = effect.localScale;
            local.y = spectrum.Levels[i] * scale;
            effect.localScale = local;
        }
    }
}
