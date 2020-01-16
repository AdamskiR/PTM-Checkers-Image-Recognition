using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMaster : MonoBehaviour
{
    [Header("time")]
    [SerializeField] int turn = 0;
    [SerializeField] bool whiteTurn = true;
    [SerializeField] bool play;

    [Header("checkers position")]
    [SerializeField] List<TilesDetails> previousWhiteTiles;
    [SerializeField] List<TilesDetails> previousBlackTiles;
    [SerializeField] List<TilesDetails> currentWhiteTiles;
    [SerializeField] List<TilesDetails> currentBlackTiles;

    public int same = 0;
    bool capturingPending = false;


    void Start()
    {
        turn = 0;
        whiteTurn = true;
    }

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
                    Debug.Log("Found: " + currentWhiteTiles[j].tileName + previousWhiteTiles[i].tileName);
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

    private bool FurtherCapturePossible(TilesDetails previousTile)
    {
        foreach (var pt in previousTile.captureTiles)
        {
            if (!pt.occupiedBlack && !pt.occupiedWhite)
            {
                foreach (var nt in previousTile.neighborTiles)
                {


                    foreach (var t in FindTile(pt.tileName).neighborTiles)
                    {
                        Debug.Log("Bicie: " + t.tileName + " sasiad " + nt.tileName);
                        if (t.tileName == nt.tileName)
                        {
                            if (FindTile(nt.tileName).occupiedBlack)
                            {
                                Debug.Log("Bij dalej na pole: " + pt.tileName);
                                return true;
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
                        Debug.Log("Bicie: " + t.tileName + " sasiad " + nt.tileName);
                        if (t.tileName == nt.tileName)
                        {
                            if (FindTile(nt.tileName).occupiedBlack)
                            {
                                return FindTile(nt.tileName);
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
                        Debug.Log("Bicie: " + t.tileName + " sasiad " + nt.tileName);
                        if (t.tileName == nt.tileName)
                        {
                            if (FindTile(nt.tileName).occupiedBlack)
                            {
                                return FindTile(pt.tileName);
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

    private bool LegalMoveKing(TilesDetails previousTile, TilesDetails newTile)
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

    private void MakePrevoiusListCurrent(List<TilesDetails> checkersW, List<TilesDetails> checkersB)
    {
        previousBlackTiles.Clear();
        previousWhiteTiles.Clear();
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
    }

    private void MoveWasMade()
    {
        string move = "";
        if (whiteTurn) { move = "Move: W " + PreviousTile().tileName + " -> " + NewTile().tileName; }
        else { move = "Move: B " + PreviousTile().tileName + " -> " + NewTile().tileName; }
        MakePrevoiusListCurrent(currentWhiteTiles, currentBlackTiles);
        if (!whiteTurn)
        {
            turn++;
            whiteTurn = true;
            FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, move, "Wykonano ruch");
        }
        else
        {
            whiteTurn = false;
            FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, move, "Wykonano ruch");

        }

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

    private void PromoteChecker(TilesDetails t)
    {
        t.king = true;
        Debug.Log("Hail the king!");
        //TODO add animation here
    }

    private void EndReach(TilesDetails t)
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

    private void CheckCurrentCheckersLocation()
    {
        currentBlackTiles = new List<TilesDetails>(FindObjectOfType<CheckersDetection>().blackTiles);
        currentWhiteTiles = new List<TilesDetails>(FindObjectOfType<CheckersDetection>().whiteTiles);
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

    IEnumerator ContinueCaptureing(TilesDetails finalPosition)
    {
        Debug.Log("Bij na pole: " + FurtherCaptureEnd(finalPosition));
        if (FurtherCaptureTarget(finalPosition).occupiedBlack && !FurtherCaptureEnd(finalPosition).occupiedWhite && !OneWhiteCheckerMoved())
            capturingPending = false;
        else
        {
            Debug.Log("Bij na pole: " + FurtherCaptureEnd(finalPosition));
            yield return new WaitForSeconds(1);
        }

    }

    public void CheckMovement()
    {
        if (BoardDetected() && play)
        {
            CheckCurrentCheckersLocation();

            if (whiteTurn)
            {
                if (!capturingPending)
                if (WhiteNumberSame() && OneWhiteCheckerMoved())
                {
                    if (PreviousTile() != null && NewTile() != null)
                    {
                        //Debug.Log("Zmiana tile'a: " + PreviousTile().tileName + " na " + NewTile().tileName);
                        if (LegalMoveChecker(PreviousTile(), NewTile()) && BlackNumberSame())
                        {
                            //EndReach(NewTile());
                            MoveWasMade();
                        }
                        else
                        {
                            //Debug.Log("Nie wykonano ruchu bez bicia");
                            //Debug.Log("Bicie: " + PreviousTile().tileName + "->->->" + NewTile().tileName);

                            if (LegalCaptureChecker(PreviousTile(), NewTile()))
                            {
                                TilesDetails finalPosition = new TilesDetails
                                {
                                    tileName = NewTile().tileName,
                                    TileRepresentation3D = NewTile().TileRepresentation3D,
                                    tilePosition = NewTile().tilePosition,
                                    distanceToMarker = NewTile().distanceToMarker,
                                    occupiedBlack = NewTile().occupiedBlack,
                                    occupiedWhite = NewTile().occupiedWhite,
                                    king = NewTile().king,
                                    neighborTiles = NewTile().neighborTiles,
                                    neighborWhiteTiles = NewTile().neighborWhiteTiles,
                                    neighborBlackTiles = NewTile().neighborBlackTiles,
                                    captureTiles = NewTile().captureTiles
                                };

                                string move = "";
                                if (whiteTurn) { move = "Move: W " + PreviousTile().tileName + " -> " + NewTile().tileName; }
                                else { move = "Move: B " + PreviousTile().tileName + " -> " + NewTile().tileName; }
                                MakePrevoiusListCurrent(currentWhiteTiles, currentBlackTiles);
                                FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, move, "Wykonano ruch");

                                for (int i=0;i<12;i++)
                                {
                                    Debug.Log("Można dalej bić: " + FurtherCapturePossible(finalPosition)+" bo na "+finalPosition.tileName);
                                    if (FurtherCapturePossible(finalPosition))
                                    {
                                        capturingPending = true;
                                        StartCoroutine(ContinueCaptureing(finalPosition));
                                        break;
                                        if (whiteTurn) { move = "Move: W " + PreviousTile().tileName + " -> " + NewTile().tileName; }
                                        else { move = "Move: B " + PreviousTile().tileName + " -> " + NewTile().tileName; }
                                        MakePrevoiusListCurrent(currentWhiteTiles, currentBlackTiles);

                                    }
                                    else
                                    {
                                        Debug.Log("Nie można dalej bić: " + FurtherCapturePossible(finalPosition));
                                        break;
                                    }
                                }
                                if (!whiteTurn && !capturingPending)
                                {
                                    turn++;
                                    whiteTurn = true;
                                    FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, move, "Wykonano ruch");
                                }
                                else
                                {
                                    whiteTurn = false;
                                    FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, move, "Wykonano ruch");

                                }
                            }
                            else
                            {
                                //Debug.Log("Nie wykonano bicia");
                                //if (LegalMoveKing(PreviousTile(), NewTile()));
                                //{
                                //     MoveWasMade();
                                //}
                            }

                        }
                    }

                    FindObjectOfType<TextManager>().UpdateCheckersNumber(previousWhiteTiles.Count, previousBlackTiles.Count);
                }


            }
            else
            {
                if (BlackNumberSame() && OneBlackCheckerMoved())
                {
                    if (PreviousTile() != null && NewTile() != null)
                    {
                        //Debug.Log("Zmiana tile'a: " + PreviousTile().tileName + " na " + NewTile().tileName);
                        if (LegalMoveChecker(PreviousTile(), NewTile()) && WhiteNumberSame())
                        {
                            MoveWasMade();
                        }
                        else
                        {
                            //Debug.Log("Nie wykonano ruchu bez bicia");


                            if (LegalCaptureChecker(PreviousTile(), NewTile()))
                            {
                                MoveWasMade();
                            }
                            //else Debug.Log("Nie wykonano bicia");


                        }

                        FindObjectOfType<TextManager>().UpdateCheckersNumber(previousWhiteTiles.Count, previousBlackTiles.Count);

                    }
                    else Debug.Log("tiles are null ffks");
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
            previousBlackTiles.Clear();
            previousWhiteTiles.Clear();
            currentBlackTiles.Clear();
            currentWhiteTiles.Clear();
            FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, "white first", "Gra zatrzymana");
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
                    FindObjectOfType<TextManager>().UpdateText(turn, whiteTurn, play, "white first", "Gra wystartowała");
                }
            }
            else
            {
                Debug.Log("Board not detected");
            }
        }
    }

}
