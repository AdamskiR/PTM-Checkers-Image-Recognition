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

    private bool WhiteNumberSame()
    {
        if (currentWhiteTiles.Count != previousWhiteTiles.Count) return false;
        else return true;
    }
    private bool BlackNumberSame()
    {
        if (currentBlackTiles.Count != previousBlackTiles.Count) return false;
        else return true;
    }

    private bool OneWhiteCheckerMoved()
    {
        int same = 0;
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

        if (same == previousWhiteTiles.Count - 1) return true;
        else return false;
    }

    private TilesDetails PreviousTile(bool playerWhite)
    {
        if (playerWhite)
        {
            for (int i = 0; i < previousWhiteTiles.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < currentWhiteTiles.Count; j++)
                {
                    if (currentWhiteTiles[j].tileName == previousWhiteTiles[i].tileName)
                    {
                        found = true;
                        break;
                    }
                    if (!found) return previousWhiteTiles[i];
                }
            }
            return null;
        }
        else
        {
            for (int i = 0; i < previousBlackTiles.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < currentBlackTiles.Count; j++)
                {
                    if (currentBlackTiles[j].tileName == previousBlackTiles[i].tileName)
                    {
                        found = true;
                        break;
                    }
                    if (!found) return previousBlackTiles[i];
                }
            }
            return null;
        }
    }

    private TilesDetails NewTile(bool playerWhite)
    {
        if (playerWhite)
        {
            for (int i = 0; i < previousWhiteTiles.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < currentWhiteTiles.Count; j++)
                {
                    if (currentWhiteTiles[j].tileName == previousWhiteTiles[i].tileName)
                    {
                        found = true;
                        break;
                    }
                    if (!found) return currentWhiteTiles[i];
                }
            }
            return null;
        }
        else
        {
            for (int i = 0; i < previousBlackTiles.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < currentBlackTiles.Count; j++)
                {
                    if (currentBlackTiles[j].tileName == previousBlackTiles[i].tileName)
                    {
                        found = true;
                        break;
                    }
                    if (!found) return currentBlackTiles[i];
                }
            }
            return null;
        }
    }

    private bool OneBlackCheckerMoved()
    {
        int same = 0;
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

        if (same == previousBlackTiles.Count - 1) return true;
        else return false;
    }

    private bool LegalMovePosition(TilesDetails previousTile, TilesDetails newTile, bool playerWhite)
    {
        if (playerWhite)
        {
            foreach (var pt in previousTile.neighborWhiteTiles)
            {
                if (pt.tileName == newTile.tileName) return true;
            }
            return false;
        }
        else
        {
            foreach (var pt in previousTile.neighborBlackTiles)
            {
                if (pt.tileName == newTile.tileName) return true;
            }
            return false;
        }

    }

    private bool LegalCapturePosition(TilesDetails previousTile, TilesDetails newTile)
    {
        foreach (var ct in previousTile.captureTiles)
        {
            if (ct.tileName == newTile.tileName)
            {
                for (int i=0;i< previousTile.neighborTiles.Length;i++)
                    for (int j = 0; j < newTile.neighborTiles.Length; j++)
                    {
                    if (previousTile.neighborTiles[i].tileName == newTile.neighborTiles[j].tileName)
                    {
                        if (!previousTile.neighborTiles[i].occupiedWhite && !previousTile.neighborTiles[i].occupiedBlack) return true;
                    }
                }
            }
        }
        return false;
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
                if (WhiteNumberSame())
                {
                    if (OneWhiteCheckerMoved())
                    {
                        if (LegalMovePosition(PreviousTile(whiteTurn), NewTile(whiteTurn), whiteTurn))
                        {
                            string move = "Move: " + PreviousTile(whiteTurn).tileName + " -> " + NewTile(whiteTurn).tileName;
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
                            FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, move);
                        }
                    }
                    FindObjectOfType<TextManager>().UpdateCheckersNumber(previousWhiteTiles.Count,previousBlackTiles.Count);
                }
                

            }
            else
            {
                if (BlackNumberSame())
                {
                    if (OneBlackCheckerMoved())
                    {
                        if (LegalMovePosition(PreviousTile(whiteTurn), NewTile(whiteTurn), whiteTurn))
                        {
                            string move = "Move: " + PreviousTile(whiteTurn).tileName + " -> " + NewTile(whiteTurn).tileName;
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
                            FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, move);
                        }
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
            FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, "stop");
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
                    FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, "white first");
                }
            }
            else
            {
                //TODO show "Detect Board message first" text
            }
        }
    }

}
