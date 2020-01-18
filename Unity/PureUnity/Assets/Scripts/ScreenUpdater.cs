using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUpdater : MonoBehaviour
{
    [SerializeField] float captureRate;
    [SerializeField] TextMeshProUGUI FPSnumber;

    CameraInput photoTaker;

    ReadColorRedinHSV red;
    ReadColorWhiteinHSV white;
    ReadColorBlackinHSV black;
    ReadColorYellowinHSV yellow;
    ReadColorGreeninHSV green;
    Simple2DVisualizer visual;


    public Slider sliderFPS1;
    public Slider sliderFPS2;


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
        FPSnumber.text = "0.25"; 
    }

    void Update()
    {
        if (isReady && FindObjectOfType<GameMaster>().BoardDetected())
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


    public void SubmitSlider1Setting()
    {
        captureRate = sliderFPS1.value;
        float fps = 1f / captureRate;
        FPSnumber.text = fps.ToString();
    }

    public void SubmitSlider2Setting()
    {
        captureRate = sliderFPS2.value;
        float fps = 1f / captureRate;
        FPSnumber.text = fps.ToString();
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
