using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class ReadColorGreeninHSV : MonoBehaviour
{
    [SerializeField] string imagePath = "/CameraPictures/CameraInput.png";

    [Header("RGB Colors")]
    [SerializeField] [Range(0, 1)] float HvalueMin = 0.4f;
    [SerializeField] [Range(0, 1)] float HvalueMax = 0.6f;
    [SerializeField] [Range(0, 1)] float Svalue = 0.6f;
    [SerializeField] [Range(0, 1)] float Vvalue = 0.3f;

    [Header("Picture details")]
    [SerializeField][Range(0,100)] float ObjectsDetectionRange = 50;
    public List<Vector2> middlePoints;
    public AllGroups ListOfAllGroups = new AllGroups();

    public int[,] mapa;
    int mapaX = 1380, mapaY = 820;
    int nextgroup = 2;

    public Slider sliderHvalueMin;
    public Slider sliderHvalueMax;
    public Slider sliderSvalue;
    public Slider sliderVvalue;
    public Slider sliderRange;

    public void SubmitSliderSetting()
    {
        HvalueMax = sliderHvalueMax.value;
        HvalueMin = sliderHvalueMin.value;
        Svalue = sliderSvalue.value;
        Vvalue = sliderVvalue.value;
        ObjectsDetectionRange = sliderRange.value;
    }

    void Start()
    {
        mapa = new int[mapaX, mapaY];
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


    public void ReadColors()
    {
        ListOfAllGroups.list.Clear();
        middlePoints.Clear();
        nextgroup = 2;

        Texture2D image = null;
        byte[] fileData;

        if (File.Exists(Application.dataPath + imagePath))
        {
            fileData = File.ReadAllBytes(Application.dataPath + imagePath);
            image = new Texture2D(2, 2);
            image.LoadImage(fileData);

            Texture2D newTex = new Texture2D(image.width, image.height);

            for (int x = 0; x < newTex.width; x++)
            {
                for (int y = 0; y < newTex.height; y++)
                {

                    var rgb = image.GetPixel(x, y);
                    float H, S, V;
                    Color.RGBToHSV(rgb, out H, out S, out V);

                    if (H > HvalueMin && H < HvalueMax && S > Svalue && V > Vvalue)
                    {
                        mapa[x, y] = 1;
                        //Color rgb = Color.black;
                        //newTex.SetPixel(x, y, Color.black);
                    }
                    else
                    {
                        mapa[x, y] = 0;
                    }

                }
            }

            //newTex.Apply();

            //var Bytes = newTex.EncodeToPNG();
            //File.WriteAllBytes(Application.dataPath + "/CameraPictures/CameraInputMapGreen.png", Bytes);


            #region(Grupowanie pixeli)
            for (int x = 0; x < newTex.width; x++)
            {
                for (int y = 0; y < newTex.height; y++)
                {
                    if (mapa[x, y] == 1)
                    {

                        for (int z = 1; z < 10; z++)
                        {
                            try
                            {
                            if (mapa[x - z, y] > 1)
                            {
                                mapa[x, y] = mapa[x - z, y];
                                int index = mapa[x, y] - 2;
                                ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                                break;
                            }

                            if (mapa[x + z, y] > 1)
                            {
                                mapa[x, y] = mapa[x + z, y];
                                int index = mapa[x, y] - 2;
                                ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                                break;
                            }
                            if (mapa[x, y - z] > 1)
                            {
                                mapa[x, y] = mapa[x, y - z];
                                int index = mapa[x, y] - 2;
                                ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                                break;
                            }
                                if (mapa[x, y + z] > 1)
                                {
                                    mapa[x, y] = mapa[x, y + z];
                                    int index = mapa[x, y] - 2;
                                    ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                // przy krawedziach catch bedzie dzialal
                            }
                        }

                        if (mapa[x, y] == 1)
                        {
                            ListOfAllGroups.list.Add(new SingleGroup());
                            int index = ListOfAllGroups.list.Count - 1;
                            ListOfAllGroups.list[index].list = new List<Vector2Int>();
                            ListOfAllGroups.list[index].list.Add(new Vector2Int(x, y));
                            mapa[x, y] = nextgroup;
                            nextgroup++;
                        }
                    }
                }
            } //END OF FOR
            #endregion


            #region(Łączenie grup pixeli)
            for (int ll = 0; ll < ListOfAllGroups.list.Count; ll++)
            {
                if (ListOfAllGroups.list[ll].list.Count > 0)
                {
                    for (int la = 0; la < ListOfAllGroups.list.Count; la++)
                    {
                        if (ll != la && ListOfAllGroups.list[la].list.Count > 0)
                        {
                            Vector2Int v1 = new Vector2Int(ListOfAllGroups.list[ll].list[0].x, ListOfAllGroups.list[ll].list[0].y);
                            foreach (Vector2Int point in ListOfAllGroups.list[la].list)
                            {
                                Vector2Int v2 = point;
                                if (Vector2Int.Distance(v1, v2) < ObjectsDetectionRange)
                                {
                                    ListOfAllGroups.list[ll].list.AddRange(ListOfAllGroups.list[la].list);
                                    ListOfAllGroups.list[la].list.Clear();
                                    break;
                                }
                            }

                        }
                    }
                }
            }
            #endregion


            #region(Wyznaczenie środków grupy)
            for (int ll = 0; ll < ListOfAllGroups.list.Count; ll++)
            {
                int minX = 1300, minY = 880, maxX = 0, maxY = 0;
                if (ListOfAllGroups.list[ll].list.Count > 10)
                {
                    foreach (Vector2Int v in ListOfAllGroups.list[ll].list)
                    {
                        if (v.x < minX) minX = v.x;
                        if (v.y < minY) minY = v.y;
                        if (v.x > maxX) maxX = v.x;
                        if (v.y > maxY) maxY = v.y;
                    }
                    middlePoints.Add(new Vector2((minX + maxX) / 2, (minY + maxY) / 2));
                }

            }
            #endregion


            #region(kolorowanie grup)
            /*
            float a, bb, c, d;
            for (int ll = 0; ll < ListOfAllGroups.list.Count; ll++)
            {
                if (ListOfAllGroups.list[ll].list.Count > 10)
                {
                    Debug.Log("Pole: " + ll);
                    a = 1.0f * 0.06f * (ll + 1);
                    bb = 1.0f * 0.02f * (ll + 1);
                    c = 1.0f * 0.03f * (ll + 1);
                    d = 1;
                    foreach (Vector2Int v in ListOfAllGroups.list[ll].list)
                    {
                        newTex.SetPixel(v.x, v.y, new Color(a, bb, c, d));
                    }
                }
            } //KILL ME PLS //


            newTex.Apply();

            var Bytes1 = newTex.EncodeToPNG();
            Destroy(newTex);

            File.WriteAllBytes(Application.dataPath + "/CameraPictures/CameraInputTiles.png", Bytes1);
            */
            #endregion

        }
    }

}
