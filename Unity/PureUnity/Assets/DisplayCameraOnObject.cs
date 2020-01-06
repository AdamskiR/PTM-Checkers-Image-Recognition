using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DisplayCameraOnObject : MonoBehaviour
{
    public WebCamTexture webCamTexture;
    WebCamDevice[] devices;

    void Start()
    {
        devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture();
        webCamTexture.deviceName = devices[0].name;
        webCamTexture.Play();

        GetComponent<Renderer>().material.mainTexture = webCamTexture;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            TakePhoto();
        }
    }

    public void TakePhoto()  
    {

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        
        byte[] bytes = photo.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/CameraPictures/CameraInput.png", bytes);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 80, 40), "Switch\n Camera"))
        {
            webCamTexture.Stop();
            webCamTexture.deviceName = (webCamTexture.deviceName == devices[0].name) ? devices[1].name : devices[0].name;
            webCamTexture.Play();

        }
    }
}