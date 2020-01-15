using UnityEngine;

[System.Serializable]
public class NeighbourTilesDetails
{
    public string tileName;
    public GameObject TileRepresentation3D;
    public bool occupiedWhite = false;
    public bool occupiedBlack = false;
    public TilesDetails []captureTiles;


    public NeighbourTilesDetails()
    {
    }

    public NeighbourTilesDetails(TilesDetails t)
    {
        tileName = t.tileName;
        TileRepresentation3D = t.TileRepresentation3D;
        occupiedBlack = t.occupiedBlack;
        occupiedWhite = t.occupiedWhite;
    }

    public NeighbourTilesDetails(NeighbourTilesDetails t)
    {
        tileName = t.tileName;
        TileRepresentation3D = t.TileRepresentation3D;
        occupiedBlack = t.occupiedBlack;
        occupiedWhite = t.occupiedWhite;
        captureTiles = t.captureTiles;
    }
}
