using System;
using UnityEngine;

[System.Serializable]
public class TilesDetails 
{
    public string tileName;
    public GameObject TileRepresentation3D;
    public Vector3 tilePosition;
    public float distanceToMarker;
    public bool occupiedWhite = false;
    public bool occupiedBlack = false;
    public bool king = false;
    public NeighbourTilesDetails[] neighborTiles;
    public NeighbourTilesDetails[] neighborWhiteTiles;
    public NeighbourTilesDetails[] neighborBlackTiles;
    public NeighbourTilesDetails[]captureTiles;


    public TilesDetails()
    {
    }


    public TilesDetails(TilesDetails t)
    {
        tileName = t.tileName;
        TileRepresentation3D = t.TileRepresentation3D;
        tilePosition = t.tilePosition;
        distanceToMarker = t.distanceToMarker;
        occupiedBlack = t.occupiedBlack;
        occupiedWhite = t.occupiedWhite;

    }

    public TilesDetails(Vector3 pos)
    {
        tilePosition = pos;
    }

    public TilesDetails(Vector3 pos, Vector3 marker)
    {
        tilePosition = pos;
        distanceToMarker = Vector3.Distance(tilePosition, marker);
    }

    private void CalculateDistance(Vector3 marker)
    {
        distanceToMarker = Vector3.Distance(tilePosition, marker);
    }
}
