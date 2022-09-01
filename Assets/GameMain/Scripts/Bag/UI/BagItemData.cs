using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BagItemData
{
    public string name;
    public string imageName;
    public string inforText;
    public int type;
}


public class BagItemGridData
{
    public BagItemData bagData;
    public int num;
}
