using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class getPixels : MonoBehaviour
{
    int mapaX = 1380, mapaY =820;
    public int[,] mapa;
    int nextgroup = 2;
    int[] groupNumber;
    public AllGroups ListOfAllGroups = new AllGroups();
   // public AllGroups ListOfAllTiles = new AllGroups();

    public int[] big;
    [SerializeField] int grupaK;

    // Start is called before the first frame update
    void Start()
    {
        mapa = new int [mapaX, mapaY];
        groupNumber = new int[99];
        big = new int[20];
        
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            ReadColors();
        }
    }



    [System.Serializable]
    public class SingleGroup
    {
        public List<Vector2Int> list;
    }

    [System.Serializable]
    public class AllGroups
    {
       public List<SingleGroup> list;
    }


    void ReadColors()
    {
        // List < List int[,] > Pixel;

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

                    float r = image.GetPixel(x, y).r;
                    float g = image.GetPixel(x, y).g;
                    float b = image.GetPixel(x, y).b;

                    if (r > 0.76f && g < 0.5f && b < 0.5f)
                    {
                        Color rgb = Color.black;
                        mapa[x, y] = 1;
                        newTex.SetPixel(x, y, Color.black);

                    }
                    else
                    {
                        mapa[x, y] = 0;
                    }

                }
            }

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
                            if (mapa[x - z, y] > 1)
                            {
                                mapa[x, y] = mapa[x - z, y];

                                int index = mapa[x, y] - 2;
                                ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));

                                groupNumber[mapa[x - z, y]]++;
                                break;
                            }
                            if (mapa[x + z, y] > 1)
                            {
                                mapa[x, y] = mapa[x + z, y];

                                int index = mapa[x, y] - 2;
                                ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));

                                groupNumber[mapa[x + z, y]]++;
                                break;
                            }
                            if (mapa[x, y - z] > 1)
                            {
                                mapa[x, y] = mapa[x, y - z];


                                int index = mapa[x, y] - 2;
                                ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                                groupNumber[mapa[x, y - z]]++;
                                break;
                            }
                            if (mapa[x, y + z] > 1)
                            {
                                mapa[x, y] = mapa[x, y + z];

                                int index = mapa[x, y] - 2;
                                ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));

                                groupNumber[mapa[x, y + z]]++;
                                break;
                            } // TODO merge this
                        }

                        if (mapa[x, y] == 1)
                        {
                            ListOfAllGroups.list.Add(new SingleGroup());
                            int index = ListOfAllGroups.list.Count - 1;
                            ListOfAllGroups.list[index].list = new List<Vector2Int>();
                            ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));

                            mapa[x, y] = nextgroup;
                            groupNumber[nextgroup]++;
                            nextgroup++;
                            Debug.Log("nowa grupa");
                        }
                    }
                }
            } //END OF FOR

            int ijk = 0;
            for (int ll = 0; ll < ListOfAllGroups.list.Count; ll++)
            {
                
                if (ListOfAllGroups.list[ll].list.Count > 3000)
                {
                    big[ijk] = ll;
                    ijk++;
                }
            }


            for (int ll = 0; ll < ListOfAllGroups.list.Count; ll++)
            {
                if (ListOfAllGroups.list[ll].list.Count < 3000)
                {
                    Vector2Int v1 = new Vector2Int(ListOfAllGroups.list[ll].list[0].x, ListOfAllGroups.list[ll].list[0].y);
                    for (int la = 0; la < 8; la++)
                    {
                        Vector2Int v2 = new Vector2Int(ListOfAllGroups.list[big[la]].list[0].x, ListOfAllGroups.list[big[la]].list[0].y);
                        
                        if (Vector2Int.Distance(v1,v2) < 30)
                        {
                            ListOfAllGroups.list[big[la]].list.AddRange(ListOfAllGroups.list[la].list);
                            Debug.Log(Vector2Int.Distance(v1, v2) + " Between: " + ll + "and"+ big[la]);
                        }
                    }
                }

            }

          /*  float a, bb, c, d;
            for (int ll = 0; ll < ListOfAllGroups.list.Count; ll++)
            {
                a = 1.0f * 0.02f * ll;
                bb = 1.0f * 0.01f * ll;
                c = 1.0f * 0.03f * ll;
                d = 1;
                foreach (Vector2Int v in ListOfAllGroups.list[ll].list)
                {
                    newTex.SetPixel(v.x, v.y, new Color(a,bb,c,d));
                }
            }*/ //KILL ME PLS //



               

                 



            newTex.Apply();

            var Bytes1 = newTex.EncodeToPNG();
            Destroy(newTex);

            File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + 1 + "Pola.png", Bytes1);

        }
    }

}
