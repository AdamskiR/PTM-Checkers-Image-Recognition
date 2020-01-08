using UnityEngine;

[System.Serializable]
public class TilesDetails
{
    public string tileName;
    public GameObject TileRepresentation3D;
    public Vector3 tilePosition;
    public float distanceToMarker;

    public TilesDetails()
    {
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
