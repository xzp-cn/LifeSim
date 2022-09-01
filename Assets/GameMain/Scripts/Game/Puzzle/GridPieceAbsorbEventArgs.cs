using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class GridPieceAbsorbEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(GridPieceAbsorbEventArgs).GetHashCode();
    // Start is called before the first frame update

    public GridPieceAbsorbEventArgs()
    {

    }
    /// <summary>
    /// 获取用户自定义数据。
    /// </summary>
    public object UserData
    {
        get;
        private set;
    }

    public override int Id
    {
        get
        {
            return EventId;
        }
    }

    public static GridPieceAbsorbEventArgs Create(string args)
    {
        GridPieceAbsorbEventArgs absorbEventArgs = ReferencePool.Acquire<GridPieceAbsorbEventArgs>();
        absorbEventArgs.UserData = (object)args;
        return absorbEventArgs;
    }

    public override void Clear()
    {

    }
}
