using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class QuesFreshEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(QuesFreshEventArgs).GetHashCode();

    public QuesFreshEventArgs()
    {

    }

    /// <summary>
    /// 获取用户自定义数据。
    /// </summary>
    public object UserData { get; private set; }

    public override int Id
    {
        get { return EventId; }
    }

    public static QuesFreshEventArgs Create(VarInt32 args)
    {
        QuesFreshEventArgs quesFreshEventArgs = ReferencePool.Acquire<QuesFreshEventArgs>();
        quesFreshEventArgs.UserData = (object) args;
        return quesFreshEventArgs;
    }


    public override void Clear()
    {

    }
}
