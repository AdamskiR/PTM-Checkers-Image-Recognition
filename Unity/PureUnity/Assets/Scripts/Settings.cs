using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] Canvas settings;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
