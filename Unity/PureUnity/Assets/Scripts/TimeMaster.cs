using System;
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
    [SerializeField] List<TilesDetails> previousWhiteTiles;
    [SerializeField] List<TilesDetails> previousBlackTiles;
    [SerializeField] List <TilesDetails> currentWhiteTiles;
    [SerializeField] List <TilesDetails> currentBlackTiles;

    public int same = 0;
    
   
    void Start()
    {
        turn = 0;
        whiteTurn = true;
    }


    public void CheckMovement()
    {
        if (BoardDetected() && play)
        {

            currentBlackTiles = FindObjectOfType<CheckersDetection>().blackTiles;
            currentWhiteTiles = FindObjectOfType<CheckersDetection>().whiteTiles;

            diff = 0;
                
            if (whiteTurn)
            {
                if (currentWhiteTiles.Count != previousWhiteTiles.Count) diff= -1; // return false;
                else
                {
                    same = 0;
                    for (int i = 0; i < previousWhiteTiles.Count; i++)
                    {
                        for (int j = 0; j < currentWhiteTiles.Count; j++)
                        {
                            if (currentWhiteTiles[j].tileName == previousWhiteTiles[i].tileName)
                            {
                                same++;
                                break;
                            }
                        }
                    }

                    if (same == previousWhiteTiles.Count-1)
                    {
                        previousWhiteTiles = new List<TilesDetails>(currentWhiteTiles);
                        previousBlackTiles = new List<TilesDetails>(currentBlackTiles);
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
                    FindObjectOfType<TextManager>().UpdateCheckersNumber(previousWhiteTiles.Count,previousBlackTiles.Count);
                }
                

            }
            else
            {
                if (currentBlackTiles.Count != previousBlackTiles.Count) diff = -1; // return false;
                else
                {
                    same = 0;
                    for (int i = 0; i < previousBlackTiles.Count; i++)
                    {
                        for (int j = 0; j < currentBlackTiles.Count; j++)
                        {
                            if (currentBlackTiles[j].tileName == previousBlackTiles[i].tileName)
                            {
                                same++;
                                break;
                            }
                        }
                    }

                    if (same == previousBlackTiles.Count - 1)
                    {
                        previousWhiteTiles = new List<TilesDetails>(currentWhiteTiles);
                        previousBlackTiles = new List<TilesDetails>(currentBlackTiles);
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
                    FindObjectOfType<TextManager>().UpdateCheckersNumber(previousWhiteTiles.Count, previousBlackTiles.Count);
                }

            }

                
        }
        
    }

    private bool BoardDetected()
    {
        if (FindObjectOfType<BoardDetector>().AllTiles().Length == 32)
        {
            return true;
        }
        else return false;
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
            if (BoardDetected())
            { 
            var board = FindObjectOfType<BoardDetector>();
            var checkers = FindObjectOfType<CheckersDetection>();
                if (board.RightTilesNumber())
                {
                    int bIndex = 0, wIndex = 0;
                    var tiles = checkers.AllTiles();

                    previousBlackTiles = new List <TilesDetails> (FindObjectOfType<CheckersDetection>().blackTiles);
                    previousWhiteTiles = new List<TilesDetails> (FindObjectOfType<CheckersDetection>().whiteTiles);

                    play = true;
                    FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play);
                }
            }
            else
            {
                //TODO show "Detect Board message first" text
            }
        }
    }

}
