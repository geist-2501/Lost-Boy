using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Camera targetCam;


    private void Start()
    {
        targetCam = Camera.main;

    }

    private void LateUpdate()
    {
        if (targetCam)
        {
            transform.forward = targetCam.transform.forward;
        }

    }

}
