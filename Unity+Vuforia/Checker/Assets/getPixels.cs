using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class getPixels : MonoBehaviour
{
    int mapaX = 2000, mapaY =1200;
    public int[,] mapa;
    int nextgroup = 2;
    int[] groupNumber;

    // Start is called before the first frame update
    void Start()
    {
        mapa = new int [mapaX, mapaY];
        groupNumber = new int[99];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            ReadColors();
        }
    }

    public struct pixel
    {
        public int xP, yP;
        public pixel(int x, int y)
        {
            xP = x;
            yP = y;
        }
    }

    void ReadColors()
    {
        Texture2D image = null;
        byte[] fileData;

        if (File.Exists(Application.dataPath + "/Backgrounds/" + 0 + ".png"))
        {
            fileData = File.ReadAllBytes(Application.dataPath + "/Backgrounds/" + 0 + ".png");
            image = new Texture2D(2, 2);
            image.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            
            Texture2D newTex = new Texture2D(image.width, image.height);
           
            for (int x = 0; x < newTex.width; x++)
            {
                for (int y = 0; y < newTex.height; y++)
                {

                    float r =image.GetPixel(x, y).r;
                    float g = image.GetPixel(x, y).g;
                    float b = image.GetPixel(x, y).b;

                    if (r > 0.76f && g < 0.5f && b <0.5f)
                    {
                        Color rgb = Color.black;
                        mapa[x, y] = 1;
                        newTex.SetPixel(x, y, Color.black);
                          
                        }
                    else
                    {
                        mapa[x, y] = 0;
                        //Color rgb = image.GetPixel(x, y);
                        //newTex.SetPixel(x, y, rgb);
                    }
                    //Debug.Log("color r: " + r);
                    
                }
            }
            /*
                 newTex.SetPixel(minX+i, minY + i, Color.green);
                 newTex.SetPixel(minX + i, maxY + i, Color.green);
                 newTex.SetPixel(maxX + i, minY + i, Color.green);
                 newTex.SetPixel(maxX + i, maxY + i, Color.green);
             */

           newTex.Apply();

            var Bytes = newTex.EncodeToPNG();
           //Destroy(newTex);

            File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + 1 + "Mapa.png", Bytes);


            for (int x = 0; x < newTex.width; x++)
            {
                for (int y = 0; y < newTex.height; y++)
                {
                    if (mapa[x, y] == 1)
                    {
                        
                        for (int z = 1; z < 20; z++)
                        {
                            if (mapa[x - z, y] > 1) { mapa[x, y] = mapa[x - z, y]; groupNumber[mapa[x - z, y]]++; break; }
                            if (mapa[x + z, y] > 1) { mapa[x, y] = mapa[x + z, y]; groupNumber[mapa[x + z, y]]++; break; }
                            if (mapa[x, y - z] > 1) { mapa[x, y] = mapa[x, y - z]; groupNumber[mapa[x , y - z]]++; break; }
                            if (mapa[x, y + z] > 1) { mapa[x, y] = mapa[x, y + z]; groupNumber[mapa[x , y + z]]++; break; } // TODO merge this
                        }

                        if (mapa[x, y] == 1)
                        {
                            mapa[x, y] = nextgroup;
                            groupNumber[nextgroup]++;
                            nextgroup++;
                            Debug.Log("nowa grupa");
                        }
                    }
                }
            }

            for (int t = 0; t < nextgroup; t++)
            {
                if (groupNumber[t] > 3300)
                {
                    Debug.Log(t);

                }
            }

                for (int x = 0; x < newTex.width; x++)
            {
                for (int y = 0; y < newTex.height; y++)
                {
                    if (mapa[x,y] == 2)
                    {
                        newTex.SetPixel(x, y, Color.green);
                    }

                    if (mapa[x, y] == 6)
                    {
                        newTex.SetPixel(x, y, Color.yellow);
                    }

                    if (mapa[x, y] == 8)
                    {
                        newTex.SetPixel(x, y, Color.cyan);
                    }

                    if (mapa[x, y] == 10)
                    {
                        newTex.SetPixel(x, y, Color.yellow);
                    }

                    if (mapa[x, y] == 32)
                    {
                        newTex.SetPixel(x, y, Color.blue);
                    }

                    if (mapa[x, y] == 65)
                    {
                        newTex.SetPixel(x, y, Color.magenta);
                    }

                    if (mapa[x, y] == 66)
                    {
                        newTex.SetPixel(x, y, Color.red);
                    }

                    if (mapa[x, y] == 69)
                    {
                        newTex.SetPixel(x, y, Color.white);
                    }
                }
            }


                    newTex.Apply();

            var Bytes1 = newTex.EncodeToPNG();
            Destroy(newTex);

            File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + 1 + "Pola.png", Bytes1);

        }
    }

}
