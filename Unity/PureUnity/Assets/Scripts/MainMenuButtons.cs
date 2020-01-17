using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] Canvas rules;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void openCloseRules()
    {
        if (rules.enabled)
        {
            rules.enabled = false;
        }
        else
        {
            rules.enabled = true;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
