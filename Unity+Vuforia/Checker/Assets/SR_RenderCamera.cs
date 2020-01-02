using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SR_RenderCamera : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] RawImage img;
    [SerializeField] RenderTexture renderTexture;

    public int FileCounter = 0;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            CamCapture();
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            LoadPNG(Application.dataPath + "/Backgrounds/" + (FileCounter-1) + ".png");
        }
    }

    void CamCapture()
    {
        Camera Cam = cam;

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;


        Cam.Render();

        Graphics.Blit(null, renderTexture);

        if (Cam != null)
        {
            Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
            Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
            Image.Apply();
            RenderTexture.active = currentRT;

            var Bytes = Image.EncodeToPNG();
            Destroy(Image);

           

            File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + FileCounter + ".png", Bytes);
            FileCounter++;
        }
    }


    public Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            img.texture = tex;
        }
        return tex;
    }

}
