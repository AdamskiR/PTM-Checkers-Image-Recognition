using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] Canvas settings;

    void Start()
    {
        
    }

   public void Test()
    {
        FindObjectOfType<ScreenUpdater>().ReadCamera();
    }

    

    public void EnableSettingsCanvas()
    {
        if (settings.enabled)
        {
            settings.enabled = false;
        }
        else
        {
            settings.enabled = true;
        }
    }



}
