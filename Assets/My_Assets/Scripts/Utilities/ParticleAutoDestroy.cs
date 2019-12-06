using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyParticle());
    }

    IEnumerator DestroyParticle()
    {
        var particle = GetComponent<ParticleSystem>();

        yield return new WaitWhile(() => particle.IsAlive(true));

        Destroy(gameObject);
    }
}