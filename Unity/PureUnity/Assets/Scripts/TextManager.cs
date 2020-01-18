using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextManager : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI playStopText;
    [SerializeField] TextMeshProUGUI whiteNumber;
    [SerializeField] TextMeshProUGUI blackNumber;
    [SerializeField] TextMeshProUGUI moveText;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] TextMeshProUGUI boardDetectedtext;

    void Start()
    {
        turnText.text = "Turn: 0";
        playerText.text = "Player: White";
        playStopText.text = "Play";
        whiteNumber.text = "White: 0";
        blackNumber.text = "Black: 0";
        moveText.text = "Last move: Not make yet";
        statusText.text = "Welcome! \nGood luck, have fun";
    }

    public void UpdateText(bool white, string move)
    {
        if (white) playerText.text = "Player: White";
        else playerText.text = "Player: Black";

        moveText.text = move;
    }

    public void UpdateText(int turn, bool white, bool play, string move, string status)
    {
        turnText.text = "Turn: " + turn.ToString();

        if (white) playerText.text = "Player: White";
        else playerText.text = "Player: Black";

        if (play) playStopText.text = "Stop";
        else playStopText.text = "Play";

        moveText.text = move;
        statusText.text = status;
    }

    public void UpdateText(string status)
    {
        statusText.text = status;
    }

    public void UpdateText(int turn, bool white, bool play, string move)
    {
        turnText.text = "Turn: " + turn.ToString();

        if (white) playerText.text = "Player: White";
        else playerText.text = "Player: Black";

        if (play) playStopText.text = "Stop";
        else playStopText.text = "Play";

        moveText.text = move;
    }

    public void UpdateCheckersNumber(int white, int black)
    {
        whiteNumber.text = "White: " + white.ToString();
        blackNumber.text = "Black: " + black.ToString();
    }
    public void DisplayInfoAboutBoardDetection(bool boardDetected)
    {
        if (boardDetected) boardDetectedtext.text = "Board Detected";
        else boardDetectedtext.text = "Board NOT Detected";
    }

}
