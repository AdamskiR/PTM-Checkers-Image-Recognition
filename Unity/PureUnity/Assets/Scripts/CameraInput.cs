using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraInput : MonoBehaviour
{
    public WebCamTexture webCamTexture;
    WebCamDevice[] devices;

    void Start()
    {
        devices = WebCamTexture.devices;
        webCamTexture = new WebCamTexture();
        webCamTexture.deviceName = devices[0].name;
        webCamTexture.Play();

        // GetComponent<Renderer>().material.mainTexture = webCamTexture;
        GetComponent<Image>().defaultMaterial.mainTexture = webCamTexture;
    }

    public void TakePhoto()  
    {

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        byte[] bytes = photo.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/CameraPictures/CameraInput.png", bytes);
    }

    public void SwitchWebcamera()
    {
        webCamTexture.Stop();
        webCamTexture.deviceName = (webCamTexture.deviceName == devices[0].name) ? devices[1].name : devices[0].name;
        webCamTexture.Play();
    }

   /* void OnGUI()
    {
        GUIStyle customButton = ("button");
        customButton.fontSize = 36;

        if (GUI.Button(new Rect(0, 0, 200, 120), "Switch \nCamera"))
        {
            webCamTexture.Stop();
            webCamTexture.deviceName = (webCamTexture.deviceName == devices[0].name) ? devices[1].name : devices[0].name;
            webCamTexture.Play();
        }
    }*/

}