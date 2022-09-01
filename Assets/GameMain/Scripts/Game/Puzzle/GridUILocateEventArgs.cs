using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class GridUILocateEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(GridUILocateEventArgs).GetHashCode();
    // Start is called before the first frame update

    public GridUILocateEventArgs()
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
    
    public static GridUILocateEventArgs Create(object args)
    {
        GridUILocateEventArgs gridUiLocateEvent = ReferencePool.Acquire<GridUILocateEventArgs>();
        gridUiLocateEvent.UserData = args;
        return gridUiLocateEvent;
    }

    public override void Clear()
    {

    }
}

