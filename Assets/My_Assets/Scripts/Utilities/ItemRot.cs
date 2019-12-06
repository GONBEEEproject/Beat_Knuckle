using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ItemRot : MonoBehaviour
{
    [SerializeField]
    private Vector3 worldRot, localRot;

    [SerializeField]
    private float sec = 1f;

    private void Start()
    {
        worldRot /= sec;
        localRot /= sec;
    }

    private void Update()
    {
        transform.Rotate(worldRot * Time.deltaTime, Space.World);
        transform.Rotate(localRot * Time.deltaTime, Space.Self);
    }
}