using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardDetector : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] Vector3 marker;
    [SerializeField] TilesDetails[] tiles;

    [Header("3D Objects")]
    [SerializeField] GameObject[] TileRepresentation3D;
    [SerializeField] int desirableTilesNumber = 32;
    [SerializeField] int markerTileA1Distance = 30;

    private int tilesNumber;

    CameraInput photoTaker;
    ReadColorRedinHSV red;
    Simple2DVisualizer visual;
    ReadColorGreeninHSV green;

    private void Start()
    {
        photoTaker = FindObjectOfType<CameraInput>();
        visual = FindObjectOfType<Simple2DVisualizer>();
        red = FindObjectOfType<ReadColorRedinHSV>();
        green = FindObjectOfType<ReadColorGreeninHSV>();
    }

    public void DetectBoard()
    { 
        {
            photoTaker.TakePhoto();
            red.ReadColors();
            green.ReadColors();
            visual.ShowTiles();
            CalculateBoard();
        }
    }

    private void CalculateBoard()
    {
       // try
        {
            SetNumberOfObjects();
            tiles = new TilesDetails[tilesNumber];
            GetObjectsData();
            SortTilesByCoordinates();
            Link3DObjectsWithCameraInput();

            FillCaptureTiles();
            FillNeighbours();
        }
       // catch (Exception e)
        {
          //  Debug.Log("Blad wykrywania planszy");
        }
    }

    private void SetNumberOfObjects()
    {
        tilesNumber = FindObjectOfType<ReadColorRedinHSV>().middlePoints.Count;
        if (tilesNumber > 40) tilesNumber = 40;
    }

    public bool RightTilesNumber()
    {
        if (desirableTilesNumber == tilesNumber)
            return true;
        else return false;
    }

    public TilesDetails[] AllTiles()
    {
        return tiles;
    }

    public GameObject[] AllTileRepresentation3D()
    {
        return TileRepresentation3D;
    }

    private void GetObjectsData()
    {
        try
        {
            if (FindObjectOfType<ReadColorGreeninHSV>().middlePoints.Count > 0)
                marker = new Vector3(FindObjectOfType<ReadColorGreeninHSV>().middlePoints[0].x, FindObjectOfType<ReadColorGreeninHSV>().middlePoints[0].y, 1);

            for (int i = 0; i < tilesNumber; i++)
            {
                tiles[i] = new TilesDetails(new Vector3(FindObjectOfType<ReadColorRedinHSV>().middlePoints[i].x, FindObjectOfType<ReadColorRedinHSV>().middlePoints[i].y, 1), marker);
            }

        }
        catch (Exception e)
        {
            Debug.Log("Nie odczytano pól");
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

    private void SortTilesByCoordinates()
    {
        tiles = tiles.OrderBy(x => x.distanceToMarker).ToArray();
        float cornerRU = tiles[0].tilePosition.y - marker.y;
        float cornerLD = marker.y - tiles[0].tilePosition.y;
        float cornerLU = marker.x - tiles[0].tilePosition.x;
        float cornerRD = tiles[0].tilePosition.x - marker.x;
        Debug.Log("RU: " + cornerRU + ", RD" + cornerRD + " ,LD" + cornerLD + " ,LU" + cornerLU);

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

    private NeighbourTilesDetails FindTile(string name)
    {
        foreach (var t in tiles)
        {
            if (t.tileName == name) return new NeighbourTilesDetails(t);
        }
        return null;
    }

    private void FillCaptureTiles()
    {
        NeighbourTilesDetails[] neighbours;

        foreach (var t in tiles)
        {
            switch (t.tileName)
            {
                case "A1":
                    neighbours = new NeighbourTilesDetails[1];
                    neighbours[0] = FindTile("C3");
                    t.captureTiles = neighbours;
                    break;

                case "C1":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("C3");
                    neighbours[1] = FindTile("E3");
                    t.captureTiles = neighbours;
                    break;

                case "E1":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("C3");
                    neighbours[1] = FindTile("G3");
                    t.captureTiles = neighbours;
                    break;

                case "G1":
                    neighbours = new NeighbourTilesDetails[1];
                    neighbours[0] = FindTile("E3");
                    t.captureTiles = neighbours;
                    break;

                case "B2":
                    neighbours = new NeighbourTilesDetails[1];
                    neighbours[0] = FindTile("D4");
                    t.captureTiles = neighbours;
                    break;

                case "D2":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("B4");
                    neighbours[1] = FindTile("F4");
                    t.captureTiles = neighbours;
                    break;

                case "F2":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("D4");
                    neighbours[1] = FindTile("H4");
                    t.captureTiles = neighbours;
                    break;

                case "H2":
                    neighbours = new NeighbourTilesDetails[1];
                    neighbours[0] = FindTile("F4");
                    t.captureTiles = neighbours;
                    break;

                case "A3":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("C1");
                    neighbours[1] = FindTile("C5");
                    t.captureTiles = neighbours;
                    break;

                case "C3":
                    neighbours = new NeighbourTilesDetails[4];
                    neighbours[0] = FindTile("A1");
                    neighbours[1] = FindTile("E1");
                    neighbours[2] = FindTile("A5");
                    neighbours[3] = FindTile("E5");
                    t.captureTiles = neighbours;
                    break;

                case "E3":
                    neighbours = new NeighbourTilesDetails[4];
                    neighbours[0] = FindTile("C1");
                    neighbours[1] = FindTile("G1");
                    neighbours[2] = FindTile("C5");
                    neighbours[3] = FindTile("G5");
                    t.captureTiles = neighbours;
                    break;

                case "G3":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("E1");
                    neighbours[1] = FindTile("E5");
                    t.captureTiles = neighbours;
                    break;

                case "B4":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("D2");
                    neighbours[1] = FindTile("D6");
                    t.captureTiles = neighbours;
                    break;

                case "D4":
                    neighbours = new NeighbourTilesDetails[4];
                    neighbours[0] = FindTile("B2");
                    neighbours[1] = FindTile("F2");
                    neighbours[2] = FindTile("B6");
                    neighbours[3] = FindTile("F6");
                    t.captureTiles = neighbours;
                    break;

                case "F4":
                    neighbours = new NeighbourTilesDetails[4];
                    neighbours[0] = FindTile("D2");
                    neighbours[1] = FindTile("H2");
                    neighbours[2] = FindTile("D6");
                    neighbours[3] = FindTile("H6");
                    t.captureTiles = neighbours;
                    break;

                case "H4":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("F2");
                    neighbours[1] = FindTile("F6");
                    t.captureTiles = neighbours;
                    break;

                case "A5":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("C3");
                    neighbours[1] = FindTile("C7");
                    t.captureTiles = neighbours;
                    break;

                case "C5":
                    neighbours = new NeighbourTilesDetails[4];
                    neighbours[0] = FindTile("A3");
                    neighbours[1] = FindTile("E3");
                    neighbours[2] = FindTile("A7");
                    neighbours[3] = FindTile("E7");
                    t.captureTiles = neighbours;
                    break;

                case "E5":
                    neighbours = new NeighbourTilesDetails[4];
                    neighbours[0] = FindTile("C3");
                    neighbours[1] = FindTile("G3");
                    neighbours[2] = FindTile("C7");
                    neighbours[3] = FindTile("G7");
                    t.captureTiles = neighbours;
                    break;

                case "G5":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("E3");
                    neighbours[1] = FindTile("E7");
                    t.captureTiles = neighbours;
                    break;

                case "B6":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("D4");
                    neighbours[1] = FindTile("D8");
                    t.captureTiles = neighbours;
                    break;

                case "D6":
                    neighbours = new NeighbourTilesDetails[4];
                    neighbours[0] = FindTile("B4");
                    neighbours[1] = FindTile("F4");
                    neighbours[2] = FindTile("B8");
                    neighbours[3] = FindTile("F8");
                    t.captureTiles = neighbours;
                    break;

                case "F6":
                    neighbours = new NeighbourTilesDetails[4];
                    neighbours[0] = FindTile("D4");
                    neighbours[1] = FindTile("H4");
                    neighbours[2] = FindTile("D8");
                    neighbours[3] = FindTile("H8");
                    t.captureTiles = neighbours;
                    break;

                case "H6":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("F4");
                    neighbours[1] = FindTile("F8");
                    t.captureTiles = neighbours;
                    break;

                case "A7":
                    neighbours = new NeighbourTilesDetails[1];
                    neighbours[0] = FindTile("C5");
                    t.captureTiles = neighbours;
                    break;

                case "C7":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("A5");
                    neighbours[1] = FindTile("E5");
                    t.captureTiles = neighbours;
                    break;

                case "E7":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("C5");
                    neighbours[1] = FindTile("G5");
                    t.captureTiles = neighbours;
                    break;

                case "G7":
                    neighbours = new NeighbourTilesDetails[1];
                    neighbours[0] = FindTile("E5");
                    t.captureTiles = neighbours;
                    break;

                case "B8":
                    neighbours = new NeighbourTilesDetails[1];
                    neighbours[0] = FindTile("D6");
                    t.captureTiles = neighbours;
                    break;

                case "D8":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("B6");
                    neighbours[1] = FindTile("F6");
                    t.captureTiles = neighbours;
                    break;

                case "F8":
                    neighbours = new NeighbourTilesDetails[2];
                    neighbours[0] = FindTile("D6");
                    neighbours[1] = FindTile("H8");
                    t.captureTiles = neighbours;
                    break;

                case "H8":
                    neighbours = new NeighbourTilesDetails[1];
                    neighbours[0] = FindTile("F6");
                    t.captureTiles = neighbours;
                    break;

            }
        }
    }
   
    private void FillNeighbours()
    {
        NeighbourTilesDetails[] neighbours;
        NeighbourTilesDetails[] blackNeighbours;
        NeighbourTilesDetails[] whiteNeighbours;

        foreach (var t in tiles)
        {
            switch (t.tileName)
            {
                case "A1":
                    neighbours = new NeighbourTilesDetails[1];
                    whiteNeighbours = new NeighbourTilesDetails[1];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("B2"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("B2"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    break;

                case "C1":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[1];
                    blackNeighbours = new NeighbourTilesDetails[1];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("D2"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("B2"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("D2"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("B2"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "E1":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[1];
                    blackNeighbours = new NeighbourTilesDetails[1];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("D2"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("F2"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("F2"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("D2"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "G1":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[1];
                    blackNeighbours = new NeighbourTilesDetails[1];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("F2"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("H2"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("H2"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("F2"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "B2":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = FindTile("A1");
                    neighbours[1] = new NeighbourTilesDetails(FindTile("C1"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("A3"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("C3"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("C1"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("C3"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("A1"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("A3"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "D2":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("C1"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("E1"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("C3"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("E3"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("E1"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("E3"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("C1"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("C3"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "F2":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("E1"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("G1"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("E3"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("G3"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("G1"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("G3"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("E1"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("E3"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "H2":
                    neighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[0];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("G1"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("G3"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("G1"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("G3"));
                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "A3":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[0];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("B2"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("B4"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("B2"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("B4"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "C3":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("B2"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("D2"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("B4"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("D4"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("D2"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("D4"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("B2"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("B4"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "E3":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("D2"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("F2"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("F4"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("D4"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("F2"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("F4"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("D2"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("D4"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "G3":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("F2"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("H2"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("F4"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("H4"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("H2"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("H4"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("F2"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("F4"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "B4":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("A3"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("C3"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("A5"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("C5"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("C3"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("C5"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("A3"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("A5"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "D4":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("C3"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("E3"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("C5"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("E5"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("E3"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("E5"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("C3"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("C5"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "F4":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("E3"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("G3"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("E5"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("G5"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("G3"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("G5"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("E3"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("E5"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "H4":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[0];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("G3"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("G5"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("G3"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("G5"));
                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "A5":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[0];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("B4"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("B6"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("B4"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("B6"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "C5":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("B4"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("D4"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("B6"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("D6"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("D4"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("D6"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("B4"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("B6"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "E5":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("D4"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("F4"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("D6"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("F6"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("F4"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("F6"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("D4"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("D6"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "G5":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("F4"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("H4"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("F6"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("H6"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("H4"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("H6"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("F4"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("F6"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "B6":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("C5"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("C5"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("A7"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("C7"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("C5"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("C7"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("A5"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("A7"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "D6":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("C5"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("E5"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("C7"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("E7"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("E5"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("E7"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("C5"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("C7"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "F6":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("E5"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("G5"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("E7"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("G7"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("G5"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("G7"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("E5"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("E7"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "H6":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[0];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("G5"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("G7"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("G5"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("G7"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "A7":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[0];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("B6"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("B8"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("B6"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("B8"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "C7":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("B6"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("D6"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("C5"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("D8"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("D6"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("D8"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("B6"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("B8"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "E7":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("D6"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("F6"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("D8"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("F8"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("F6"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("F8"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("D6"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("D8"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "G7":
                    neighbours = new NeighbourTilesDetails[4];
                    whiteNeighbours = new NeighbourTilesDetails[2];
                    blackNeighbours = new NeighbourTilesDetails[2];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("F6"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("H6"));
                    neighbours[2] = new NeighbourTilesDetails(FindTile("F8"));
                    neighbours[3] = new NeighbourTilesDetails(FindTile("H8"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("H6"));
                    whiteNeighbours[1] = new NeighbourTilesDetails(FindTile("H8"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("F6"));
                    blackNeighbours[1] = new NeighbourTilesDetails(FindTile("F8"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "B8":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[1];
                    blackNeighbours = new NeighbourTilesDetails[1];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("A7"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("C7"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("C7"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("A7"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "D8":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[1];
                    blackNeighbours = new NeighbourTilesDetails[1];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("C7"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("E7"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("E7"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("C7"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "F8":
                    neighbours = new NeighbourTilesDetails[2];
                    whiteNeighbours = new NeighbourTilesDetails[1];
                    blackNeighbours = new NeighbourTilesDetails[1];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("E7"));
                    neighbours[1] = new NeighbourTilesDetails(FindTile("G7"));
                    whiteNeighbours[0] = new NeighbourTilesDetails(FindTile("G7"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("H8"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

                case "H8":
                    neighbours = new NeighbourTilesDetails[1];
                    whiteNeighbours = new NeighbourTilesDetails[0];
                    blackNeighbours = new NeighbourTilesDetails[1];

                    neighbours[0] = new NeighbourTilesDetails(FindTile("G7"));
                    blackNeighbours[0] = new NeighbourTilesDetails(FindTile("G7"));

                    t.neighborTiles = neighbours;
                    t.neighborWhiteTiles = whiteNeighbours;
                    t.neighborBlackTiles = blackNeighbours;
                    break;

            }
        }
    }
}
