using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple2DVisualizer : MonoBehaviour
{
    [SerializeField] GameObject[] whiteCheckers;
    [SerializeField] GameObject[] blackCheckers;
    [SerializeField] GameObject[] tiles;
    [SerializeField] GameObject[] marker;

    public void ShowGame()
    {
        int tilesNumber = FindObjectOfType<ReadColorRedinHSV>().middlePoints.Count;
        if (tilesNumber > 42) tilesNumber = 42;

        int whiteCheckerNumber = FindObjectOfType<ReadColorWhiteinHSV>().middlePoints.Count;
        if (whiteCheckerNumber > 16) whiteCheckerNumber = 16;

        int blackCheckerNumber = FindObjectOfType<ReadColorBlackinHSV>().middlePoints.Count;
        if (blackCheckerNumber > 16) blackCheckerNumber = 16;

        int greenCheckerNumber = FindObjectOfType<ReadColorGreeninHSV>().middlePoints.Count;
        if (greenCheckerNumber > 16) greenCheckerNumber = 16;

        try
        {
            CleanScrean();
            FillScrean(tilesNumber, whiteCheckerNumber, blackCheckerNumber, greenCheckerNumber);
        }
        catch (Exception e)
        {
            Debug.Log("Nie wykryto pionkow/pol");
        }
        FindObjectOfType<CheckersDetection>().CalculateTiles();
    }

    public void ShowTiles()
    {
        int tilesNumber = FindObjectOfType<ReadColorRedinHSV>().middlePoints.Count;
        if (tilesNumber > 42) tilesNumber = 42;

        int greenCheckerNumber = FindObjectOfType<ReadColorGreeninHSV>().middlePoints.Count;
        if (greenCheckerNumber > 16) greenCheckerNumber = 16;

        try
        {
            CleanTiles();
            FillScrean(tilesNumber, greenCheckerNumber);
        }
        catch (Exception e)
        {
            Debug.Log("Nie wykryto pol");
        }
       // FindObjectOfType<CheckersDetection>().CalculateTiles();
    }

    private void CleanTiles()
    {
        for (int i = 0; i < 42; i++)
        {
            tiles[i].transform.position = new Vector3(2500, 200, 1);
        }

        for (int i = 0; i < 16; i++)
        {
            marker[i].transform.position = new Vector3(2500, 200, 1);
        }
    }

    private void CleanScrean()
    {
        for (int i = 0; i < 42; i++)
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

        for (int i = 0; i < 16; i++)
        {
            marker[i].transform.position = new Vector3(2500, 200, 1);
        }
    }

    private void FillScrean(int tilesNumber,  int greenCheckerNumber)
    {
        for (int i = 0; i < tilesNumber; i++)
        {
            tiles[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadColorRedinHSV>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadColorRedinHSV>().middlePoints[i].y / 1.2f + 200, 1);
        }

        for (int i = 0; i < greenCheckerNumber; i++)
        {
            marker[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadColorGreeninHSV>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadColorGreeninHSV>().middlePoints[i].y / 1.2f + 200, 1);
        }
    }

    private void FillScrean(int tilesNumber, int whiteCheckerNumber, int blackCheckerNumber, int greenCheckerNumber)
    {
        for (int i = 0; i < tilesNumber; i++)
        {
            tiles[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadColorRedinHSV>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadColorRedinHSV>().middlePoints[i].y / 1.2f + 200, 1);
        }

        for (int i = 0; i < whiteCheckerNumber; i++)
        {
            whiteCheckers[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadColorWhiteinHSV>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadColorWhiteinHSV>().middlePoints[i].y / 1.2f + 200, 1);
        }

        for (int i = 0; i < blackCheckerNumber; i++)
        {
            blackCheckers[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadColorBlackinHSV>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadColorBlackinHSV>().middlePoints[i].y / 1.2f + 200, 1);
        }

        for (int i = 0; i < greenCheckerNumber; i++)
        {
            marker[i].transform.position = new Vector3(transform.position.x - FindObjectOfType<ReadColorGreeninHSV>().middlePoints[i].x / 1.2f + 200, transform.position.y - FindObjectOfType<ReadColorGreeninHSV>().middlePoints[i].y / 1.2f + 200, 1);
        }
    }

}
