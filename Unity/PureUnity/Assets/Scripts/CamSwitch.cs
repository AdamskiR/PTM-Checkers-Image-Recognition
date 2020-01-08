﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CamSwitch : MonoBehaviour
{
    public Camera[] cameras;
    private int currentCameraIndex;

    // Use this for initialization
    void Start()
    {
        currentCameraIndex = 0;
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].enabled = false;
        }
        if (cameras.Length > 0)
        {
            cameras[0].enabled = true;
            Debug.Log("Camera with name: " + cameras[0].GetComponent<Camera>().name + ", is now enabled");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCameraIndex++;
            Debug.Log("C button has been pressed. Switching to the next camera");
            if (currentCameraIndex < cameras.Length)
            {
                cameras[currentCameraIndex - 1].enabled=false;
                cameras[currentCameraIndex].enabled=true;
                Debug.Log("Camera with name: " + cameras[currentCameraIndex].GetComponent<Camera>().name + ", is now enabled");
            }
            else
            {
                cameras[currentCameraIndex - 1].enabled=false;
                currentCameraIndex = 0;
                cameras[currentCameraIndex].enabled = true ;
                Debug.Log("Camera with name: " + cameras[currentCameraIndex].GetComponent<Camera>().name + ", is now enabled");
            }
        }
    }
}