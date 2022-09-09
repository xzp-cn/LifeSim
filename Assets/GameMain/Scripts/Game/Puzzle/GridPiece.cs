using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GridPiece
{
    public Transform par;

    public GameObject item;

    public EdgeStruct EdgeStruct;

    public void OnInit(Transform _par, GameObject _item, EdgeStruct _struct)
    {
        par = _par;
        item = _item;
        EdgeStruct = _struct;
    }
    /// <summary>
    /// 创建片状
    /// </summary>
    public void OnOpen(GridItem[,] gridArr, int splitNum)
    {

        colorList = new List<Color>();
        for (int i = 0; i < splitNum; i++)
        {
            colorList.Add(RandomColorRGB());
        }
        InitPiece(gridArr);
        ChangeItemColor(gridArr, splitNum);
        PosJust();
        PlacePut();
    }

    public void GetAnswer(GridItem[,] gridArr, int splitNum)
    {
        GameObject go= new GameObject("Answer");
        go.transform.SetParent(par.parent);
        par = go.transform;
        InitPiece(gridArr);
        ChangeItemColor(gridArr, splitNum);
    }

    void InitPiece(GridItem[,] gridArr)
    {
        for (int i = 0; i < gridArr.GetLength(0); i++)
        {
            for (int j = 0; j < gridArr.GetLength(1); j++)
            {
                GameObject go = GameObject.Instantiate(item, par);
                RectTransform rt = go.GetComponent<RectTransform>();
                Vector3 pos = new Vector3(i * rt.rect.width + i * 2f, -j * rt.rect.height - j * 2f, 0);
                rt.anchoredPosition3D = pos;
                go.name = string.Format("{0},{1}", i, j);
            }
        }
    }

    List<Color> colorList = new List<Color>();
    void ChangeItemColor(GridItem[,] gridArr, int spliteNum)
    { 

        for (int i = 0; i < spliteNum; i++)
        {
            RectTransform itemPar = par.Find(i.ToString()) as RectTransform;
            if (itemPar == null)
            {
                itemPar = new GameObject(i.ToString()).AddComponent<RectTransform>();
            }
            itemPar.SetParent(par);
            itemPar.anchoredPosition3D = Vector3.zero;

            int w = 0;
            int h = 0;
            for (int j = 0; j < gridArr.GetLength(0); j++)
            {
                for (int k = 0; k < gridArr.GetLength(1); k++)
                {
                    if (gridArr[j, k].shapeIndex == i)
                    {
                        string str = string.Format("{0},{1}", j, k);
                        try
                        {
                            Transform temp = par.Find(str);
                            temp.SetParent(itemPar);
                            temp.GetComponent<Image>().color = colorList[i];
                        }
                        catch (Exception e)
                        {
                            Debug.Log(str);
                        }
                    }
                }
            }
        }
    }

    void PosJust()
    {
        foreach (Transform _parTransform in par)
        {
            if (_parTransform == par)
            {
                continue;
            }

            Vector2 posDelta = Vector2.zero;
            foreach (Transform _par in _parTransform)
            {
                if (_par == _parTransform)
                {
                    continue;
                }

                RectTransform rt = _par as RectTransform;
                posDelta += rt.anchoredPosition;
            }

            posDelta /= _parTransform.childCount;

            foreach (Transform _par in _parTransform)
            {
                if (_par == _parTransform)
                {
                    continue;
                }

                RectTransform rt = _par as RectTransform;
                rt.anchoredPosition -= posDelta;
            }
        }
    }

    void PlacePut()
    {
        Transform gridPar = par;

        RectTransform rt = item.transform as RectTransform;

        float xL = EdgeStruct.leftFromEdge.position.x + rt.rect.size.x * 5;
        float xR = EdgeStruct.leftFromEdge.position.x + (float)Screen.width / 2 - rt.rect.size.x * 8;
        //
        float xL1 = EdgeStruct.rightToEdge.position.x - (float)Screen.width / 2 + rt.rect.size.x * 8;
        float xR1 = EdgeStruct.rightToEdge.position.x - rt.rect.size.x * 4;
        //屏幕
        float yB = EdgeStruct.bottomEdge.position.y + rt.rect.size.x * 2;
        float yT = EdgeStruct.toEdge.position.y - rt.rect.size.x * 4;

        int i = 0;
        foreach (Transform _parTransform in par)
        {
            if (_parTransform == par)
            {
                continue;
            }

            // Random.InitState(100);
            float x0 = Random.Range(xL, xR);
            float x1 = Random.Range(xL1, xR1);
            float x = i % 2 == 0 ? x0 : x1;
            float y = Random.Range(yB, yT);
            _parTransform.localPosition = gridPar.InverseTransformPoint(new Vector3(x, y, 0));
            i++;
        }
    }

    private Color RandomColorRGB()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        return color;
    }

    public void OnClose()
    {
        for (int i = par.childCount-1; i >=0; i--)
        {
            GameObject.Destroy(par.GetChild(i).gameObject);
        }
    }
}

[System.Serializable]
public struct EdgeStruct
{
    public Transform leftFromEdge;
    public Transform rightToEdge;
    public Transform bottomEdge;
    public Transform toEdge;
}
