using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnText;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI playStopText;

    void Start()
    {
        turnText.text = "Turn: 0";
        playerText.text = "Player: White";
        playStopText.text = "Play";
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

}
