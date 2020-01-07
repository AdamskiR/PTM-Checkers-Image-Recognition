using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMaster : MonoBehaviour
{
    [SerializeField] int captureRate;

    DisplayCameraOnObject photoTaker;
    ReadCameraInput board;
    ReadWhiteColor whiteCheckers;
    ReadYellowColor markerTracker;
    VisualizerTest visual;
    bool isReady = true;
    
    void Start()
    {
        photoTaker = FindObjectOfType<DisplayCameraOnObject>();
        board = FindObjectOfType<ReadCameraInput>();
        whiteCheckers = FindObjectOfType<ReadWhiteColor>();
        visual = FindObjectOfType<VisualizerTest>();
        markerTracker = FindObjectOfType<ReadYellowColor>();
    }

    // Update is called once per frame
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
