using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMaster : MonoBehaviour
{
    [Header ("time")]
    [SerializeField] int turn = 0;
    [SerializeField] bool whiteTurn = true;
    [SerializeField] bool play;
    [SerializeField] int diff;
    //[SerializeField] TilesDetails[] previousBoard;
    // [SerializeField] TilesDetails[] currentBoard;

    [Header("checkers position")]
    [SerializeField] TilesDetails[] previousWhiteTiles;
    [SerializeField] TilesDetails[] previousBlackTiles;
    [SerializeField] TilesDetails[] currentWhiteTiles;
    [SerializeField] TilesDetails[] currentBlackTiles;
   

    
   
    void Start()
    {
        turn = 0;
        whiteTurn = true;
    }


    public void CheckMovement()
    {
        if (FindObjectOfType<TileTracker>().RightTilesNumber() && play)
        {
            foreach (var t in FindObjectOfType<TileTracker>().AllTiles())
            {
                if (t.occupiedBlack) currentBlackTiles[currentBlackTiles.Length] = t;
                if (t.occupiedWhite) currentWhiteTiles[currentWhiteTiles.Length] = t;
            }

            diff = 0;
                //check difrence in chekers position
            for (int i=0;i< previousBlackTiles.Length;i++)
                {
                    if (currentBlackTiles[i].occupiedBlack != previousBlackTiles[i].occupiedBlack)
                    {
                        diff++;
                    }
                }

            for (int i = 0; i < previousWhiteTiles.Length; i++)
            {
                if (currentWhiteTiles[i].occupiedBlack != previousWhiteTiles[i].occupiedBlack)
                {
                    diff++;
                }
            }

            if (diff == 2)
                {
                previousWhiteTiles = currentWhiteTiles;
                previousBlackTiles = currentBlackTiles;

                    if (!whiteTurn)
                    {
                        turn++;
                        whiteTurn = true;
                       
                    }
                    else
                    {
                        whiteTurn = false;
                    }
                    FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play);
                }
                
        }
    }

    public void PlayButton()
    {
        if (play)
        {
            play = false;
            FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play);
        }
        else
        {
            var TT = FindObjectOfType<TileTracker>();
            if (TT.RightTilesNumber())
            {
                int bIndex = 0, wIndex = 0;
                foreach (var t in FindObjectOfType<TileTracker>().AllTiles())
                {
                    previousBlackTiles = new TilesDetails[TT.blackCheckers.Length];
                    previousWhiteTiles = new TilesDetails[TT.whiteCheckers.Length];
                    if (t.occupiedBlack) { previousBlackTiles[bIndex] = t; bIndex++; }
                    if (t.occupiedWhite) { previousWhiteTiles[wIndex] = t; wIndex++; }
                }
                
                play = true;
                FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play);
            }
        }
    }

}
