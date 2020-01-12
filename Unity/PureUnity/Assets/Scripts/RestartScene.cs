using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    [SerializeField] Camera [] cameras;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int i = 0;
            foreach (Camera cam in cameras)
            {
                if (i==0)
                {
                    cam.gameObject.SetActive(true);
                    i++;
                }
                else
                {
                    cam.gameObject.SetActive(false);
                    i++;
                }
        }
        }
    }
}
