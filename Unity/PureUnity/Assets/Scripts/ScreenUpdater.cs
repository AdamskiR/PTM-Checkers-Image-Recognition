using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUpdater : MonoBehaviour
{
    [SerializeField] int captureRate;

    CameraInput photoTaker;

    ReadColorRed red;
    ReadColorWhite white;
    ReadColorBlack black;
    ReadColorGreen green; 

    Simple2DVisualizer visual;

    bool isReady = true;
    
    void Start()
    {
        photoTaker = FindObjectOfType<CameraInput>();
        visual = FindObjectOfType<Simple2DVisualizer>();

        red = FindObjectOfType<ReadColorRed>();
        white = FindObjectOfType<ReadColorWhite>();
        black = FindObjectOfType<ReadColorBlack>();
        green = FindObjectOfType<ReadColorGreen>();
    }

    void Update()
    {
        if (isReady)
        StartCoroutine(ProcessCameraImage());
    }

   IEnumerator ProcessCameraImage()
    {
        isReady = false;

        photoTaker.TakePhoto();

        red.ReadColors();
        white.ReadColors();
        black.ReadColors();
        green.ReadColors();

        visual.ShowGame();
        yield return new WaitForSeconds(captureRate);
        isReady = true;
    }


}
