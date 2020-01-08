using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMaster : MonoBehaviour
{
    [SerializeField] int captureRate;

    CameraInput photoTaker;
    ReadCameraInput board;
    ReadWhiteColor whiteCheckers;
    ReadYellowColor markerTracker;
    Simple2DVisualizer visual;

    bool isReady = true;
    
    void Start()
    {
        photoTaker = FindObjectOfType<CameraInput>();
        board = FindObjectOfType<ReadCameraInput>();
        whiteCheckers = FindObjectOfType<ReadWhiteColor>();
        visual = FindObjectOfType<Simple2DVisualizer>();
        markerTracker = FindObjectOfType<ReadYellowColor>();
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

        board.ReadColors();
        whiteCheckers.ReadColors();
        markerTracker.ReadColors();

        visual.ShowGame();
        yield return new WaitForSeconds(captureRate);
        isReady = true;
    }


}
