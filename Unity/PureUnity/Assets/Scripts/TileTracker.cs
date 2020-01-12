using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileTracker : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] Vector3 marker;
    [SerializeField] Vector3[] whiteCheckers;
    [SerializeField] Vector3[] blackCheckers;
    [SerializeField] TilesDetails[] tiles;

    [Header("3D Objects")]
    [SerializeField] GameObject[] TileRepresentation3D = new GameObject[8];
    [SerializeField] GameObject[] whiteRepresentation3D = new GameObject[6];
    [SerializeField] GameObject[] blackRepresentation3D = new GameObject[6];

    [SerializeField] int TileNumber = 32;
    [SerializeField] int markerTileA1Distance = 60;
    private int tilesNumber;
    private int whiteCheckerNumber;
    private int blackCheckerNumber;

    public void CalculateTiles()
    {
        SetNumberOfObjects();

        whiteCheckers = new Vector3[whiteCheckerNumber];
        blackCheckers = new Vector3[blackCheckerNumber];
        tiles = new TilesDetails[tilesNumber];

        if (tiles.Length == TileNumber)
        {
            HideAll3DChecker();
            try
            {
                GetObjectsData();
                //SortTilesByDistanceToMarker();

                SortTilesByCoordinates();
                Link3DObjectsWithCameraInput();
                PutCheckersOnTiles();
            }
            catch (Exception e)
            {
                Debug.Log("Nie ukonczono wizualizacji 3D");
            }
        }

    }

    private void PutCheckersOnTiles()
    {
        for (int i = 0; i < whiteCheckers.Length; i++)
        {
            int index = -1;
            float minDistance = 500;
            for (int j = 0; j < tiles.Length; j++)
            {
                float distanceTileChecker = Vector3.Distance(whiteCheckers[i], tiles[j].tilePosition);
                if (Math.Abs(distanceTileChecker) < minDistance)
                {
                    minDistance = Math.Abs(distanceTileChecker);
                    index = j;
                }

            }
            if (index > -1)
            {
                whiteRepresentation3D[i].transform.position = new Vector3(TileRepresentation3D[index].transform.position.x,
                TileRepresentation3D[index].transform.position.y + 30, TileRepresentation3D[index].transform.position.z);
                Debug.Log("kurczak na polu: " + index);
            }

            else
            {
                Debug.Log("kaczka poza polem");
            }
        }

        for (int i = 0; i < blackCheckers.Length; i++)
        {
            int index = -1;
            float minDistance = 500;
            for (int j = 0; j < tiles.Length; j++)
            {
                float distChecker = Vector3.Distance(blackCheckers[i], marker);
                float distTile = Vector3.Distance(tiles[j].tilePosition, marker);
                if (Math.Abs(distChecker - distTile) < minDistance)
                {
                    minDistance = Math.Abs(distChecker - distTile);
                    index = j;
                }

            }
            if (index > -1)
            {
                blackRepresentation3D[i].transform.position = new Vector3(TileRepresentation3D[index].transform.position.x,
                TileRepresentation3D[index].transform.position.y + 30, TileRepresentation3D[index].transform.position.z);
                Debug.Log("kurczak na polu: " + index);
            }

            else
            {
                Debug.Log("kaczka poza polem");
            }
        }

    }

    private void Link3DObjectsWithCameraInput()
    {
        for (int i = 0; i < tilesNumber; i++)
        {
            tiles[i].TileRepresentation3D = TileRepresentation3D[i];
            tiles[i].tileName = TileRepresentation3D[i].GetComponent<TileName>().tileName;
        }
    }

    private void SortTilesByDistanceToMarker()
    {
        tiles = tiles.OrderBy(x => x.distanceToMarker).ToArray();

        float distTile1 = Vector3.Distance(tiles[0].tilePosition, tiles[1].tilePosition);
        float distTile2 = Vector3.Distance(tiles[0].tilePosition, tiles[2].tilePosition);

        float distTile4 = Vector3.Distance(tiles[7].tilePosition, tiles[4].tilePosition);
        float distTile5 = Vector3.Distance(tiles[7].tilePosition, tiles[5].tilePosition);

        if (distTile1 > distTile2)
        {
            TilesDetails pom = tiles[2];
            tiles[2] = tiles[1];
            tiles[1] = pom;
        }

        if (distTile4 > distTile5)
        {
            TilesDetails pom = tiles[5];
            tiles[5] = tiles[4];
            tiles[4] = pom;
        }
    }

    private void PrintTilesArray()
    {
        foreach(var t  in tiles )
        {
            Debug.Log(t.tilePosition.x + " "+t.tilePosition.y);
        }
        Debug.Log("end");
    }

    

    private void SortTilesByCoordinates()
    {
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

    }

    private void GetObjectsData()
    {
        try
        {
        if (FindObjectOfType<ReadColorGreeninHSV>().middlePoints.Count > 0)
        marker = new Vector3(FindObjectOfType<ReadColorGreeninHSV>().middlePoints[0].x, FindObjectOfType<ReadColorGreeninHSV>().middlePoints[0].y, 1);

        for (int i = 0; i<tilesNumber; i++)
            {
            tiles[i] = new TilesDetails(new Vector3(FindObjectOfType<ReadColorRedinHSV>().middlePoints[i].x, FindObjectOfType<ReadColorRedinHSV>().middlePoints[i].y, 1), marker);
            }

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
                  Debug.Log("Nie odczytano koordynatnow planszy");
        }
    }

    private void HideAll3DChecker()
    {
        for (int i = 0; i < whiteRepresentation3D.Length; i++)
        {
            whiteRepresentation3D[i].transform.position = new Vector3(0, -60, 0);
        }

        for (int i = 0; i < blackRepresentation3D.Length; i++)
        {
            blackRepresentation3D[i].transform.position = new Vector3(0, -60, 0);
        }

    }

    private void SetNumberOfObjects()
    {
        tilesNumber = FindObjectOfType<ReadColorRedinHSV>().middlePoints.Count;
        if (tilesNumber > 8) tilesNumber = 8;

        whiteCheckerNumber = FindObjectOfType<ReadColorWhiteinHSV>().middlePoints.Count;
        if (whiteCheckerNumber > 16) whiteCheckerNumber = 16;

        blackCheckerNumber = FindObjectOfType<ReadColorBlackinHSV>().middlePoints.Count;
        if (blackCheckerNumber > 16) blackCheckerNumber = 16;
    }

}
