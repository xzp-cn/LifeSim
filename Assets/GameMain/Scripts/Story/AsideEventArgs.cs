using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class AsideEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(AsideEventArgs).GetHashCode();
    // Start is called before the first frame update

    public AsideEventArgs()
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

    public override int Id
    {
        get
        {
            return EventId;

        }
    }

    public static GameEventArgs Create(VarInt32 args)
    {
        AsideEventArgs asideEventArgs = ReferencePool.Acquire<AsideEventArgs>();
        asideEventArgs.UserData = (object)args;
        return asideEventArgs;
    }

    public override void Clear()
    {
        UserData = null;
    }
}
