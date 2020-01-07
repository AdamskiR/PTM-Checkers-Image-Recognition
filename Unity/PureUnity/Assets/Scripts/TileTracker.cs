using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTracker : MonoBehaviour
{
    [SerializeField] Vector3[] whiteCheckers;
    [SerializeField] Vector3[] marker;
    [SerializeField] Vector3[] tiles ;
    [SerializeField] GameObject[] TileRepresentation3D = new GameObject[8];
    [SerializeField] GameObject[] whiteRepresentation3D = new GameObject[6];



    private void Start()
    {
        
        tiles = new Vector3[8];
        marker = new Vector3[1];
    }

    public void CalculateTiles()
    {
        for (int i = 0; i < 8; i++)
        {
            whiteRepresentation3D[i].transform.position = new Vector3(0, -30, 0);
        }

            int tilesNumber = FindObjectOfType<ReadCameraInput>().middlePoints.Count;
        if (tilesNumber > 8) tilesNumber = 8;

        int checkerNumber = FindObjectOfType<ReadWhiteColor>().middlePoints.Count;
        if (checkerNumber > 16) checkerNumber = 16;
        whiteCheckers = new Vector3[checkerNumber];

        int markerNumber = FindObjectOfType<ReadYellowColor>().middlePoints.Count;
        if (markerNumber > 1) markerNumber = 1;

      //  try
       // {
            for (int i = 0; i < tilesNumber; i++)
            {
                tiles[i] = new Vector3(FindObjectOfType<ReadCameraInput>().middlePoints[i].x, FindObjectOfType<ReadCameraInput>().middlePoints[i].y, 1);
            }

       for (int i = 0; i < checkerNumber; i++)
            {
                whiteCheckers[i] = new Vector3(FindObjectOfType<ReadWhiteColor>().middlePoints[i].x, FindObjectOfType<ReadWhiteColor>().middlePoints[i].y, 1);
            }

            for (int i = 0; i < markerNumber; i++)
            {
                marker[i] = new Vector3(FindObjectOfType<ReadYellowColor>().middlePoints[i].x, FindObjectOfType<ReadYellowColor>().middlePoints[i].y, 1);
            }//TODO make it to make sense, pls array for one?!
             /*}
              catch (Exception e)
              {
                  Debug.Log("Nie odczytano koordynatnow planszy");
              }*/
        //Debug.Log("nie mamy 8 pol");

        try
        { 

            if (tiles.Length == 8)
            {
               // Debug.Log("mamy 8 pol");
                float[] namedTiles = new float [8];
               Vector3[] sortedTiles = new Vector3[8];

               /* for (int i = 0; i < 8; i++)
                {
                  float dist = Vector3.Distance(marker[0], tiles[i]);
                    namedTiles[i] = dist;
                } //named tiles

                for (int i = 0; i < 8; i++)
                {
                    float max = 0;
                    int index = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        if (namedTiles[j] > max)
                        {
                            max = namedTiles[j];
                            index = j;
                        }
                    }
                    sortedTiles[7 - i] = tiles[index];
                    namedTiles[index] = 0;
                
                } //named tiles*/



                for (int i = 0; i < whiteCheckers.Length; i++)
                {
                   // Debug.Log("mamy pionki");
                    int index = -1;
                    float minDistance = 500;
                    for (int j = 0; j < tiles.Length; j++)
                    {
                        float distChecker = Vector3.Distance(whiteCheckers[i], marker[0]);
                        float distTile = Vector3.Distance( tiles[j], marker[0]);
                       // float dist = Vector3.Distance(whiteCheckers[i], sortedTiles[j]); //tiles
                        if (Math.Abs(distChecker-distTile) < minDistance)
                        {
                            minDistance = Math.Abs(distChecker - distTile);
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


            }



            //Mathf.Max(tempArray)


        }
        catch (Exception e)
        {
            Debug.Log("Nie ukonczono wizualizacji 3D");
        }


    }

}
