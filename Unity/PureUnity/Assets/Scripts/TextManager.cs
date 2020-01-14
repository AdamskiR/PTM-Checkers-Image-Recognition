using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    /* NIE USUWAC
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI playStopText;
    [SerializeField] TextMeshProUGUI whiteNumber;
    [SerializeField] TextMeshProUGUI blackNumber;
    */

    [SerializeField] Text turnText;
    [SerializeField] Text playerText;
    [SerializeField] Text playStopText;
    [SerializeField] Text whiteNumber;
    [SerializeField] Text blackNumber;

    void Start()
    {
        turnText.text = "Turn: 0";
        playerText.text = "Player: White";
        playStopText.text = "Play";
        whiteNumber.text = "White: calculating";
        blackNumber.text = "Black: calculating";
    }

    // Update is called once per frame
    public void UpdateText(int turn, bool white, bool play)
    {
        turnText.text = "Turn: " + turn.ToString();

        if (white) playerText.text = "Player: White";
        else playerText.text = "Player: Black";

        if (play) playStopText.text = "Stop";
        else playStopText.text = "Play";
    }

    public void UpdateCheckersNumber(int white, int black)
    {
        whiteNumber.text = "White: " + white.ToString();
        blackNumber.text = "Black: " + black.ToString();
    }

}
