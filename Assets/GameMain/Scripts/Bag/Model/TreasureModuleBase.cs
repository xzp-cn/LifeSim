using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreasureModuleBase
{
    protected Vector3[] posArr;
    public virtual void Init(Vector3[] _posArr)
    {
        posArr= _posArr;
    }

}
