using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRunner : MonoBehaviour
{
    public GameObject[] punch;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int r = Random.Range(0, 2);

            GameObject g = Instantiate(punch[r]);
            g.GetComponent<Punch_Item>().Initialize(TimeController.Instance.Offset, r);
        }
    }
}
