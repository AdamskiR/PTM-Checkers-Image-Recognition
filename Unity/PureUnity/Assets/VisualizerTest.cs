using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerTest : MonoBehaviour
{
    [SerializeField] GameObject[] whiteCheckers;
    [SerializeField] GameObject[] tiles;

    void Start()
    {
        
    }

    // Update is called once per frame
    public void ShowGame()
    {
        int tilesNumber = FindObjectOfType<ReadCameraInput>().middlePoints.Count;
        if (tilesNumber > 32) tilesNumber = 32;

        int checkerNumber = FindObjectOfType<ReadCameraInput>().middlePoints.Count;
        if (checkerNumber > 16) checkerNumber = 16;

        try {
            for (int i = 0; i < 32; i++)
            {
                tiles[i].transform.position = new Vector3(2500, 200, 1);
            }

            for (int i = 0; i < 16; i++)
            {
                whiteCheckers[i].transform.position = new Vector3(2500, 200, 1);
            }

            for (int i = 0; i < tilesNumber; i++)
                {
                    tiles[i].transform.position = new Vector3(FindObjectOfType<ReadCameraInput>().middlePoints[i].x, FindObjectOfType<ReadCameraInput>().middlePoints[i].y, 1);
                }

                for (int i = 0; i < checkerNumber; i++)
                {
                    whiteCheckers[i].transform.position = new Vector3(FindObjectOfType<ReadWhiteColor>().middlePoints[i].x, FindObjectOfType<ReadWhiteColor>().middlePoints[i].y, 1);
                }

            }
            catch (Exception e)
            {
                Debug.Log("Za malo pol/pionkow");
            }
    }
    
}
