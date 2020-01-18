using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckersDetection : MonoBehaviour
{
    [Header("Data")]
    public Vector3[] whiteCheckers;
    public Vector3[] blackCheckers;
    public Vector3[] kings;

    public List<TilesDetails> whiteTiles;
    public List<TilesDetails> blackTiles;
    public List<TilesDetails> kingsTiles;
    [SerializeField] TilesDetails[] tiles;

    [Header("3D Objects")]
    [SerializeField] GameObject[] whiteRepresentation3D = new GameObject[20];
    [SerializeField] GameObject[] blackRepresentation3D = new GameObject[20];
    [SerializeField] GameObject[] blackKingsRepresentation3D = new GameObject[12];
    [SerializeField] GameObject[] whiteKingsRepresentation3D = new GameObject[12];
    [SerializeField] GameObject[] TileRepresentation3D;


    [SerializeField] int desirableTilesNumber = 32;
    [SerializeField] int errorMargin = 30;

    private int whiteCheckerNumber;
    private int blackCheckerNumber;
    private int kingsNumber;

    public void CalculateTiles()
    {
        SetNumberOfObjects();

        tiles = FindObjectOfType<BoardDetector>().AllTiles();

        whiteCheckers = new Vector3[whiteCheckerNumber];
        blackCheckers = new Vector3[blackCheckerNumber];
        kings = new Vector3[kingsNumber];
        

        try
        {
            if (FindObjectOfType<GameMaster>().CheckMovement())
            {
                HideAll3DChecker();
                GetObjectsData();
                ClearTilesOccupation();
                PutCheckersOnTiles();
            }
            else
            {
                GetObjectsData();
                ClearTilesOccupation();
                DontPutCheckersOnTiles();
            }
            
            

        }
        catch (Exception e)
        {
            Debug.Log("Niepoprawnie zczytano pozycje pionkow");
        }

    }

    public void ThrowCheckersOnBoard()
    {
        try
        {
                HideAll3DChecker();
                GetObjectsData();
                ClearTilesOccupation();
                PutCheckersOnTiles();
            
        }
        catch (Exception e)
        {
            Debug.Log("Niepoprawnie zczytano pozycje pionkow");
        }
    }


    public TilesDetails[] AllTiles()
    {
        return tiles;
    }

    private void ClearTilesOccupation()
    {
        try
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].occupiedBlack = false;
                tiles[i].occupiedWhite = false;
                tiles[i].king = false;            }
        }
        catch (Exception e)
        {
            Debug.Log("Problem z czyszczeniem");
        }
    }


    private void DontPutCheckersOnTiles()
    {
        try
        {
            whiteTiles.Clear();
            blackTiles.Clear();
            kingsTiles.Clear();
            TileRepresentation3D = FindObjectOfType<BoardDetector>().AllTileRepresentation3D();
            for (int i = 0; i < whiteCheckers.Length; i++)
            {
                int index = -1;
                float minDistance = 500;
                for (int j = 0; j < tiles.Length; j++)
                {
                    float distanceTileChecker = Vector3.Distance(whiteCheckers[i], tiles[j].tilePosition);
                    if (Math.Abs(distanceTileChecker) < minDistance && Math.Abs(distanceTileChecker) < errorMargin)
                    {
                        minDistance = Math.Abs(distanceTileChecker);
                        index = j;
                    }

                }
                if (index > -1)
                {
                    
                    tiles[index].occupiedWhite = true;
                    whiteTiles.Add(tiles[index]);
                }
            }

            for (int i = 0; i < blackCheckers.Length; i++)
            {
                int index = -1;
                float minDistance = 500;
                for (int j = 0; j < tiles.Length; j++)
                {
                    float distanceTileChecker = Vector3.Distance(blackCheckers[i], tiles[j].tilePosition);
                    if (Math.Abs(distanceTileChecker) < minDistance && Math.Abs(distanceTileChecker) < errorMargin)
                    {
                        minDistance = Math.Abs(distanceTileChecker);
                        index = j;
                    }

                }
                if (index > -1)
                {
                    
                    tiles[index].occupiedBlack = true;
                    blackTiles.Add(tiles[index]);
                }

            }

            for (int i = 0; i < kings.Length; i++)
            {
                int index = -1;
                float minDistance = 500;
                for (int j = 0; j < tiles.Length; j++)
                {
                    float distanceTileChecker = Vector3.Distance(kings[i], tiles[j].tilePosition);
                    if (Math.Abs(distanceTileChecker) < minDistance && Math.Abs(distanceTileChecker) < errorMargin)
                    {
                        minDistance = Math.Abs(distanceTileChecker);
                        index = j;
                    }

                }
                if (index > -1)
                {

                    if (tiles[index].occupiedWhite)
                    {
                        kingsTiles.Add(tiles[index]);
                        tiles[index].king = true;
                        
                    }

                    if (tiles[index].occupiedBlack)
                    {
                        kingsTiles.Add(tiles[index]);
                        tiles[index].king = true;
                    }

                }

            }

        }
        catch (Exception e)
        {
            Debug.Log("Problem ze spawnowaniem pionkow");
        }

    }


    private void PutCheckersOnTiles()
    {
        try
        { 
        whiteTiles.Clear();
        blackTiles.Clear();
        kingsTiles.Clear();
        TileRepresentation3D = FindObjectOfType<BoardDetector>().AllTileRepresentation3D();
        for (int i = 0; i < whiteCheckers.Length; i++)
        {
            int index = -1;
            float minDistance = 500;
            for (int j = 0; j < tiles.Length; j++)
            {
                float distanceTileChecker = Vector3.Distance(whiteCheckers[i], tiles[j].tilePosition);
                if (Math.Abs(distanceTileChecker) < minDistance && Math.Abs(distanceTileChecker) < errorMargin)
                {
                    minDistance = Math.Abs(distanceTileChecker);
                    index = j;
                }

            }
            if (index > -1)
            {
                whiteRepresentation3D[i].transform.position = new Vector3(TileRepresentation3D[index].transform.position.x,
                TileRepresentation3D[index].transform.position.y + 20, TileRepresentation3D[index].transform.position.z);
                tiles[index].occupiedWhite = true;
                whiteTiles.Add(tiles[index]);
            }
        }

        for (int i = 0; i < blackCheckers.Length; i++)
        {
            int index = -1;
            float minDistance = 500;
            for (int j = 0; j < tiles.Length; j++)
            {
                float distanceTileChecker = Vector3.Distance(blackCheckers[i], tiles[j].tilePosition);
                if (Math.Abs(distanceTileChecker) < minDistance && Math.Abs(distanceTileChecker) < errorMargin)
                {
                    minDistance = Math.Abs(distanceTileChecker);
                    index = j;
                }

            }
            if (index > -1)
            {
                blackRepresentation3D[i].transform.position = new Vector3(TileRepresentation3D[index].transform.position.x,
                TileRepresentation3D[index].transform.position.y + 20, TileRepresentation3D[index].transform.position.z);
                tiles[index].occupiedBlack = true;
                blackTiles.Add(tiles[index]);
            }

        }

            for (int i = 0; i < kings.Length; i++)
            {
                int index = -1;
                float minDistance = 500;
                for (int j = 0; j < tiles.Length; j++)
                {
                    float distanceTileChecker = Vector3.Distance(kings[i], tiles[j].tilePosition);
                    if (Math.Abs(distanceTileChecker) < minDistance && Math.Abs(distanceTileChecker) < errorMargin)
                    {
                        minDistance = Math.Abs(distanceTileChecker);
                        index = j;
                    }

                }
                if (index > -1)
                {
                    
                    if (tiles[index].occupiedWhite)
                    {
                        kingsTiles.Add(tiles[index]);
                        tiles[index].king = true;
                        whiteKingsRepresentation3D[i].transform.position = new Vector3(TileRepresentation3D[index].transform.position.x,
                        TileRepresentation3D[index].transform.position.y + 20, TileRepresentation3D[index].transform.position.z);
                    }

                    if (tiles[index].occupiedBlack)
                    {
                        kingsTiles.Add(tiles[index]);
                        tiles[index].king = true;
                        blackKingsRepresentation3D[i].transform.position = new Vector3(TileRepresentation3D[index].transform.position.x,
                        TileRepresentation3D[index].transform.position.y + 20, TileRepresentation3D[index].transform.position.z);
                    }

                }

            }

        }
        catch (Exception e)
        {
            Debug.Log("Problem ze spawnowaniem pionkow");
        }

    }


    private void GetObjectsData()
    {
        try
        {

            for (int i = 0; i < whiteCheckerNumber; i++)
            {
                whiteCheckers[i] = new Vector3(FindObjectOfType<ReadColorWhiteinHSV>().middlePoints[i].x, FindObjectOfType<ReadColorWhiteinHSV>().middlePoints[i].y, 1);
            }
            for (int i = 0; i < blackCheckerNumber; i++)
            {
                blackCheckers[i] = new Vector3(FindObjectOfType<ReadColorBlackinHSV>().middlePoints[i].x, FindObjectOfType<ReadColorBlackinHSV>().middlePoints[i].y, 1);
            }

            for (int i = 0; i < kingsNumber; i++)
            {
                kings[i] = new Vector3(FindObjectOfType<ReadColorYellowinHSV>().middlePoints[i].x, FindObjectOfType<ReadColorYellowinHSV>().middlePoints[i].y, 1);
            }

        }
        catch (Exception e)
        {
            Debug.Log("Nie odczytano koordynatnow pionkow");
        }
    }

    private void HideAll3DChecker()
    {
        for (int i = 0; i < whiteRepresentation3D.Length; i++)
        {
            whiteRepresentation3D[i].transform.position = new Vector3(0, -120, 0);
        }

        for (int i = 0; i < blackRepresentation3D.Length; i++)
        {
            blackRepresentation3D[i].transform.position = new Vector3(0, -120, 0);
        }

        for (int i = 0; i < blackKingsRepresentation3D.Length; i++)
        {
            blackKingsRepresentation3D[i].transform.position = new Vector3(0, -120, 0);
        }

        for (int i = 0; i < whiteKingsRepresentation3D.Length; i++)
        {
            whiteKingsRepresentation3D[i].transform.position = new Vector3(0, -120, 0);
        }

    }

    private void SetNumberOfObjects()
    {
        whiteCheckerNumber = FindObjectOfType<ReadColorWhiteinHSV>().middlePoints.Count;
        if (whiteCheckerNumber > 16) whiteCheckerNumber = 16;

        blackCheckerNumber = FindObjectOfType<ReadColorBlackinHSV>().middlePoints.Count;
        if (blackCheckerNumber > 16) blackCheckerNumber = 16;

        kingsNumber = FindObjectOfType<ReadColorYellowinHSV>().middlePoints.Count;
        if (kingsNumber > 16) kingsNumber = 16;
    }


}
