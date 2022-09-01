using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class PlotOverEventArgs : GameEventArgs
{

    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(PlotOverEventArgs).GetHashCode();
    // Start is called before the first frame update

    public PlotOverEventArgs()
    {
        UserData = null;
    }
    /// <summary>
    /// 获取用户自定义数据。
    /// </summary>
    public object UserData
    {
        get;
        private set;
    }

    public bool isOverStory
    {
        get;
        set;
    }

    public override int Id
    {
        get
        {
            return EventId;

        }
    }

    public static GameEventArgs Create(VarInt32 args,bool _isOverStory)
    {
        PlotOverEventArgs plotOverEventArgs = ReferencePool.Acquire<PlotOverEventArgs>();
        plotOverEventArgs.UserData = (object)args;
        plotOverEventArgs.isOverStory = _isOverStory;
        return plotOverEventArgs;
    }

    public override void Clear()
    {
        UserData = null;
    }
}
