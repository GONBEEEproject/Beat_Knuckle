using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChecker : MonoBehaviour
{
    private Vector3 _lastPos;
    public float Speed { get; private set; }

    private void LateUpdate()
    {
        Speed = ((transform.position - _lastPos) / Time.deltaTime).sqrMagnitude;
        _lastPos = transform.position;
    }
}