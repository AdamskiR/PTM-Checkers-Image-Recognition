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

    void Start()
    {
        turnText.text = "Turn: 0";
        playerText.text = "Player: White";
        playStopText.text = "Play";
        whiteNumber.text = "White: calculating";
        blackNumber.text = "Black: calculating";
        moveText.text = "White first";
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

    public void UpdateText(int turn, bool white, bool play, string status)
    {
        turnText.text = "Turn: " + turn.ToString();

        if (white) playerText.text = "Player: White";
        else playerText.text = "Player: Black";

        if (play) playStopText.text = "Stop";
        else playStopText.text = "Play";

        statusText.text = status;
    }

    public void UpdateCheckersNumber(int white, int black)
    {
        whiteNumber.text = "White: " + white.ToString();
        blackNumber.text = "Black: " + black.ToString();
    }

}
