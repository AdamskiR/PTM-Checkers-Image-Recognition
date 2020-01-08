using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple2DVisualizer : MonoBehaviour
{
    [SerializeField] GameObject[] whiteCheckers;
    [SerializeField] GameObject[] tiles;

    public void ShowGame()
    {
        int tilesNumber = FindObjectOfType<ReadCameraInput>().middlePoints.Count;
        if (tilesNumber > 32) tilesNumber = 32;

        int checkerNumber = FindObjectOfType<ReadWhiteColor>().middlePoints.Count;
        if (checkerNumber > 16) checkerNumber = 16;

        try
        {
            CleanScrean();
            FillScrean(tilesNumber,checkerNumber);
        }
        catch (Exception e)
        {
            Debug.Log("Nie wykryto pionkow/pol");
        }
        FindObjectOfType<TileTracker>().CalculateTiles();
    }

    private void CleanScrean()
    {
        for (int i = 0; i < 32; i++)
        {
            tiles[i].transform.position = new Vector3(2500, 200, 1);
        }

        for (int i = 0; i < 16; i++)
        {
            whiteCheckers[i].transform.position = new Vector3(2500, 200, 1);
        }
    }

    private void FillScrean(int tilesNumber, int checkerNumber)
    {
        for (int i = 0; i < tilesNumber; i++)
        {
            tiles[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadCameraInput>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadCameraInput>().middlePoints[i].y / 1.2f + 200, 1);
        }

        for (int i = 0; i < checkerNumber; i++)
        {
            whiteCheckers[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadWhiteColor>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadWhiteColor>().middlePoints[i].y / 1.2f + 200, 1);
        }
    }

}
