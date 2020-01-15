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
                            Debug.Log("Pole 'bicia':" + previousTile.neighborTiles[i].tileName+ " oraz "+newTile.neighborTiles[j].tileName);
                        if (previousTile.neighborTiles[i].occupiedWhite || previousTile.neighborTiles[i].occupiedBlack)
                            if (!newTile.neighborTiles[i].occupiedWhite && !newTile.neighborTiles[i].occupiedBlack) return true;
                        }
                }
            }
        }
        return false;
    }

    private void MakePRevoiusListCurrent(CheckersDetection checkers)
    {
        for (int i = 0; i < checkers.blackTiles.Count; i++)
            previousBlackTiles.Add(new TilesDetails
            {
                tileName = checkers.blackTiles[i].tileName,
                TileRepresentation3D = checkers.blackTiles[i].TileRepresentation3D,
                tilePosition = checkers.blackTiles[i].tilePosition,
                distanceToMarker = checkers.blackTiles[i].distanceToMarker,
                occupiedBlack = checkers.blackTiles[i].occupiedBlack,
                occupiedWhite = checkers.blackTiles[i].occupiedWhite,
                neighborTiles = checkers.blackTiles[i].neighborTiles,
                neighborWhiteTiles = checkers.blackTiles[i].neighborWhiteTiles,
                neighborBlackTiles = checkers.blackTiles[i].neighborBlackTiles,
                captureTiles = checkers.blackTiles[i].captureTiles
            });
        for (int i = 0; i < checkers.whiteTiles.Count; i++)
            previousWhiteTiles.Add(new TilesDetails
            {
                tileName = checkers.whiteTiles[i].tileName,
                TileRepresentation3D = checkers.whiteTiles[i].TileRepresentation3D,
                tilePosition = checkers.whiteTiles[i].tilePosition,
                distanceToMarker = checkers.whiteTiles[i].distanceToMarker,
                occupiedBlack = checkers.whiteTiles[i].occupiedBlack,
                occupiedWhite = checkers.whiteTiles[i].occupiedWhite,
                neighborTiles = checkers.whiteTiles[i].neighborTiles,
                neighborWhiteTiles = checkers.whiteTiles[i].neighborWhiteTiles,
                neighborBlackTiles = checkers.whiteTiles[i].neighborBlackTiles,
                captureTiles = checkers.whiteTiles[i].captureTiles
            });
    }


    public void CheckMovement()
    {
        if (BoardDetected() && play)
        {
            currentBlackTiles = new List<TilesDetails>(FindObjectOfType<CheckersDetection>().blackTiles);
            currentWhiteTiles = new List <TilesDetails>(FindObjectOfType<CheckersDetection>().whiteTiles);

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
                        else
                        {
                            Debug.Log("Nie wykonano ruchu bez bicia");
                            Debug.Log("Bicie: " + PreviousTile(whiteTurn).tileName + "->->->" + NewTile(whiteTurn).tileName);
                            foreach(var pt in PreviousTile(whiteTurn).neighborTiles)
                            Debug.Log(pt.occupiedBlack);
                            if (LegalCapturePosition(PreviousTile(whiteTurn), NewTile(whiteTurn)))
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
                            else Debug.Log("Nie wykonano bicia");
                        }
                    }
                    else Debug.Log("Nie wykonano ruchu, lub wykonano ich zbyt wiele");
                    FindObjectOfType<TextManager>().UpdateCheckersNumber(previousWhiteTiles.Count, previousBlackTiles.Count);
                }
                else Debug.Log("Stracono poionki");
                

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
                        else
                        {
                            Debug.Log("Nie wykonano ruchu bez bicia");


                            if (LegalCapturePosition(PreviousTile(whiteTurn), NewTile(whiteTurn)))
                            {
                                string move = "Move: " + PreviousTile(whiteTurn).tileName + " ->->-> " + NewTile(whiteTurn).tileName;
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
                            else Debug.Log("Nie wykonano bicia");


                        }
                    }
                    else Debug.Log("Nie wykonano ruchu, lub wykonano ich zbyt wiele");
                    FindObjectOfType<TextManager>().UpdateCheckersNumber(previousWhiteTiles.Count, previousBlackTiles.Count);

                }
                else Debug.Log("Stracono poionki");
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
                    for (int i = 0; i < checkers.blackTiles.Count; i++)
                        previousBlackTiles.Add(new TilesDetails {
                            tileName = checkers.blackTiles[i].tileName,
                            TileRepresentation3D = checkers.blackTiles[i].TileRepresentation3D,
                            tilePosition = checkers.blackTiles[i].tilePosition,
                            distanceToMarker = checkers.blackTiles[i].distanceToMarker,
                            occupiedBlack = checkers.blackTiles[i].occupiedBlack,
                            occupiedWhite = checkers.blackTiles[i].occupiedWhite,
                            neighborTiles = checkers.blackTiles[i].neighborTiles,
                            neighborWhiteTiles = checkers.blackTiles[i].neighborWhiteTiles,
                            neighborBlackTiles = checkers.blackTiles[i].neighborBlackTiles,
                            captureTiles = checkers.blackTiles[i].captureTiles
                        } );
                    for (int i = 0; i < checkers.whiteTiles.Count; i++)
                        previousWhiteTiles.Add(new TilesDetails
                        {
                            tileName = checkers.whiteTiles[i].tileName,
                            TileRepresentation3D = checkers.whiteTiles[i].TileRepresentation3D,
                            tilePosition = checkers.whiteTiles[i].tilePosition,
                            distanceToMarker = checkers.whiteTiles[i].distanceToMarker,
                            occupiedBlack = checkers.whiteTiles[i].occupiedBlack,
                            occupiedWhite = checkers.whiteTiles[i].occupiedWhite,
                            neighborTiles = checkers.whiteTiles[i].neighborTiles,
                            neighborWhiteTiles = checkers.whiteTiles[i].neighborWhiteTiles,
                            neighborBlackTiles = checkers.whiteTiles[i].neighborBlackTiles,
                            captureTiles = checkers.whiteTiles[i].captureTiles
                        });

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
