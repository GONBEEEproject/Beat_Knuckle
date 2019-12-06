using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cameras;
    private int target;


    private void Start()
    {
        UnityEngine.XR.XRSettings.showDeviceView = false;

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }

        cameras[0].SetActive(true);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            target++;

            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(false);
            }

            cameras[target % cameras.Length].SetActive(true);
        }
    }
}