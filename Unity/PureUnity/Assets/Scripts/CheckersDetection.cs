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
    [SerializeField] TilesDetails[] tiles;
    public List <TilesDetails> whiteTiles;
    public List <TilesDetails> blackTiles;

    [Header("3D Objects")]
    [SerializeField] GameObject[] whiteRepresentation3D = new GameObject[20];
    [SerializeField] GameObject[] blackRepresentation3D = new GameObject[20];
    [SerializeField] GameObject[] TileRepresentation3D;


    [SerializeField] int desirableTilesNumber = 32;
    [SerializeField] int errorMargin = 30;

    private int whiteCheckerNumber;
    private int blackCheckerNumber;

   public  void CalculateTiles()
    {
        SetNumberOfObjects();

        tiles = FindObjectOfType<BoardDetector>().AllTiles();

            whiteCheckers = new Vector3[whiteCheckerNumber];
            blackCheckers = new Vector3[blackCheckerNumber];
            HideAll3DChecker();

            //try
            {
                GetObjectsData();
                ClearTilesOccupation();

            //SortTilesByCoordinates();
            //Link3DObjectsWithCameraInput();
            
            PutCheckersOnTiles();
            FindObjectOfType<TimeMaster>().CheckMovement();

        }
             //catch (Exception e)
            {
                //Debug.Log("Niepoprawnie zczytano pozycje pionkow");
            }

    }


    public TilesDetails[] AllTiles ()
    {
        return tiles;
    }

    private void ClearTilesOccupation()
    {
        for (int i = 0; i<tiles.Length;i++)
        {
           tiles[i].occupiedBlack = false;
            tiles[i].occupiedWhite = false;
        }
    }

    private void PutCheckersOnTiles()
    {
        whiteTiles.Clear();
        blackTiles.Clear();
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
                TileRepresentation3D[index].transform.position.y + 30, TileRepresentation3D[index].transform.position.z);
                tiles[index].occupiedWhite = true;
                whiteTiles.Add(tiles[index]);
                //Debug.Log("kurczak na polu: " + tiles[index].tileName);
            }

            else
            {
                //Debug.Log("kaczka poza polem");
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
                TileRepresentation3D[index].transform.position.y + 30, TileRepresentation3D[index].transform.position.z);
                tiles[index].occupiedBlack = true;
                blackTiles.Add(tiles[index]);
               // Debug.Log("kurczak na polu: " + index);
            }

            else
            {
               // Debug.Log("kaczka poza polem");
            }
        }

    }

    private void Link3DObjectsWithCameraInput()
    {
        /*for (int i = 0; i < tilesNumber; i++)
        {
            tiles[i].TileRepresentation3D = TileRepresentation3D[i];
            tiles[i].tileName = TileRepresentation3D[i].GetComponent<TileName>().tileName;
        }*/
    }


    private void SortTilesByCoordinates()
    {/*
        tiles = tiles.OrderBy(x => x.distanceToMarker).ToArray();
        float cornerRU = tiles[0].tilePosition.y - marker.y;
        float cornerLD = marker.y - tiles[0].tilePosition.y;
        float cornerLU = marker.x - tiles[0].tilePosition.x;
        float cornerRD = tiles[0].tilePosition.x - marker.x;
        Debug.Log("RU: " + cornerRU +", RD"+cornerRD + " ,LD" + cornerLD+ " ,LU" + cornerLU);

        if (cornerRU > markerTileA1Distance)
        {

            tiles = tiles.OrderBy(x => x.tilePosition.y).ToArray();
            double numberOfTilesInRowD = Math.Sqrt(tilesNumber * 2) / 2;
            int numberOfTilesInRow = (int)Math.Round(numberOfTilesInRowD);
            for (int i = 0; i < numberOfTilesInRow * 2; i++)
            {
                TilesDetails[] tilesInRow = new TilesDetails[numberOfTilesInRow];
                for (int j = 0; j < numberOfTilesInRow; j++)
                {
                    tilesInRow[j] = tiles[i * numberOfTilesInRow + j];
                }
                tilesInRow = tilesInRow.OrderBy(x => x.tilePosition.x).ToArray();
                for (int j = 0; j < numberOfTilesInRow; j++)
                {
                    tiles[i * numberOfTilesInRow + j] = tilesInRow[j];
                }
            }

        }

        if (cornerRD > markerTileA1Distance)
        {
            tiles = tiles.OrderBy(x => x.tilePosition.x).ToArray();
            double numberOfTilesInRowD = Math.Sqrt(tilesNumber * 2) / 2;
            int numberOfTilesInRow = (int)Math.Round(numberOfTilesInRowD);
            for (int i = 0; i < numberOfTilesInRow * 2; i++)
            {
                TilesDetails[] tilesInRow = new TilesDetails[numberOfTilesInRow];
                for (int j = 0; j < numberOfTilesInRow; j++)
                {
                    tilesInRow[j] = tiles[i * numberOfTilesInRow + j];
                }
                tilesInRow = tilesInRow.OrderByDescending(x => x.tilePosition.y).ToArray();
                for (int j = 0; j < numberOfTilesInRow; j++)
                {
                    tiles[i * numberOfTilesInRow + j] = tilesInRow[j];
                }
            }
        }

        if (cornerLU > markerTileA1Distance)
        {
            tiles = tiles.OrderByDescending(x => x.tilePosition.x).ToArray();
            double numberOfTilesInRowD = Math.Sqrt(tilesNumber * 2) / 2;
            int numberOfTilesInRow = (int)Math.Round(numberOfTilesInRowD);
            for (int i = 0; i < numberOfTilesInRow * 2; i++)
            {
                TilesDetails[] tilesInRow = new TilesDetails[numberOfTilesInRow];
                for (int j = 0; j < numberOfTilesInRow; j++)
                {
                    tilesInRow[j] = tiles[i * numberOfTilesInRow + j];
                }
                tilesInRow = tilesInRow.OrderBy(x => x.tilePosition.y).ToArray();
                for (int j = 0; j < numberOfTilesInRow; j++)
                {
                    tiles[i * numberOfTilesInRow + j] = tilesInRow[j];
                }
            }
        }

        if (cornerLD > markerTileA1Distance)
        {
            tiles = tiles.OrderByDescending(x => x.tilePosition.y).ToArray();
            double numberOfTilesInRowD = Math.Sqrt(tilesNumber * 2) / 2;
            int numberOfTilesInRow = (int)Math.Round(numberOfTilesInRowD);
            for (int i = 0; i < numberOfTilesInRow * 2; i++)
            {
                TilesDetails[] tilesInRow = new TilesDetails[numberOfTilesInRow];
                for (int j = 0; j < numberOfTilesInRow; j++)
                {
                    tilesInRow[j] = tiles[i * numberOfTilesInRow + j];
                }
                tilesInRow = tilesInRow.OrderByDescending(x => x.tilePosition.x).ToArray();
                for (int j = 0; j < numberOfTilesInRow; j++)
                {
                    tiles[i * numberOfTilesInRow + j] = tilesInRow[j];
                }
            }
        }
        */
    }

    private void GetObjectsData()
    {
        try
        {

        for (int i = 0; i<whiteCheckerNumber; i++)
            {
                whiteCheckers[i] = new Vector3(FindObjectOfType<ReadColorWhiteinHSV>().middlePoints[i].x, FindObjectOfType<ReadColorWhiteinHSV>().middlePoints[i].y, 1);
            }
        for (int i = 0; i < blackCheckerNumber; i++)
            {
                blackCheckers[i] = new Vector3(FindObjectOfType<ReadColorBlackinHSV>().middlePoints[i].x, FindObjectOfType<ReadColorBlackinHSV>().middlePoints[i].y, 1);
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

    }

    private void SetNumberOfObjects()
    {
        whiteCheckerNumber = FindObjectOfType<ReadColorWhiteinHSV>().middlePoints.Count;
        if (whiteCheckerNumber > 16) whiteCheckerNumber = 16;

        blackCheckerNumber = FindObjectOfType<ReadColorBlackinHSV>().middlePoints.Count;
        if (blackCheckerNumber > 16) blackCheckerNumber = 16;
    }

   
}
