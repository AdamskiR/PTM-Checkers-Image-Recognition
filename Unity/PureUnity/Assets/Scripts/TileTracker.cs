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
    [SerializeField] GameObject[] TileRepresentation3D;
    [SerializeField] GameObject[] whiteRepresentation3D = new GameObject[20];
    [SerializeField] GameObject[] blackRepresentation3D = new GameObject[20];

    [SerializeField] int tileNumber = 32;
    [SerializeField] int markerTileA1Distance = 60;
    [SerializeField] int errorMargin = 50;
    private int tilesNumber;
    private int whiteCheckerNumber;
    private int blackCheckerNumber;

    public void CalculateTiles()
    {
        SetNumberOfObjects();

        whiteCheckers = new Vector3[whiteCheckerNumber];
        blackCheckers = new Vector3[blackCheckerNumber];
        tiles = new TilesDetails[tilesNumber];

        if (tiles.Length == tileNumber)
        {
            HideAll3DChecker();
            
            //try
            {
                
                GetObjectsData();
                ClearTilesOccupation();
                //SortTilesByDistanceToMarker();

                SortTilesByCoordinates();
                Link3DObjectsWithCameraInput();
                PutCheckersOnTiles();
                FillCaptureTiles();
                //FillNeighbours();
            }
           // catch (Exception e)
            {
                Debug.Log("Nie ukonczono wizualizacji 3D");
            }
        }

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
        if (tilesNumber > 40) tilesNumber = 40;

        whiteCheckerNumber = FindObjectOfType<ReadColorWhiteinHSV>().middlePoints.Count;
        if (whiteCheckerNumber > 16) whiteCheckerNumber = 16;

        blackCheckerNumber = FindObjectOfType<ReadColorBlackinHSV>().middlePoints.Count;
        if (blackCheckerNumber > 16) blackCheckerNumber = 16;
    }

    private TilesDetails FindTile(string name)
    {
        foreach(var t in tiles)
        {
            if (t.tileName == name) return t;
        }
        return null;
    }

    ////legit

    private void FillCaptureTiles()
    {
        TilesDetails[] neighbours;

        foreach (var t in tiles)
        {
            switch (t.tileName)
            {
                case "A1":
                    neighbours = new TilesDetails[1];
                    neighbours[0] = FindTile("C3");
                    t.captureTiles = neighbours;
                    break;

                case "C1":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("C3");
                    neighbours[1] = FindTile("E3");
                    t.captureTiles = neighbours;
                    break;

                case "E1":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("C3");
                    neighbours[1] = FindTile("G3");
                    t.captureTiles = neighbours;
                    break;

                case "G1":
                    neighbours = new TilesDetails[1];
                    neighbours[0] = FindTile("E3");
                    t.captureTiles = neighbours;
                    break;

                case "B2":
                    neighbours = new TilesDetails[1];
                    neighbours[0] = FindTile("D4");
                    t.captureTiles = neighbours;
                    break;

                case "D2":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("B4");
                    neighbours[1] = FindTile("F4");
                    t.captureTiles = neighbours;
                    break;

                case "F2":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("D4");
                    neighbours[1] = FindTile("H4");
                    t.captureTiles = neighbours;
                    break;

                case "H2":
                    neighbours = new TilesDetails[1];
                    neighbours[0] = FindTile("F4");
                    t.captureTiles = neighbours;
                    break;

                case "A3":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("C1");
                    neighbours[1] = FindTile("C5");
                    t.captureTiles = neighbours;
                    break;

                case "C3":
                    neighbours = new TilesDetails[4];
                    neighbours[0] = FindTile("A1");
                    neighbours[1] = FindTile("E1");
                    neighbours[2] = FindTile("A5");
                    neighbours[3] = FindTile("E5");
                    t.captureTiles = neighbours;
                    break;

                case "E3":
                    neighbours = new TilesDetails[4];
                    neighbours[0] = FindTile("C1");
                    neighbours[1] = FindTile("G1");
                    neighbours[2] = FindTile("C5");
                    neighbours[3] = FindTile("G5");
                    t.captureTiles = neighbours;
                    break;

                case "G3":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("E1");
                    neighbours[1] = FindTile("E5");
                    t.captureTiles = neighbours;
                    break;

                case "B4":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("D2");
                    neighbours[1] = FindTile("D6");
                    t.captureTiles = neighbours;
                    break;

                case "D4":
                    neighbours = new TilesDetails[4];
                    neighbours[0] = FindTile("B2");
                    neighbours[1] = FindTile("F2");
                    neighbours[2] = FindTile("B6");
                    neighbours[3] = FindTile("F6");
                    t.captureTiles = neighbours;
                    break;

                case "F4":
                    neighbours = new TilesDetails[4];
                    neighbours[0] = FindTile("D2");
                    neighbours[1] = FindTile("H2");
                    neighbours[2] = FindTile("D6");
                    neighbours[3] = FindTile("H6");
                    t.captureTiles = neighbours;
                    break;

                case "H4":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("F2");
                    neighbours[1] = FindTile("F6");
                    t.captureTiles = neighbours;
                    break;

                case "A5":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("C3");
                    neighbours[1] = FindTile("C7");
                    t.captureTiles = neighbours;
                    break;

                case "C5":
                    neighbours = new TilesDetails[4];
                    neighbours[0] = FindTile("A3");
                    neighbours[1] = FindTile("E3");
                    neighbours[2] = FindTile("A7");
                    neighbours[3] = FindTile("E7");
                    t.captureTiles = neighbours;
                    break;

                case "E5":
                    neighbours = new TilesDetails[4];
                    neighbours[0] = FindTile("C3");
                    neighbours[1] = FindTile("G3");
                    neighbours[2] = FindTile("C7");
                    neighbours[3] = FindTile("G7");
                    t.captureTiles = neighbours;
                    break;

                case "G5":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("E3");
                    neighbours[1] = FindTile("E7");
                    t.captureTiles = neighbours;
                    break;

                case "B6":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("D4");
                    neighbours[1] = FindTile("D8");
                    t.captureTiles = neighbours;
                    break;

                case "D6":
                    neighbours = new TilesDetails[4];
                    neighbours[0] = FindTile("B4");
                    neighbours[1] = FindTile("F4");
                    neighbours[2] = FindTile("B8");
                    neighbours[3] = FindTile("F8");
                    t.captureTiles = neighbours;
                    break;

                case "F6":
                    neighbours = new TilesDetails[4];
                    neighbours[0] = FindTile("D4");
                    neighbours[1] = FindTile("H4");
                    neighbours[2] = FindTile("D8");
                    neighbours[3] = FindTile("H8");
                    t.captureTiles = neighbours;
                    break;

                case "H6":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("F4");
                    neighbours[1] = FindTile("F8");
                    t.captureTiles = neighbours;
                    break;

                case "A7":
                    neighbours = new TilesDetails[1];
                    neighbours[0] = FindTile("C5");
                    t.captureTiles = neighbours;
                    break;

                case "C7":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("A5");
                    neighbours[1] = FindTile("E5");
                    t.captureTiles = neighbours;
                    break;

                case "E7":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("C5");
                    neighbours[1] = FindTile("G5");
                    t.captureTiles = neighbours;
                    break;

                case "G7":
                    neighbours = new TilesDetails[1];
                    neighbours[0] = FindTile("E5");
                    t.captureTiles = neighbours;
                    break;

                case "B8":
                    neighbours = new TilesDetails[1];
                    neighbours[0] = FindTile("D6");
                    t.captureTiles = neighbours;
                    break;

                case "D8":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("B6");
                    neighbours[1] = FindTile("F6");
                    t.captureTiles = neighbours;
                    break;

                case "F8":
                    neighbours = new TilesDetails[2];
                    neighbours[0] = FindTile("D6");
                    neighbours[1] = FindTile("H8");
                    t.captureTiles = neighbours;
                    break;

                case "H8":
                    neighbours = new TilesDetails[1];
                    neighbours[0] = FindTile("F6");
                    t.captureTiles = neighbours;
                    break;

            }
        }
    }

       private  void FillNeighbours()
       {
            TilesDetails[] neighbours;

            foreach (var t in tiles)
            {
                switch (t.tileName)
                {
                    case "A1":
                        neighbours = new TilesDetails[1];
                        neighbours[0].tileName = "B2";
                        t.neighborTiles = neighbours;
                        break;

                    case "C1":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "B2";
                        neighbours[1].tileName = "D2";
                        t.neighborTiles = neighbours;
                        break;

                    case "E1":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "D2";
                        neighbours[1].tileName = "F2";
                        t.neighborTiles = neighbours;
                        break;

                    case "G1":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "F2";
                        neighbours[1].tileName = "H2";
                        t.neighborTiles = neighbours;
                        break;

                    case "B2":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "A1";
                        neighbours[1].tileName = "C1";
                        neighbours[2].tileName = "A3";
                        neighbours[3].tileName = "C3";
                        t.neighborTiles = neighbours;
                        break;

                    case "D2":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "C1";
                        neighbours[1].tileName = "E1";
                        neighbours[2].tileName = "C3";
                        neighbours[3].tileName = "E3";
                        t.neighborTiles = neighbours;
                        break;

                    case "F2":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "E1";
                        neighbours[1].tileName = "G1";
                        neighbours[2].tileName = "E3";
                        neighbours[3].tileName = "G3";
                        t.neighborTiles = neighbours;
                        break;

                    case "H2":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "G1";
                        neighbours[1].tileName = "G3";
                        t.neighborTiles = neighbours;
                        break;

                    case "A3":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "B2";
                        neighbours[1].tileName = "B4";
                        t.neighborTiles = neighbours;
                        break;

                    case "C3":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "B2";
                        neighbours[1].tileName = "D2";
                        neighbours[2].tileName = "B4";
                        neighbours[3].tileName = "D4";
                        t.neighborTiles = neighbours;
                        break;

                    case "E3":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "D2";
                        neighbours[1].tileName = "F2";
                        neighbours[2].tileName = "F4";
                        neighbours[3].tileName = "D4";
                        t.neighborTiles = neighbours;
                        break;

                    case "G3":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "F2";
                        neighbours[1].tileName = "H2";
                        neighbours[2].tileName = "F4";
                        neighbours[3].tileName = "H4";
                        t.neighborTiles = neighbours;
                        break;

                    case "B4":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "A3";
                        neighbours[1].tileName = "C3";
                        neighbours[2].tileName = "A5";
                        neighbours[3].tileName = "C5";
                        t.neighborTiles = neighbours;
                        break;

                    case "D4":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "C3";
                        neighbours[1].tileName = "E3";
                        neighbours[2].tileName = "C5";
                        neighbours[3].tileName = "E5";
                        t.neighborTiles = neighbours;
                        break;

                    case "F4":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "E3";
                        neighbours[1].tileName = "G3";
                        neighbours[2].tileName = "E5";
                        neighbours[3].tileName = "G5";
                        t.neighborTiles = neighbours;
                        break;

                    case "H4":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "G3";
                        neighbours[1].tileName = "G5";
                        t.neighborTiles = neighbours;
                        break;

                    case "A5":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "B4";
                        neighbours[1].tileName = "B6";
                        t.neighborTiles = neighbours;
                        break;

                    case "C5":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "B4";
                        neighbours[1].tileName = "D4";
                        neighbours[2].tileName = "B6";
                        neighbours[3].tileName = "D6";
                        t.neighborTiles = neighbours;
                        break;

                    case "E5":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "D4";
                        neighbours[1].tileName = "F4";
                        neighbours[2].tileName = "D6";
                        neighbours[3].tileName = "F6";
                        t.neighborTiles = neighbours;
                        break;

                    case "G5":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "F4";
                        neighbours[1].tileName = "H4";
                        neighbours[2].tileName = "F6";
                        neighbours[3].tileName = "H6";
                        t.neighborTiles = neighbours;
                        break;

                    case "B6":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "A5";
                        neighbours[1].tileName = "C5";
                        neighbours[2].tileName = "A7";
                        neighbours[3].tileName = "C7";
                        t.neighborTiles = neighbours;
                        break;

                    case "D6":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "C5";
                        neighbours[1].tileName = "E5";
                        neighbours[2].tileName = "C7";
                        neighbours[3].tileName = "E7";
                        t.neighborTiles = neighbours;
                        break;

                    case "F6":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "E5";
                        neighbours[1].tileName = "G5";
                        neighbours[2].tileName = "E7";
                        neighbours[3].tileName = "G7";
                        t.neighborTiles = neighbours;
                        break;

                    case "H6":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "G5";
                        neighbours[1].tileName = "G7";
                        t.neighborTiles = neighbours;
                        break;

                    case "A7":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "B6";
                        neighbours[1].tileName = "B8";
                        t.neighborTiles = neighbours;
                        break;

                    case "C7":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "B6";
                        neighbours[1].tileName = "D6";
                        neighbours[2].tileName = "B8";
                        neighbours[3].tileName = "D8";
                        t.neighborTiles = neighbours;
                        break;

                    case "E7":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "D6";
                        neighbours[1].tileName = "F6";
                        neighbours[2].tileName = "D8";
                        neighbours[3].tileName = "F8";
                        t.neighborTiles = neighbours;
                        break;

                    case "G7":
                        neighbours = new TilesDetails[4];
                        neighbours[0].tileName = "F6";
                        neighbours[1].tileName = "H6";
                        neighbours[2].tileName = "F8";
                        neighbours[3].tileName = "H8";
                        t.neighborTiles = neighbours;
                        break;

                    case "B8":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "A7";
                        neighbours[1].tileName = "C7";
                        t.neighborTiles = neighbours;
                        break;

                    case "D8":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "C7";
                        neighbours[1].tileName = "E7";
                        t.neighborTiles = neighbours;
                        break;

                    case "F8":
                        neighbours = new TilesDetails[2];
                        neighbours[0].tileName = "E7";
                        neighbours[1].tileName = "G7";
                        t.neighborTiles = neighbours;
                        break;

                    case "H8":
                        neighbours = new TilesDetails[1];
                        neighbours[0].tileName = "G7";
                        t.neighborTiles = neighbours;
                        break;

                }



            }
        }




    ////endoflegit


}
