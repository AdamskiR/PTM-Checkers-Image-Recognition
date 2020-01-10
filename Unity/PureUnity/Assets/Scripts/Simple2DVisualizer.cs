using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple2DVisualizer : MonoBehaviour
{
    [SerializeField] GameObject[] whiteCheckers;
    [SerializeField] GameObject[] blackCheckers;
    [SerializeField] GameObject[] tiles;

    public void ShowGame()
    {
        int tilesNumber = FindObjectOfType<ReadColorRed>().middlePoints.Count;
        if (tilesNumber > 32) tilesNumber = 32;

        int whiteCheckerNumber = FindObjectOfType<ReadColorWhite>().middlePoints.Count;
        if (whiteCheckerNumber > 16) whiteCheckerNumber = 16;

        int blackCheckerNumber = FindObjectOfType<ReadColorBlack>().middlePoints.Count;
        if (blackCheckerNumber > 16) blackCheckerNumber = 16;

        try
        {
            CleanScrean();
            FillScrean(tilesNumber, whiteCheckerNumber, blackCheckerNumber);
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

        for (int i = 0; i < 16; i++)
        {
            blackCheckers[i].transform.position = new Vector3(2500, 200, 1);
        }
    }

    private void FillScrean(int tilesNumber, int whiteCheckerNumber, int blackCheckerNumber)
    {
        for (int i = 0; i < tilesNumber; i++)
        {
            tiles[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadColorRed>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadColorRed>().middlePoints[i].y / 1.2f + 200, 1);
        }

        for (int i = 0; i < whiteCheckerNumber; i++)
        {
            whiteCheckers[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadColorWhite>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadColorWhite>().middlePoints[i].y / 1.2f + 200, 1);
        }

        for (int i = 0; i < blackCheckerNumber; i++)
        {
            blackCheckers[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadColorBlack>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadColorBlack>().middlePoints[i].y / 1.2f + 200, 1);
        }
    }

}
