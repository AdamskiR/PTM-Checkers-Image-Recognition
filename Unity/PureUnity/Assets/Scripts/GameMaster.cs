using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [Header("time")]
    [SerializeField] int turn = 0;
    [SerializeField] bool whiteTurn = true;
    [SerializeField] bool play;

    [Header("checkers position")]
    [SerializeField] List<TilesDetails> previousWhiteTiles;
    [SerializeField] List<TilesDetails> previousBlackTiles;
    [SerializeField] List<TilesDetails> previousKings;
    [SerializeField] List<TilesDetails> currentWhiteTiles;
    [SerializeField] List<TilesDetails> currentBlackTiles;
    [SerializeField] List<TilesDetails> currentKings;
    [SerializeField] string mandatoryTile = "";

    [SerializeField] Canvas whiteWin;
    [SerializeField] Canvas blackWin;


    public int same = 0;
    bool capturingPending = false;
    private int whiteChecker = 0, whiteKing = 0;
    private int blackChecker = 0, blackKing = 0;

    void Start()
    {
        turn = 0;
        whiteTurn = true;
    }

    #region(Checkers number)
    private bool WhiteNumberSame()
    {
        if (currentWhiteTiles.Count != previousWhiteTiles.Count)
        {
            Debug.Log("Zmiana ilości pionków białych!");
            return false;
        }
        else return true;
    }
    private bool BlackNumberSame()
    {
        if (currentBlackTiles.Count != previousBlackTiles.Count)
        {
            Debug.Log("Zmiana ilości pionków czarnych!");
            return false;
        }
        else return true;
    }

    private bool OneWhiteCheckerMoved()
    {
        int same = 0;

        for (int i = 0; i < previousWhiteTiles.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < currentWhiteTiles.Count; j++)
            {
                if (currentWhiteTiles[j].tileName == previousWhiteTiles[i].tileName)
                {
                    //Debug.Log("Found: " + currentWhiteTiles[j].tileName + previousWhiteTiles[i].tileName);
                    found = true;
                    break;
                }
            }
            if (!found)
            {

                same++;
            }
        }

        if (same == 1) return true;
        else
        {
            Debug.Log("Wykonaj ruch tylko jednym pionkiem białym! Wykonano ruchy:" + same);
            return false;
        }
    }
    private bool OneBlackCheckerMoved()
    {
        int same = 0;

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
            }
            if (!found)
            {

                same++;
            }
        }

        if (same == 1) return true;
        else
        {
            Debug.Log("Wykonaj ruch tylko jednym pionkiem czarnym! Wykonano ruchy:" + same);
            return false;
        }
    }
    #endregion


    #region(Tiles Finders)
    private TilesDetails PreviousTile()
    {
        if (whiteTurn)
        {
            //if (currentWhiteTiles.Count == previousWhiteTiles.Count && currentBlackTiles.Count == previousBlackTiles.Count)
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
                    }
                    if (!found)
                    {
                        return previousWhiteTiles[i];
                    }
                }
                Debug.Log("OH NO I AM NULL!!!!");
                return null;
            }

        }
        else
        {
            //if (currentBlackTiles.Count == previousBlackTiles.Count && currentWhiteTiles.Count == previousWhiteTiles.Count)
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
                    }
                    if (!found)
                    {
                        return previousBlackTiles[i];
                    }
                }
                Debug.Log("OH NO I AM NULL!!!!");
                return null;
            }
        }
        //return null;

    }
    private TilesDetails NewTile()
    {
        if (whiteTurn)
        {
            //if (currentWhiteTiles.Count == previousWhiteTiles.Count && currentBlackTiles.Count == previousBlackTiles.Count)
            {

                for (int i = 0; i < currentWhiteTiles.Count; i++)
                {
                    bool found = false;
                    for (int j = 0; j < previousWhiteTiles.Count; j++)
                    {
                        if (currentWhiteTiles[i].tileName == previousWhiteTiles[j].tileName)
                        {
                            found = true;
                            break;
                        }

                    }
                    if (!found) return currentWhiteTiles[i];
                }
                Debug.Log("OH NO I AM NULL!!!!");
                return null;
            }
        }
        else
        {
            //if (currentBlackTiles.Count == previousBlackTiles.Count && currentWhiteTiles.Count == previousWhiteTiles.Count)
            {

                for (int i = 0; i < currentBlackTiles.Count; i++)
                {
                    bool found = false;
                    for (int j = 0; j < previousBlackTiles.Count; j++)
                    {
                        if (currentBlackTiles[i].tileName == previousBlackTiles[j].tileName)
                        {
                            found = true;
                            break;
                        }

                    }
                    if (!found) return currentBlackTiles[i];
                }
                Debug.Log("OH NO I AM NULL!!!!");
                return null;
            }
        }
        // return null;
    }
    private TilesDetails FindTile(string name)
    {
        var tiles = FindObjectOfType<BoardDetector>().AllTiles();
        foreach (var t in tiles)
        {
            if (t.tileName == name) return t;
        }
        return null;
    }
    #endregion

    public bool IsGameOn()
    {
        return play;
    }

    public void ToogleWhites()
    {
        if (whiteTurn)
        {
            whiteTurn = false;
            FindObjectOfType<TextManager>().UpdateText(whiteTurn, "Black turn");
        }
        else
        {
            whiteTurn = true;
            FindObjectOfType<TextManager>().UpdateText(whiteTurn, "White turn");
        }
    }


    private void CheckCurrentCheckersLocation()
    {
        currentBlackTiles = new List<TilesDetails>(FindObjectOfType<CheckersDetection>().blackTiles);
        currentWhiteTiles = new List<TilesDetails>(FindObjectOfType<CheckersDetection>().whiteTiles);
        currentKings = new List<TilesDetails>(FindObjectOfType<CheckersDetection>().kingsTiles);
    }
    private void EndTurn()
    {
        mandatoryTile = "";
        if (!whiteTurn)
        {
            turn++;
            whiteTurn = true;
            FindObjectOfType<TextManager>().UpdateCheckersNumber(previousWhiteTiles.Count, previousBlackTiles.Count);
        }
        else
        {
            whiteTurn = false;
            FindObjectOfType<TextManager>().UpdateCheckersNumber(previousWhiteTiles.Count, previousBlackTiles.Count);

        }
    }
    private void MakePrevoiusListCurrent(List<TilesDetails> checkersW, List<TilesDetails> checkersB, List<TilesDetails> kings)
    {
        previousBlackTiles.Clear();
        previousWhiteTiles.Clear();
        previousKings.Clear();
        for (int i = 0; i < checkersW.Count; i++)
            previousWhiteTiles.Add(new TilesDetails
            {
                tileName = checkersW[i].tileName,
                TileRepresentation3D = checkersW[i].TileRepresentation3D,
                tilePosition = checkersW[i].tilePosition,
                distanceToMarker = checkersW[i].distanceToMarker,
                occupiedBlack = checkersW[i].occupiedBlack,
                occupiedWhite = checkersW[i].occupiedWhite,
                king = checkersW[i].king,
                neighborTiles = checkersW[i].neighborTiles,
                neighborWhiteTiles = checkersW[i].neighborWhiteTiles,
                neighborBlackTiles = checkersW[i].neighborBlackTiles,
                captureTiles = checkersW[i].captureTiles
            });
        for (int i = 0; i < checkersB.Count; i++)
            previousBlackTiles.Add(new TilesDetails
            {
                tileName = checkersB[i].tileName,
                TileRepresentation3D = checkersB[i].TileRepresentation3D,
                tilePosition = checkersB[i].tilePosition,
                distanceToMarker = checkersB[i].distanceToMarker,
                occupiedBlack = checkersB[i].occupiedBlack,
                occupiedWhite = checkersB[i].occupiedWhite,
                king = checkersB[i].king,
                neighborTiles = checkersB[i].neighborTiles,
                neighborWhiteTiles = checkersB[i].neighborWhiteTiles,
                neighborBlackTiles = checkersB[i].neighborBlackTiles,
                captureTiles = checkersB[i].captureTiles
            });

        for (int i = 0; i < kings.Count; i++)
            previousKings.Add(new TilesDetails
            {
                tileName = kings[i].tileName,
                TileRepresentation3D = kings[i].TileRepresentation3D,
                tilePosition = kings[i].tilePosition,
                distanceToMarker = kings[i].distanceToMarker,
                occupiedBlack = kings[i].occupiedBlack,
                occupiedWhite = kings[i].occupiedWhite,
                king = kings[i].king,
                neighborTiles = kings[i].neighborTiles,
                neighborWhiteTiles = kings[i].neighborWhiteTiles,
                neighborBlackTiles = kings[i].neighborBlackTiles,
                captureTiles = kings[i].captureTiles
            });

    }



    private bool LegalMoveChecker(TilesDetails previousTile, TilesDetails newTile)
    {
        if (whiteTurn)
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

    private bool LegalMoveKing(TilesDetails previousTile, TilesDetails newTile)
    {
        foreach (var pt in previousTile.neighborTiles)
        {
            if (pt.tileName == newTile.tileName) return true;
        }
        return false;


    }

    private bool IsCapturePosible()
    {
        if (whiteTurn)
        {
            foreach (var t in previousWhiteTiles)
            {
                Debug.Log("Pionek: " + t.tileName + " " + FurtherCapturePossible(t));
                if (FurtherCapturePossible(t)) return true;
            }
            return false;
        }
        else
        {
            foreach (var t in previousBlackTiles)
            {
                Debug.Log("Pionek: " + t.tileName + " " + FurtherCapturePossible(t));
                if (FurtherCapturePossible(t)) return true;
            }
            return false;
        }
    }

    private bool IsOccupied(string name)
    {
        foreach (var prevW in previousWhiteTiles)
        {
            if (name == prevW.tileName) return true;
        }
        foreach (var prevB in previousBlackTiles)
        {
            if (name == prevB.tileName) return true;
        }
        return false;
    }

    private bool FurtherCapturePossible(TilesDetails previousTile)
    {

        foreach (var pt in previousTile.captureTiles)
        {
            if (!IsOccupied(pt.tileName))
            {
                foreach (var nt in previousTile.neighborTiles)
                {


                    foreach (var t in FindTile(pt.tileName).neighborTiles)
                    {

                        if (t.tileName == nt.tileName)
                        {
                            //Debug.Log("Bicie: " + t.tileName + " sasiad " + nt.tileName);
                            if (whiteTurn)
                            {
                                foreach (var previousB in previousBlackTiles)
                                    if (nt.tileName == previousB.tileName)
                                    {
                                        Debug.Log("Bij dalej na pole: " + pt.tileName);
                                        return true;
                                    }
                            }
                            else
                            {
                                foreach (var previousW in previousWhiteTiles)
                                {
                                    // Debug.Log("white: " + previousW.tileName + " nt " + nt.tileName);
                                    if (nt.tileName == previousW.tileName)
                                    {
                                        Debug.Log("Bij dalej na pole: " + pt.tileName);
                                        return true;
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
        return false;


    }

    private TilesDetails FurtherCaptureTarget(TilesDetails previousTile)
    {
        foreach (var pt in previousTile.captureTiles)
        {
            if (!pt.occupiedBlack && !pt.occupiedWhite)
            {
                foreach (var nt in previousTile.neighborTiles)
                {


                    foreach (var t in FindTile(pt.tileName).neighborTiles)
                    {

                        if (t.tileName == nt.tileName)
                        {
                            if (whiteTurn)
                            {
                                if (FindTile(nt.tileName).occupiedBlack)
                                {
                                    return FindTile(nt.tileName);
                                }
                            }
                            else
                            {
                                if (FindTile(nt.tileName).occupiedWhite)
                                {
                                    return FindTile(nt.tileName);
                                }
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    private TilesDetails FurtherCaptureEnd(TilesDetails previousTile)
    {
        foreach (var pt in previousTile.captureTiles)
        {
            if (!pt.occupiedBlack && !pt.occupiedWhite)
            {
                foreach (var nt in previousTile.neighborTiles)
                {


                    foreach (var t in FindTile(pt.tileName).neighborTiles)
                    {

                        if (t.tileName == nt.tileName)
                        {
                            if (whiteTurn)
                            {
                                if (FindTile(nt.tileName).occupiedBlack)
                                {
                                    return FindTile(pt.tileName);
                                }
                            }
                            else
                            {
                                if (FindTile(nt.tileName).occupiedWhite)
                                {
                                    return FindTile(pt.tileName);
                                }
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    private bool LegalCaptureChecker(TilesDetails previousTile, TilesDetails newTile)
    {
        foreach (var ct in previousTile.captureTiles)
        {
            if (ct.tileName == newTile.tileName)
            {
                for (int i = 0; i < previousTile.neighborTiles.Length; i++)
                    for (int j = 0; j < newTile.neighborTiles.Length; j++)
                    {
                        if (previousTile.neighborTiles[i].tileName == newTile.neighborTiles[j].tileName)
                        {
                            if (whiteTurn)
                            {
                                foreach (var bt in previousBlackTiles)
                                {
                                    if (bt.tileName == previousTile.neighborTiles[i].tileName)
                                    {
                                        if (bt.occupiedBlack)
                                        {
                                            int checkersOnTile = 0;
                                            foreach (var t in currentBlackTiles)
                                                if (bt.tileName == t.tileName) checkersOnTile++;

                                            foreach (var t in currentWhiteTiles)
                                                if (bt.tileName == t.tileName) checkersOnTile++;

                                            Debug.Log("There is: " + checkersOnTile + " checkers on tile: " + newTile.tileName);
                                            if (checkersOnTile == 0) return true;
                                        }
                                        else
                                        {
                                            Debug.Log("Pole: " + previousTile.neighborTiles[i].tileName + " zajete przez biale: " +
                                                previousTile.neighborTiles[i].occupiedWhite + " zajete przez czarne: " +
                                                previousTile.neighborTiles[i].occupiedBlack);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var wt in previousWhiteTiles)
                                {
                                    if (wt.tileName == previousTile.neighborTiles[i].tileName)
                                    {
                                        if (wt.occupiedWhite)
                                        {
                                            int checkersOnTile = 0;
                                            foreach (var t in currentBlackTiles)
                                                if (wt.tileName == t.tileName) checkersOnTile++;

                                            foreach (var t in currentWhiteTiles)
                                                if (wt.tileName == t.tileName) checkersOnTile++;

                                            Debug.Log("There is: " + checkersOnTile + " checkers on tile: " + newTile.tileName);
                                            if (checkersOnTile == 0) return true;
                                        }
                                        else
                                        {
                                            Debug.Log("Pole: " + previousTile.neighborTiles[i].tileName + " zajete przez biale: " +
                                                previousTile.neighborTiles[i].occupiedWhite + " zajete przez czarne: " +
                                                previousTile.neighborTiles[i].occupiedBlack);
                                        }
                                    }
                                }
                            }
                            Debug.Log("Pole 'bicia':" + previousTile.neighborTiles[i].tileName + " oraz " + newTile.neighborTiles[j].tileName);


                        }
                    }
            }
        }
        return false;
    }

    private bool LegalMoveKingAbandon(TilesDetails previousTile, TilesDetails newTile)
    {
        if (whiteTurn)
        {
            int letterP = LetterToInt(previousTile.tileName[0]);
            int letterC = LetterToInt(newTile.tileName[0]);
            int digitP = (int)char.GetNumericValue(previousTile.tileName[1]);
            int digitC = (int)char.GetNumericValue(newTile.tileName[1]);
            int letterDiff = Math.Abs(letterP - letterC);
            int digitDiff = Math.Abs(digitP - digitC);

            Debug.Log("roznica: " + letterP + letterC + "=" + letterDiff + " i " + digitDiff + " =" + digitP + digitC);
            if (letterDiff == digitDiff)
            {
                for (int i = 1; i < letterDiff; i++)
                {
                    int x = digitP - i;
                    string tile = IntToLetter(letterP - i) + x.ToString();
                    Debug.Log("tile: " + x);
                    foreach (var t in previousWhiteTiles)
                    {
                        if (t.tileName == tile)
                        {
                            Debug.Log("Bicie króla zablokowane.");
                            return false;
                        }
                    }
                    int captured = 0;
                    foreach (var t in previousBlackTiles)
                    {
                        if (t.tileName == tile)
                        {
                            captured++;
                        }
                    }
                    if (captured > 0)
                    {
                        //odejmij pionki
                        Debug.Log("Skasowano " + captured + " frajerow");
                        return true;
                    }
                }
                Debug.Log("Poprawny ruch króla");
                return true;
            }
            else
            {
                Debug.Log("Niepoprawny ruch króla");
                return false;
            }
        }
        else
        {

        }

        return false;
    }






    private void PromoteChecker(TilesDetails t)
    {
        t.king = true;
        Debug.Log("Hail the king!");
        //TODO add animation here
    }

    private void EndReached(TilesDetails t)
    {
        switch (t.tileName)
        {
            case "A1":
                if (t.occupiedBlack && t.king == false) PromoteChecker(FindTile("A1"));
                break;

            case "A3":
                if (t.occupiedBlack && t.king == false) PromoteChecker(FindTile("A3"));
                break;

            case "A5":
                if (t.occupiedBlack && t.king == false) PromoteChecker(FindTile("A5"));
                break;

            case "A7":
                if (t.occupiedBlack && t.king == false) PromoteChecker(FindTile("A7"));
                break;

            case "H2":
                if (t.occupiedWhite && t.king == false) PromoteChecker(FindTile("H2"));
                break;

            case "H4":
                if (t.occupiedWhite && t.king == false) PromoteChecker(FindTile("H4"));
                break;

            case "H6":
                if (t.occupiedWhite && t.king == false) PromoteChecker(FindTile("H6"));
                break;

            case "H8":
                if (t.occupiedWhite && t.king == false) PromoteChecker(FindTile("H8"));
                break;
        }
    }





    private char IntToLetter(int l)
    {
        switch (l)
        {
            case 1:
                return 'A';
            case 2:
                return 'B';
            case 3:
                return 'C';
            case 4:
                return 'D';
            case 5:
                return 'E';
            case 6:
                return 'F';
            case 7:
                return 'G';
            case 8:
                return 'H';
            default:
                return '-';
        }
    }
    private int LetterToInt(char l)
    {
        switch (l)
        {
            case 'A':
                return 1;
            case 'B':
                return 2;
            case 'C':
                return 3;
            case 'D':
                return 4;
            case 'E':
                return 5;
            case 'F':
                return 6;
            case 'G':
                return 7;
            case 'H':
                return 8;
            default:
                return 0;
        }
    }


    private void AckCaptureAndContinue(TilesDetails t, string move)
    {
        mandatoryTile = t.tileName;
        FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, move);
        MakePrevoiusListCurrent(currentWhiteTiles, currentBlackTiles, currentKings);
        blackChecker = previousBlackTiles.Count;
        whiteChecker = previousWhiteTiles.Count;
    }

    private void EndGame()
    {
        if (whiteKing + whiteChecker < 1 && blackKing + blackChecker > 0)
        {
            blackWin.enabled = true;
        }
        else
        {
            blackWin.enabled = false;
        }

        if (blackKing + blackChecker < 1 && whiteKing + whiteChecker>0)
        {
            whiteWin.enabled = true;
        }
        else
        {
            whiteWin.enabled = false;
        }
    }

    public void CheckMovement()
    {
        if (BoardDetected() && play)
        {
            CheckCurrentCheckersLocation();
            EndGame();

            if (whiteTurn)
            {
                if (WhiteNumberSame() && OneWhiteCheckerMoved())
                {
                    if (IsCapturePosible())
                    {
                        Debug.Log("Mamy bicie");
                        if (PreviousTile() != null && NewTile() != null)
                        {
                            if (LegalCaptureChecker(PreviousTile(), NewTile()) && currentBlackTiles.Count == previousBlackTiles.Count - 1)
                            {
                                if (mandatoryTile == "")
                                {
                                    Debug.Log("wykonano BICIE");
                                    string moves = "White: " + PreviousTile().tileName+ " -x-> " + NewTile().tileName;
                                    string stat = "";
                                    AckCaptureAndContinue(NewTile(), moves);
                                    if (FurtherCapturePossible(FindTile(mandatoryTile)))
                                    {
                                        stat = "Continue capturing from tile: " + mandatoryTile;
                                        FindObjectOfType<TextManager>().UpdateText(stat);
                                        Debug.Log("Nie koncze tury not yet");
                                    }
                                    else
                                    {
                                        stat = "White captured enemy! turn ended";
                                        FindObjectOfType<TextManager>().UpdateText(stat);
                                        Debug.Log("Koncze ture");
                                        EndReached(FindTile(mandatoryTile));
                                        EndTurn();
                                    }
                                }
                                else
                                {
                                    if (PreviousTile().tileName == mandatoryTile)
                                    {
                                        Debug.Log("wykonano BICIE");
                                        string moves = "White: " + PreviousTile().tileName + " -x-> " + NewTile().tileName;
                                        string stat = "";
                                        AckCaptureAndContinue(NewTile(),moves);
                                        if (FurtherCapturePossible(FindTile(mandatoryTile)))
                                        {
                                            stat = "Continue capturing from tile: " + mandatoryTile;
                                            FindObjectOfType<TextManager>().UpdateText(stat);
                                            Debug.Log("Nie koncze tury not yet");
                                        }
                                        else
                                        {
                                            stat = "White captured enemy! turn ended";
                                            FindObjectOfType<TextManager>().UpdateText(stat);
                                            Debug.Log("Koncze ture");
                                            EndReached(FindTile(mandatoryTile));
                                            EndTurn();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Debug.Log("Musisz wykonać poprawne bicie!");
                                string stat = "You have to capture enemy checker: ";
                                FindObjectOfType<TextManager>().UpdateText(stat);
                            }
                        }
                    }
                    else
                    {
                        if (LegalMoveChecker(PreviousTile(), NewTile()) && BlackNumberSame())
                        {
                            Debug.Log("Wykryto poprawny ruch pionkiem");
                            EndReached(NewTile());
                            string moves = "White: " + PreviousTile().tileName + " ---> " + NewTile().tileName;
                            string stat = "White turn ended";
                            AckCaptureAndContinue(NewTile(),moves);
                            FindObjectOfType<TextManager>().UpdateText(stat);

                            EndTurn();
                        }
                        else
                        {
                            if (PreviousTile().king)
                            {
                                if (LegalMoveKing(PreviousTile(), NewTile()) && BlackNumberSame())
                                {
                                    Debug.Log("Wykryto poprawny ruch królem");
                                    EndReached(NewTile());
                                    string moves = "White: " + PreviousTile().tileName + " ---> " + NewTile().tileName;
                                    string stat1 = "White turn ended";
                                    AckCaptureAndContinue(NewTile(), moves);
                                    FindObjectOfType<TextManager>().UpdateText(stat1);
                                    EndTurn();
                                }
                            }
                            else
                            {
                                Debug.Log("Wykonaj ruch pionkiem białym");
                                string stat = "Move one white checker";
                                FindObjectOfType<TextManager>().UpdateText(stat);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Oczekuje na ruch pionkiem białym");
                    string stat = "Move one white checker";
                    FindObjectOfType<TextManager>().UpdateText(stat);
                }

            }
            else
            {
                if (BlackNumberSame() && OneBlackCheckerMoved())
                {
                    if (IsCapturePosible())
                    {
                        if (PreviousTile() != null && NewTile() != null)
                        {
                            Debug.Log("Mamy bicie");
                            if (LegalCaptureChecker(PreviousTile(), NewTile()) && currentWhiteTiles.Count == previousWhiteTiles.Count - 1)
                            {
                                Debug.Log("Bicie wykoane");
                                if (mandatoryTile == "")
                                {
                                    Debug.Log("wykonano BICIE");
                                    string moves = "Black: " + PreviousTile().tileName + " -x-> " + NewTile().tileName;
                                    string stat = "";
                                    AckCaptureAndContinue(NewTile(), moves);
                                    if (FurtherCapturePossible(FindTile(mandatoryTile)))
                                    {
                                        stat = "Continue capturing from tile: " + mandatoryTile;
                                        FindObjectOfType<TextManager>().UpdateText(stat);
                                        Debug.Log("Nie koncze tury not yet");
                                    }
                                    else
                                    {
                                        stat = "Black captured enemy! Turn ended";
                                        FindObjectOfType<TextManager>().UpdateText(stat);
                                        Debug.Log("Koncze ture");
                                        EndReached(FindTile(mandatoryTile));
                                        EndTurn();
                                    }
                                }
                                else
                                {
                                    if (PreviousTile().tileName == mandatoryTile)
                                    {
                                        Debug.Log("wykonano BICIE");
                                        string moves = "Black: " + PreviousTile().tileName + " -x-> " + NewTile().tileName;
                                        string stat = "";
                                        AckCaptureAndContinue(NewTile(), moves);
                                        if (FurtherCapturePossible(FindTile(mandatoryTile)))
                                        {
                                            Debug.Log("Nie koncze tury not yet");
                                            stat = "Continue capturing from tile: " + mandatoryTile;
                                            FindObjectOfType<TextManager>().UpdateText(stat);
                                        }
                                        else
                                        {
                                            Debug.Log("Koncze ture");
                                            stat = "Black captured enemy! turn ended";
                                            FindObjectOfType<TextManager>().UpdateText(stat);
                                            EndReached(FindTile(mandatoryTile));
                                            EndTurn();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Musisz wykonać poprawne bicie!");
                            string stat = "You have to capture enemy checker: ";
                            FindObjectOfType<TextManager>().UpdateText(stat);
                        }
                    }
                    else
                    {
                        if (PreviousTile() != null && NewTile() != null)
                        {
                            if (LegalMoveChecker(PreviousTile(), NewTile()) && WhiteNumberSame())
                            {
                                Debug.Log("Wykryto poprawny ruch pionkiem");
                                EndReached(NewTile());
                                string moves = "Black: " + PreviousTile().tileName + " ---> " + NewTile().tileName;
                                string stat = "Black turn ended";
                                AckCaptureAndContinue(NewTile(), moves);
                                FindObjectOfType<TextManager>().UpdateText(stat);
                                EndTurn();
                            }
                            else
                            {
                                if (PreviousTile().king)
                                {
                                    if (LegalMoveKing(PreviousTile(), NewTile()) && WhiteNumberSame())
                                    {
                                        Debug.Log("Wykryto poprawny ruch królem");
                                        EndReached(NewTile());
                                        string moves = "Black: " + PreviousTile().tileName + " ---> " + NewTile().tileName;
                                        string stat1 = "Black turn ended";
                                        AckCaptureAndContinue(NewTile(), moves);
                                        FindObjectOfType<TextManager>().UpdateText(stat1);
                                        EndTurn();
                                    }
                                }
                                Debug.Log("Wykonaj ruch pionkiem czarnym");
                                string stat = "Move one black checker";
                                FindObjectOfType<TextManager>().UpdateText(stat);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Oczekuje na ruch pionkiem czarnym");
                    string stat = "Move one black checker";
                    FindObjectOfType<TextManager>().UpdateText(stat);
                }

            }


        }
    }

    public bool BoardDetected()
    {
        if (FindObjectOfType<BoardDetector>().AllTiles().Length == 32)
        {
            FindObjectOfType<TextManager>().DisplayInfoAboutBoardDetection(true);
            return true;
        }
        else
        {
            FindObjectOfType<TextManager>().DisplayInfoAboutBoardDetection(false);
            return false;
        }
    }

    public void PlayButton()
    {
        if (play)
        {
            play = false;
            previousBlackTiles.Clear();
            previousWhiteTiles.Clear();
            currentBlackTiles.Clear();
            currentWhiteTiles.Clear();
            whiteWin.enabled = false;
            blackWin.enabled = false;
            FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, "---", "Game stoped");
        }
        else
        {
            if (BoardDetected())
            {
                var board = FindObjectOfType<BoardDetector>();
                var checkers = FindObjectOfType<CheckersDetection>();

                    for (int i = 0; i < checkers.blackTiles.Count; i++)
                        previousBlackTiles.Add(new TilesDetails
                        {
                            tileName = checkers.blackTiles[i].tileName,
                            TileRepresentation3D = checkers.blackTiles[i].TileRepresentation3D,
                            tilePosition = checkers.blackTiles[i].tilePosition,
                            distanceToMarker = checkers.blackTiles[i].distanceToMarker,
                            occupiedBlack = checkers.blackTiles[i].occupiedBlack,
                            occupiedWhite = checkers.blackTiles[i].occupiedWhite,
                            king = checkers.blackTiles[i].king,
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
                            king = checkers.whiteTiles[i].king,
                            neighborTiles = checkers.whiteTiles[i].neighborTiles,
                            neighborWhiteTiles = checkers.whiteTiles[i].neighborWhiteTiles,
                            neighborBlackTiles = checkers.whiteTiles[i].neighborBlackTiles,
                            captureTiles = checkers.whiteTiles[i].captureTiles
                        });

                    play = true;
                    FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, "---", "Game started");
                    blackChecker = previousBlackTiles.Count;
                    whiteChecker = previousWhiteTiles.Count;
                }
            else
            {
                Debug.Log("Board not detected");
                FindObjectOfType<TextManager>().UpdateText("Board not detected!");
            }
        }
    }

}
