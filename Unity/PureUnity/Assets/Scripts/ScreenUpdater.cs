using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUpdater : MonoBehaviour
{
    [SerializeField] int captureRate;

    CameraInput photoTaker;

    ReadColorRedinHSV red;
    ReadColorWhiteinHSV white;
    ReadColorBlackinHSV black;
    ReadColorYellowinHSV yellow;
    ReadColorGreeninHSV green;
    Simple2DVisualizer visual;

    bool isReady = true;
    
    void Start()
    {
        photoTaker = FindObjectOfType<CameraInput>();
        visual = FindObjectOfType<Simple2DVisualizer>();
        green = FindObjectOfType<ReadColorGreeninHSV>();
        red = FindObjectOfType<ReadColorRedinHSV>();
        white = FindObjectOfType<ReadColorWhiteinHSV>();
        black = FindObjectOfType<ReadColorBlackinHSV>();
        yellow = FindObjectOfType<ReadColorYellowinHSV>();
    }

    void Update()
    {
        if (isReady && FindObjectOfType<GameMaster>().IsGameOn())
        //if (Input.GetKeyDown(KeyCode.A))
        StartCoroutine(ProcessCameraImage());
    }

    public void ReadCamera()
    {
        photoTaker.TakePhoto();

        red.ReadColors();
        green.ReadColors();
        white.ReadColors();
        black.ReadColors();
        yellow.ReadColors();

        visual.ShowGame();
    }

   IEnumerator ProcessCameraImage()
    {
        isReady = false;

        photoTaker.TakePhoto();

        //red.ReadColors();
        white.ReadColors();
        black.ReadColors();
        yellow.ReadColors();

        visual.ShowCheckers();
        yield return new WaitForSeconds(captureRate);
        isReady = true;
    }


}
