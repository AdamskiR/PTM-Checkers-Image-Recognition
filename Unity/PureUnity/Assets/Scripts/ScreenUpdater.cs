using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUpdater : MonoBehaviour
{
    [SerializeField] int captureRate;

    CameraInput photoTaker;

   // ReadColorRedinHSV red;
    ReadColorWhiteinHSV white;
    ReadColorBlackinHSV black;
    ReadColorYellowinHSV yellow; 

    Simple2DVisualizer visual;

    bool isReady = true;
    
    void Start()
    {
        photoTaker = FindObjectOfType<CameraInput>();
        visual = FindObjectOfType<Simple2DVisualizer>();

        //red = FindObjectOfType<ReadColorRedinHSV>();
        white = FindObjectOfType<ReadColorWhiteinHSV>();
        black = FindObjectOfType<ReadColorBlackinHSV>();
        yellow = FindObjectOfType<ReadColorYellowinHSV>();
    }

    void Update()
    {
        if (isReady)
        //if (Input.GetKeyDown(KeyCode.A))
        StartCoroutine(ProcessCameraImage());
    }

   IEnumerator ProcessCameraImage()
    {
        isReady = false;

        photoTaker.TakePhoto();

        //red.ReadColors();
        white.ReadColors();
        black.ReadColors();
        yellow.ReadColors();

        visual.ShowGame();
        yield return new WaitForSeconds(captureRate);
        isReady = true;
    }


}
